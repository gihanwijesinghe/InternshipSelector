using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MBBSInternship.Models;
using Omu.AwesomeMvc;
using MBBSInternship.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace MBBSInternship.Controllers
{
    public class PeopleController : Controller
    {
        private readonly InternshipContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public PeopleController(InternshipContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        // GET: People
        public async Task<IActionResult> Index()
        {
            var internshipContext = _context.Persons.Include(p => p.University);
            return View(await internshipContext.ToListAsync());
        }

        // GET: People/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.University)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        public IActionResult MealsGrid(GridParams g, int id)
        {
            //name = (name ?? string.Empty).ToLower();
            var query = _context.PersonHospitalRelationships as IQueryable<PersonHospitalRelationship>;
            query = query.Where(r => r.PersonId == id).Include(r => r.Hospital);
            var newQuery = query.Include(r => r.Hospital)
                .Select(r => new PersonHospitalViewModel
                {
                    Id = r.Id,
                    HospitalName = r.Hospital.Name,
                    TotalSlots = r.Hospital.TotalSlots.Value,
                    DistrictName = r.Hospital.District.Name,
                    PreferenceNumber = r.PreferenceNumber
                })
                .OrderBy(r => r.PreferenceNumber);

            return Json(new GridModelBuilder<PersonHospitalViewModel>(newQuery, g) { Key = "Id" }.Build());
        }

        public IActionResult GetList()
        {
            var query = _context.PersonHospitalRelationships as IQueryable<PersonHospitalRelationship>;
            var model = query.Include(r => r.Hospital)
                             .ToList();
            //ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id");
            //ViewData["Gender"] = new SelectList(Enum.GetNames(typeof(Gender)));
            return Json(model);
        }

        // GET: People/Create
        public IActionResult Preference(int id)
        {
            var query = _context.PersonHospitalRelationships as IQueryable<PersonHospitalRelationship>;
            var model = query.Where(r => r.PersonId == id)
                            .Include(r => r.Hospital)
                             .ToList();
            //ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id");
            //ViewData["Gender"] = new SelectList(Enum.GetNames(typeof(Gender)));
            return View(id);
        }

        [HttpPost]
        public async Task<IActionResult> ReOrderPreferences([FromBody] PreferenceList preferences)
        {
            var query = _context.PersonHospitalRelationships as IQueryable<PersonHospitalRelationship>;
            var count = 0;
            foreach (var id in preferences.ChoiceIds)
            {
                count++;
                var item = query.Where(r => r.Id == id).FirstOrDefault();
                item.PreferenceNumber = count;
                _context.Update(item);
            }
            _context.SaveChanges();
            return Json(new { success = true });
        }

        // GET: People/Create
        public IActionResult Create()
        {
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id");
            ViewData["Gender"] = new SelectList(Enum.GetNames(typeof(Gender)));
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,UniversityId,Rank,NIC,Gender")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id", person.UniversityId);
            return View(person);
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Name,UniversityId,Rank,NIC,Gender")] Person person)
        {
            if (ModelState.IsValid)
            {
                person.Password = person.Name + "@" + person.Rank;
                _context.Add(person);
                var user = new ApplicationUser
                {
                    Name = person.Name,
                    UserName = person.Password,
                    Email = person.Email
                };
                IdentityResult result = await userManager.CreateAsync(user, person.Password);
                if (result.Succeeded)
                {
                    ApplicationRole applicationRole = await roleManager.FindByNameAsync("Intern");
                    if (applicationRole != null)
                    {
                        IdentityResult roleResult = await userManager.AddToRoleAsync(user, applicationRole.Name);
                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id", person.UniversityId);
            return View(person);
        }

        // GET: People/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id", person.UniversityId);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UniversityId,Rank,NIC,Gender")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UniversityId"] = new SelectList(_context.Universities, "Id", "Id", person.UniversityId);
            return View(person);
        }

        // GET: People/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.Persons
                .Include(p => p.University)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.Persons.Any(e => e.Id == id);
        }
    }
}

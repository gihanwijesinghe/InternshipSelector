using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MBBSInternship.Models;

namespace MBBSInternship.Controllers
{
    public class PersonHospitalRelationshipController : Controller
    {
        private readonly InternshipContext _context;

        public PersonHospitalRelationshipController(InternshipContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> UpdateHospitalList()
        {
            var personsQuery = _context.Persons as IQueryable<Person>;
            var hospitalsQuery = _context.Hospitals as IQueryable<Hospital>;
            var hospitals = hospitalsQuery.ToList();
            personsQuery = personsQuery.Include(p => p.HospitalChoices);
            var persons = personsQuery.ToList();
            foreach (var person in persons)
            {
                var choices = person.HospitalChoices.Select(c => c.HospitalId);
                var remainingHospitals = hospitals.Where(h => !choices.Any(c => c == h.Id));
                var count = choices.Count();
                foreach(var remHospital in remainingHospitals)
                {
                    count++;
                    var relationship = new PersonHospitalRelationship
                    {
                        PreferenceNumber = count,
                        PersonId = person.Id,
                        HospitalId = remHospital.Id
                    };
                    _context.Add(relationship);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Preference", "People");
        }

        // GET: PersonHospitalRelationship
        public async Task<IActionResult> Index()
        {
            var internshipContext = _context.PersonHospitalRelationships.Include(p => p.Hospital).Include(p => p.Person);
            return View(await internshipContext.ToListAsync());
        }

        // GET: PersonHospitalRelationship/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personHospitalRelationship = await _context.PersonHospitalRelationships
                .Include(p => p.Hospital)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personHospitalRelationship == null)
            {
                return NotFound();
            }

            return View(personHospitalRelationship);
        }

        // GET: PersonHospitalRelationship/Create
        public IActionResult Create()
        {
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id");
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id");
            return View();
        }

        // POST: PersonHospitalRelationship/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PreferenceNumber,PersonId,HospitalId")] PersonHospitalRelationship personHospitalRelationship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personHospitalRelationship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", personHospitalRelationship.HospitalId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id", personHospitalRelationship.PersonId);
            return View(personHospitalRelationship);
        }

        // GET: PersonHospitalRelationship/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personHospitalRelationship = await _context.PersonHospitalRelationships.FindAsync(id);
            if (personHospitalRelationship == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", personHospitalRelationship.HospitalId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id", personHospitalRelationship.PersonId);
            return View(personHospitalRelationship);
        }

        // POST: PersonHospitalRelationship/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PreferenceNumber,PersonId,HospitalId")] PersonHospitalRelationship personHospitalRelationship)
        {
            if (id != personHospitalRelationship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personHospitalRelationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonHospitalRelationshipExists(personHospitalRelationship.Id))
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
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", personHospitalRelationship.HospitalId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id", personHospitalRelationship.PersonId);
            return View(personHospitalRelationship);
        }

        // GET: PersonHospitalRelationship/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personHospitalRelationship = await _context.PersonHospitalRelationships
                .Include(p => p.Hospital)
                .Include(p => p.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (personHospitalRelationship == null)
            {
                return NotFound();
            }

            return View(personHospitalRelationship);
        }

        // POST: PersonHospitalRelationship/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personHospitalRelationship = await _context.PersonHospitalRelationships.FindAsync(id);
            _context.PersonHospitalRelationships.Remove(personHospitalRelationship);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonHospitalRelationshipExists(int id)
        {
            return _context.PersonHospitalRelationships.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MBBSInternship.Models;
using MBBSInternship.ViewModels;

namespace MBBSInternship.Controllers
{
    public class SelectedHospitalLogController : Controller
    {
        private readonly InternshipContext _context;

        public SelectedHospitalLogController(InternshipContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> RunLog()
        {
            var logTime = DateTime.Now.ToUniversalTime() + TimeSpan.Parse("05:30:00");

            var globalSettingsQuery = _context.GlobalSettings as IQueryable<GlobalSettings>;
            var personsQuery = _context.Persons as IQueryable<Person>;
            var hospitalsQuery = _context.Hospitals as IQueryable<Hospital>;

            var globalSettings = globalSettingsQuery.FirstOrDefault();
            var persons = personsQuery.OrderBy(p => p.Rank).Include(p => p.HospitalChoices).ToList();
            var hospitals = hospitalsQuery.Select(h => new HospitalViewModel
            {
                Id = h.Id,
                TotalSlots = h.TotalSlots.Value,
                RemainingSlots = h.TotalSlots.Value
            }).ToList();
            foreach(var person in persons)
            {
                var hospitalChoices = person.HospitalChoices.OrderBy(c => c.PreferenceNumber).Select(c => c.HospitalId).ToList();
                foreach (var hospitalChoiceId in hospitalChoices)
                {
                    var remSlots = hospitals.Where(h => h.Id == hospitalChoiceId).FirstOrDefault().RemainingSlots;
                    if(remSlots > 0)
                    {
                        hospitals.Where(h => h.Id == hospitalChoiceId).FirstOrDefault().RemainingSlots = remSlots - 1;
                        var log = new SelectedHospitalLog
                        {
                            CurrentTime = DateTime.Now.ToUniversalTime() + TimeSpan.Parse("05:30:00"),
                            LogNumber = globalSettings.LogIndex + 1,
                            LogTime = logTime,
                            PersonId = person.Id,
                            HospitalId = hospitalChoiceId
                        };
                        _context.Add(log);
                    }
                    break;
                }
                globalSettings.LogIndex = globalSettings.LogIndex + 1;
                globalSettings.LogTime = logTime;
                _context.Update(globalSettings);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: SelectedHospitalLog
        public async Task<IActionResult> Index()
        {
            var lastLog = _context.GlobalSettings.FirstOrDefault().LogIndex;
            var internshipContext = _context.SelectedHospitalLogs.Where(s => s.LogNumber == lastLog)
                .Include(s => s.Hospital)
                .Include(s => s.Person);
            return View(await internshipContext.ToListAsync());
        }

        // GET: SelectedHospitalLog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedHospitalLog = await _context.SelectedHospitalLogs
                .Include(s => s.Hospital)
                .Include(s => s.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (selectedHospitalLog == null)
            {
                return NotFound();
            }

            return View(selectedHospitalLog);
        }

        // GET: SelectedHospitalLog/Create
        public IActionResult Create()
        {
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id");
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id");
            return View();
        }

        // POST: SelectedHospitalLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LogNumber,LogTime,CurrentTime,PersonId,HospitalId")] SelectedHospitalLog selectedHospitalLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(selectedHospitalLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", selectedHospitalLog.HospitalId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id", selectedHospitalLog.PersonId);
            return View(selectedHospitalLog);
        }

        // GET: SelectedHospitalLog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedHospitalLog = await _context.SelectedHospitalLogs.FindAsync(id);
            if (selectedHospitalLog == null)
            {
                return NotFound();
            }
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", selectedHospitalLog.HospitalId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id", selectedHospitalLog.PersonId);
            return View(selectedHospitalLog);
        }

        // POST: SelectedHospitalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,LogNumber,LogTime,CurrentTime,PersonId,HospitalId")] SelectedHospitalLog selectedHospitalLog)
        {
            if (id != selectedHospitalLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(selectedHospitalLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SelectedHospitalLogExists(selectedHospitalLog.Id))
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
            ViewData["HospitalId"] = new SelectList(_context.Hospitals, "Id", "Id", selectedHospitalLog.HospitalId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "Id", selectedHospitalLog.PersonId);
            return View(selectedHospitalLog);
        }

        // GET: SelectedHospitalLog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var selectedHospitalLog = await _context.SelectedHospitalLogs
                .Include(s => s.Hospital)
                .Include(s => s.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (selectedHospitalLog == null)
            {
                return NotFound();
            }

            return View(selectedHospitalLog);
        }

        // POST: SelectedHospitalLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var selectedHospitalLog = await _context.SelectedHospitalLogs.FindAsync(id);
            _context.SelectedHospitalLogs.Remove(selectedHospitalLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SelectedHospitalLogExists(int id)
        {
            return _context.SelectedHospitalLogs.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BCSH2_SEM.Data;
using BCSH2_SEM.Models;

namespace BCSH2_SEM.Controllers
{
    public class SessionsController : Controller
    {
        private readonly BCSH2_SEMContext _context;

        public SessionsController(BCSH2_SEMContext context)
        {
            _context = context;
        }

        // GET: Sessions
        public async Task<IActionResult> Index(string searchMember, string searchTrainer, string searchSessionType)
        {
            
            var sessions = _context.Session
                .Include(s => s.Member)
                .Include(s => s.Trainer)
                .AsQueryable();

            
            if (!string.IsNullOrEmpty(searchMember))
            {
                sessions = sessions.Where(s => (s.Member.FirstName + " " + s.Member.LastName).Contains(searchMember));
            }

            if (!string.IsNullOrEmpty(searchTrainer))
            {
                sessions = sessions.Where(s => (s.Trainer.FirstName + " " + s.Trainer.LastName).Contains(searchTrainer));
            }

           
            if (!string.IsNullOrEmpty(searchSessionType))
            {
                sessions = sessions.Where(s => s.SessionType == searchSessionType);
            }

            var viewModel = new SessionFilterViewModel
            {
                SearchMember = searchMember,
                SearchTrainer = searchTrainer,
                SearchSessionType = searchSessionType,
                Sessions = await sessions.ToListAsync(),
                SessionTypes = new List<string> { "Exercise", "Yoga", "Cardio", "Strength Training", "Pilates" }
            };
            ViewBag.SessionTypes = new SelectList(new List<string>
    {
        "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training"
    });

            return View(viewModel);
        }




        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Session == null)
            {
                return NotFound();
            }

            var session = await _context.Session
                .Include(s => s.Member)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create()
        {
            // Retrieve Members and Trainers
            ViewBag.Members = new SelectList(
        _context.Member.Select(m => new
        {
            m.Id,
            FullName = $"{m.FirstName} {m.LastName}"
        }),
        "Id",
        "FullName");
            ViewBag.Trainers = new SelectList(
        _context.Trainer.Select(t => new
        {
            t.Id,
            FullName = $"{t.FirstName} {t.LastName}"
        }),
        "Id",
        "FullName");

            ViewBag.SessionTypes = new SelectList(new List<string>
    {
        "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training"
    });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Session session)
        {
            ModelState.Remove("Trainer");
            ModelState.Remove("Member");
            if (ModelState.IsValid)
            {
                _context.Add(session);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        
            else {
                Console.WriteLine("Validation Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
}

            // Repopulate dropdowns if validation fails
            ViewBag.Members = new SelectList(
                    _context.Member.Select(m => new
                    {
                        m.Id,
                        FullName = $"{m.FirstName} {m.LastName}"
                    }),
                    "Id",
                    "FullName");
            ViewBag.Trainers = new SelectList(
        _context.Trainer.Select(t => new
        {
            t.Id,
            FullName = $"{t.FirstName} {t.LastName}"
        }),
        "Id",
        "FullName");
            ViewBag.SessionTypes = new SelectList(new List<string>
    {
        "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training"
    });

            return View(session);
        }


        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Session == null)
            {
                return NotFound();
            }

            var session = await _context.Session.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewBag.Members = new SelectList(
                    _context.Member.Select(m => new
                    {
                        m.Id,
                        FullName = $"{m.FirstName} {m.LastName}"
                    }),
                    "Id",
                    "FullName");
            ViewBag.Trainers = new SelectList(
        _context.Trainer.Select(t => new
        {
            t.Id,
            FullName = $"{t.FirstName} {t.LastName}"
        }),
        "Id",
        "FullName");

            ViewBag.SessionTypes = new SelectList(new List<string>
    {
        "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training",
        "CrossFit"
    });
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionDate,Duration,SessionType,MemberId,TrainerId")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Trainer");
            ModelState.Remove("Member");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
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
            ViewData["MemberId"] = new SelectList(_context.Member, "Id", "Id", session.MemberId);
            ViewData["TrainerId"] = new SelectList(_context.Set<Trainer>(), "Id", "Id", session.TrainerId);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Session == null)
            {
                return NotFound();
            }

            var session = await _context.Session
                .Include(s => s.Member)
                .Include(s => s.Trainer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Session == null)
            {
                return Problem("Entity set 'BCSH2_SEMContext.Session'  is null.");
            }
            var session = await _context.Session.FindAsync(id);
            if (session != null)
            {
                _context.Session.Remove(session);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
          return (_context.Session?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

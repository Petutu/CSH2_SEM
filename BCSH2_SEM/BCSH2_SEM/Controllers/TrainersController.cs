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
    public class TrainersController : Controller
    {
        private readonly BCSH2_SEMContext _context;

        public TrainersController(BCSH2_SEMContext context)
        {
            _context = context;
        }

        // GET: Trainers
        public async Task<IActionResult> Index(string searchFirstName, string searchLastName, string searchSpecialization)
        {
       
            var trainers = from t in _context.Trainer
                           select t;

         
            if (!string.IsNullOrEmpty(searchFirstName))
            {
                trainers = trainers.Where(t => t.FirstName.Contains(searchFirstName));
            }

            
            if (!string.IsNullOrEmpty(searchLastName))
            {
                trainers = trainers.Where(t => t.LastName.Contains(searchLastName));
            }

            if (!string.IsNullOrEmpty(searchSpecialization))
            {
                trainers = trainers.Where(t => t.Specialization == searchSpecialization);
            }

            
            ViewBag.Specializations = new SelectList(new List<string>
    {
        "Pilates",
        "Strength Training",
        "Cardio",
        "CrossFit",
        "Yoga"
    }, searchSpecialization); 
         
            var viewModel = new TrainerFilterViewModel
            {
                SearchFirstName = searchFirstName,
                SearchLastName = searchLastName,
                SearchSpecialization = searchSpecialization,
                Trainers = await trainers.ToListAsync()
            };

            return View(viewModel);
        }

        // GET: Trainers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Trainer == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // GET: Trainers/Create
        public IActionResult Create()
        {
            ViewBag.Specializations = new SelectList(new List<string>
{
    "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training",
        "CrossFit"
});
            return View();
        }

        // POST: Trainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Specialization")] Trainer trainer)
        {
            ModelState.Remove("Sessions");
            Console.WriteLine(ModelState.IsValid);
            if (ModelState.IsValid)
            {
                _context.Add(trainer);
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
            ViewBag.Specializations = new SelectList(new List<string>
{
    "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training",
        "CrossFit"
});
            return View(trainer);
        }


        // GET: Trainers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Trainer == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer.FindAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }
            ViewBag.Specializations = new SelectList(new List<string>
{
    "Exercise",
        "Yoga",
        "Cardio",
        "Strength Training",
        "CrossFit"
});
            return View(trainer);
        }

        // POST: Trainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Specialization")] Trainer trainer)
        {
            if (id != trainer.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Sessions");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainerExists(trainer.Id))
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
            return View(trainer);
        }

        // GET: Trainers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Trainer == null)
            {
                return NotFound();
            }

            var trainer = await _context.Trainer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            return View(trainer);
        }

        // POST: Trainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Trainer == null)
            {
                return Problem("Entity set 'BCSH2_SEMContext.Trainer'  is null.");
            }
            var trainer = await _context.Trainer.FindAsync(id);
            if (trainer != null)
            {
                _context.Trainer.Remove(trainer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainerExists(int id)
        {
          return (_context.Trainer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

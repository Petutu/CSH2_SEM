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
    public class MembersController : Controller
    {
        private readonly BCSH2_SEMContext _context;

        public MembersController(BCSH2_SEMContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index(string searchFirstName, string searchLastName, string searchMembershipType)
        {
            var members = from m in _context.Member select m;

            if (!string.IsNullOrEmpty(searchFirstName))
            {
                members = members.Where(m => m.FirstName.Contains(searchFirstName));
            }

            if (!string.IsNullOrEmpty(searchLastName))
            {
                members = members.Where(m => m.LastName.Contains(searchLastName));
            }

            if (!string.IsNullOrEmpty(searchMembershipType))
            {
                members = members.Where(m => m.MembershipType == searchMembershipType);
            }

            var viewModel = new MemberFilterViewModel
            {
                SearchFirstName = searchFirstName,
                SearchLastName = searchLastName,
                SearchMembershipType = searchMembershipType,
                Members = await members.ToListAsync()
            };

            ViewBag.MembershipTypes = new SelectList(new List<string>
    {
        "Monthly",
        "Annual",
        "Pay-As-You-Go"
    });

            return View(viewModel);
        }


        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewBag.MembershipTypes = new SelectList(new List<string>
    {
        "Monthly",
        "Annual",
        "Pay-As-You-Go"
    });
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,MembershipType,JoinDate")] Member member)
        {
            ModelState.Remove("Sessions");
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("Validation Errors:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            ViewBag.MembershipTypes = new SelectList(new List<string>
    {
        "Monthly",
        "Annual",
        "Pay-As-You-Go"
    });

            return View(member);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewBag.MembershipTypes = new SelectList(new List<string>
    {
        "Monthly",
        "Annual",
        "Pay-As-You-Go"
    });
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,MembershipType,JoinDate")] Member member)
        {
            if (id != member.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Sessions");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.Id))
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
            return View(member);
        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Member == null)
            {
                return Problem("Entity set 'BCSH2_SEMContext.Member'  is null.");
            }
            var member = await _context.Member.FindAsync(id);
            if (member != null)
            {
                _context.Member.Remove(member);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
          return (_context.Member?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

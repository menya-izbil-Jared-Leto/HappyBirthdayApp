using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HappyBirthdayApp.Data;
using HappyBirthdayApp.Models;

namespace HappyBirthdayApp.Controllers
{
    public class BirthdaysController : Controller
    {
        private readonly HappyBirthdayAppContext _context;

        public BirthdaysController(HappyBirthdayAppContext context)
        {
            _context = context;
        }

        public IActionResult ComingHBs()
        {
            DateTime today = DateTime.Today;

            var model = new ComingHBs();

            model.UpcomingList = _context.Birthday
                .Where(b => b.Date.Month >= today.Month && b.Date.Year != today.Year)
                .OrderBy(b => b.Date)
                .ToList();

            return View(model);
        }

        // GET: Birthdays
        public async Task<IActionResult> Index()
        {
            return _context.Birthday != null ?
                        View(await _context.Birthday.ToListAsync()) :
                        Problem("Entity set HappyBirthdayAppContext.Birthday'  is null.");
        }

        // GET: Birthdays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Birthday == null)
            {
                return NotFound();
            }

            var birthday = await _context.Birthday
                .FirstOrDefaultAsync(m => m.Id == id);
            if (birthday == null)
            {
                return NotFound();
            }

            return View(birthday);
        }

        // GET: Birthdays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birthdays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Img,Name,Date")] Birthday birthday, IFormFile img)
        {
            if (img != null && img.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    img.CopyTo(memoryStream);
                    birthday.Img = memoryStream.ToArray();
                }
            }
            //else 
            //{ 
            //    img = null;
            //}

            _context.Add(birthday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Birthdays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Birthday == null)
            {
                return NotFound();
            }

            var birthday = await _context.Birthday.FindAsync(id);
            if (birthday == null)
            {
                return NotFound();
            }
            return View(birthday);
        }

        // POST: Birthdays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Img,Name,Date")] Birthday birthday, IFormFile img)
        {
            if (id != birthday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(birthday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirthdayExists(birthday.Id))
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
            return View(birthday);
        }

        // GET: Birthdays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Birthday == null)
            {
                return NotFound();
            }

            var birthday = await _context.Birthday
                .FirstOrDefaultAsync(m => m.Id == id);
            if (birthday == null)
            {
                return NotFound();
            }

            return View(birthday);
        }

        // POST: Birthdays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Birthday == null)
            {
                return Problem("Entity set 'HBAppContext.Birthday'  is null.");
            }
            var birthday = await _context.Birthday.FindAsync(id);
            if (birthday != null)
            {
                _context.Birthday.Remove(birthday);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirthdayExists(int id)
        {
            return (_context.Birthday?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

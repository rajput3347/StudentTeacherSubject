using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentTeacherSubject.Data;
using StudentTeacherSubject.Models;

namespace StudentTeacherSubject.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SubjectController(ApplicationDbContext Context) 
        {
            _context = Context;
        }

       
        public async Task<IActionResult> Index()
        {
            var subjects = await _context.Subjects
                .Include(s => s.Languages)
                .ToListAsync();
            return View(subjects);
        }

      
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Class")] Subject subject, List<string> languages)
        {
            if (ModelState.IsValid)
            {
                subject.Languages = languages
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .Select(l => new Language { Name = l })
                    .ToList();
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Languages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Class")] Subject subject, List<string> languages)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSubject = await _context.Subjects
                        .Include(s => s.Languages)
                        .FirstOrDefaultAsync(s => s.Id == id);

                    existingSubject.Name = subject.Name;
                    existingSubject.Class = subject.Class;

                    existingSubject.Languages = languages
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => new Language { Name = l })
                        .ToList();

                    _context.Update(existingSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(subject);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subject = await _context.Subjects
                .Include(s => s.Languages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }

    }
}

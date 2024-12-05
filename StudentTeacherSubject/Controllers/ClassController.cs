using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTeacherSubject.Data;
using StudentTeacherSubject.Models;
using StudentTeacherSubject.ViewModel;

namespace StudentTeacherSubject.Controllers
{
    public class ClassController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClassController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            return View(await _context.Classes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Class classModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect to list of classes or a relevant action
            }
            return View(classModel);
        }

        public async Task<IActionResult> AssignTeachers(int id)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                return NotFound();
            }

            var allTeachers = await _context.Teachers.ToListAsync();

            var viewModel = new AssignTeachersViewModel
            {
                Class = classEntity,
                AvailableTeachers = allTeachers
                    .Where(t => !classEntity.Teachers.Contains(t))
                    .ToList()
            };

            return View(viewModel);
        }

        // POST: Class/AssignTeachers/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTeachers(int id, int[] teacherIds)
        {
            var classEntity = await _context.Classes
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (classEntity == null)
            {
                return NotFound();
            }

            var teachers = await _context.Teachers
                .Where(t => teacherIds.Contains(t.Id))
                .ToListAsync();

            classEntity.Teachers.Clear(); // Clear existing assignments

            foreach (var teacher in teachers)
            {
                classEntity.Teachers.Add(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Redirect to the class list or a relevant page
        }

    }
}

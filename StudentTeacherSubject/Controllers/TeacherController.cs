using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentTeacherSubject.Data;

using StudentTeacherSubject.Models;
using StudentTeacherSubject.ViewModel;

namespace StudentTeacherSubject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public TeacherController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teachers.Include(t => t.Subjects).ToListAsync());
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
             .Include(m => m.Subjects)
  
                   .FirstOrDefaultAsync(m => m.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Age,Sex,ImagePath")] Teacher teacher, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                
                if (imageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    teacher.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(_hostEnvironment.WebRootPath + "/images/teachers/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }
                }
               
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Sex,ImagePath")] Teacher teacher, IFormFile imageFile)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
              
                    string uploadDir = Path.Combine(_hostEnvironment.WebRootPath, "images/teachers");
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }
                    
                 
                    if (imageFile != null)
{
                        string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string extension = Path.GetExtension(imageFile.FileName);
                        teacher.ImagePath = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(uploadDir, fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            return View(teacher);
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images/teachers", teacher.ImagePath);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AssignSubjects(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            var subjects = await _context.Subjects.ToListAsync();

            var viewModel = new AssignSubjectsViewModel
            {
                Teacher = teacher,
                Subjects = subjects,
                AssignedSubjectIds = teacher.Subjects.Select(s => s.Id).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignSubjects(int id, AssignSubjectsViewModel model)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return NotFound();
            }

            teacher.Subjects.Clear();

            if (model.AssignedSubjectIds != null)
            {
                foreach (var subjectId in model.AssignedSubjectIds)
                {
                    var subject = await _context.Subjects.FindAsync(subjectId);
                    if (subject != null)
                    {
                        teacher.Subjects.Add(subject);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AssignTeacherToClass(int teacherId, int classId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            var classEntity = await _context.Classes.FindAsync(classId);

            if (teacher == null || classEntity == null)
            {
                return NotFound();
            }

            if (!teacher.Classes.Contains(classEntity))
            {
                teacher.Classes.Add(classEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Redirect to a relevant action
        }

       

    }
}

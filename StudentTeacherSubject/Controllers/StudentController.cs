using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentTeacherSubject.Data;
using StudentTeacherSubject.Models;
using StudentTeacherSubject.ViewModel;

namespace StudentTeacherSubject.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public StudentController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.Include(s => s.Subjects).ToListAsync());
        }


        public async Task<IActionResult> ClassList(string searchString, string studentClass)
        {
            var students = from s in _context.Students
                           select s;

            if (!string.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.Name.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(studentClass))
            {
                students = students.Where(s => s.Class == studentClass);
            }

            var studentClassVM = new ClassGroupViewModel
            {
                Classes = new SelectList(await _context.Students.Select(s => s.Class).Distinct().ToListAsync()),
                Students = await students.ToListAsync()
            };

            return View(studentClassVM);

        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,FatherName,Address,Age,Class,RollNumber")] Student student, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string path = Path.Combine(wwwRootPath + "/images/students/", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    student.ImagePath = "/images/students/" + fileName;
                }

                try
                {
                  
                    _context.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException.Message.Contains("IX_Students_RollNumber"))
                    {
                        ModelState.AddModelError("RollNumber", "Roll number must be unique.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(student);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Subjects)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FatherName,Address,Age,Class,RollNumber,ImagePath")] Student student, IFormFile imageFile)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/images/students/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        student.ImagePath = "/images/students/" + fileName;
                    }

                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            return View(student);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }



        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(student.ImagePath))
            {
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, student.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult AddStudentSubject(int id)
        {
            var student = _context.Students
                .Include(s => s.Subjects)
                .FirstOrDefault(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new AddSubjectViewModel
            {
                StudentId = student.Id,
                Subjects = _context.Subjects.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddSubjectToStudent(int studentId, int subjectId)
        {
            var student = _context.Students
                .Include(s => s.Subjects)
                .FirstOrDefault(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);
            if (subject != null && !student.Subjects.Contains(subject))
            {
                student.Subjects.Add(subject);
                _context.SaveChanges();
            }

            return RedirectToAction("Subjects", new { id = studentId });
        }

        public async Task<IActionResult> Subjects(int id)
        {
            var student = await _context.Students
                .Include(s => s.Subjects)
                .ThenInclude(sub => sub.Teachers) // Ensure teachers are included
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            var viewModel = new StudentSubjectsViewModel
            {
                StudentName = student.Name,
                Subjects = student.Subjects.Select(sub => new StudentSubjectsViewModel.SubjectWithTeacherAndClass
                {
                    SubjectName = sub.Name,
                    Class = sub.Class,
                    Teachers = sub.Teachers.Select(t => t.Name).ToList() // Collect teacher names
                }).ToList()
            };

            return View(viewModel);
        }


    }
}

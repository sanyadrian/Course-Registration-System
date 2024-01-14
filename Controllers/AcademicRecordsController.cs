using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab6.DataAccess;
using Org.BouncyCastle.Crypto.Generators;
using Microsoft.Identity.Client;

namespace Lab6.Controllers
{
    public class AcademicRecordsController : Controller
    {
        private readonly StudentrecordContext _context;

        private readonly ILogger<AcademicRecordsController> _logger;

        public AcademicRecordsController(ILogger<AcademicRecordsController> logger, StudentrecordContext context)
        {
            _logger = logger;
            _context = context;
        }
        public string NameSort { get; set; }
        public string CourseSort { get; set; }
        public string GradeSort { get; set; }
        public string CurrentSort { get; set; }

        private string CompareBy {get; set;}

        public int AcademicRecordComparer(AcademicRecord r1, AcademicRecord r2, string sortOrder)
{
    if (r1.Grade == null && r2.Grade != null) return -1;
    if (r1.Grade != null && r2.Grade == null) return 1;
    if (r1.Grade == null && r2.Grade == null) return 0;

    switch (sortOrder)
    {
        case "Course":
            return string.Compare(r1.CourseCodeNavigation.Title, r2.CourseCodeNavigation.Title);

        case "course_desc":
            return string.Compare(r2.CourseCodeNavigation.Title, r1.CourseCodeNavigation.Title);

        case "name_desc":
            return string.Compare(r1.Student.Name, r2.Student.Name);

        default:
            return 0;
    }
}
        
        // GET: AcademicRecords
        public async Task<IActionResult> Index(string sortOrder)
        {
        var studentrecordContext = await _context.AcademicRecords
                                                .Include(a => a.CourseCodeNavigation)
                                                .Include(a => a.Student)
                                                .ToListAsync();

        ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewBag.CourseSort = sortOrder == "Course" ? "course_desc" : "Course";
        ViewBag.CurrentSort = sortOrder;

        // var academicRecordsList = studentrecordContext.ToList();

        if (!string.IsNullOrEmpty(sortOrder))
    {
        studentrecordContext.Sort((r1, r2) => AcademicRecordComparer(r1, r2, sortOrder));
    }
        return View(studentrecordContext);
        }

        

        // GET: AcademicRecords/Details/5

        // GET: AcademicRecords/Create
        public IActionResult Create()
        {
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code");
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }


        // GET: AcademicRecords/Edit/5
        public async Task<IActionResult> Edit(string id, string code)
        {
            if (id == null || _context.AcademicRecords == null)
            {
                return NotFound();
            }

            // var academicRecord = await _context.AcademicRecords.FindAsync(id);
            var academicrecord = await _context.AcademicRecords
                                .Include(ar => ar.Student)
                                .Include(ar => ar.CourseCodeNavigation)
                                .FirstOrDefaultAsync(ar => ar.StudentId == id && ar.CourseCode == code);
            if (academicrecord == null)
            {
                return NotFound();
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicrecord.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicrecord.StudentId);
            return View(academicrecord);
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CourseCode,StudentId,Grade")] AcademicRecord academicRecord)
        {
            if (id != academicRecord.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicRecordExists(academicRecord.StudentId))
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
            // ViewData["CourseCode"] = new SelectList(_context.Courses, "Code", "Code", academicRecord.CourseCode);
            // ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", academicRecord.StudentId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Delete/5
        

        private bool AcademicRecordExists(string id)
        {
          return (_context.AcademicRecords?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> EditAll(string sortOrder)
        {
            var studentrecordContext = await _context.AcademicRecords
                                                    .Include(a => a.CourseCodeNavigation)
                                                    .Include(a => a.Student)
                                                    .ToListAsync();

            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CourseSort = sortOrder == "Course" ? "course_desc" : "Course";
            ViewBag.CurrentSort = sortOrder;

            if (!string.IsNullOrEmpty(sortOrder))
                {
                    studentrecordContext.Sort((r1, r2) => AcademicRecordComparer(r1, r2, sortOrder));
                }
        return View(studentrecordContext);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAll(List<AcademicRecord> academicRecords)
        {
            if (academicRecords == null)
            {
                _logger.LogError("academicRecords is null.");
                return NotFound("academicRecords is null.");
            }

            if (!academicRecords.Any())
            {
                _logger.LogWarning("academicRecords is empty.");
                return NotFound("academicRecords is empty.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var record in academicRecords)
                {
                    record.CourseCodeNavigation = await _context.Courses.FindAsync(record.CourseCode);
                    record.Student = await _context.Students.FindAsync(record.StudentId);
                }
                return View(academicRecords);
            }


            foreach (var record in academicRecords)
            {
                _context.Update(record); 
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}

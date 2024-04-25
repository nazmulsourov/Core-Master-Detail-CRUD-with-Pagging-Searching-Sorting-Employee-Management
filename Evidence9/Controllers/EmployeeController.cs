using Evidence9.Models;
using Evidence9.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Evidence9.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EvidenceCore9Context _db;
        private readonly IWebHostEnvironment _he;
        public EmployeeController(EvidenceCore9Context db, IWebHostEnvironment he)
        {
            _he = he;
            _db = db;
        }

        public async  Task<IActionResult> Index(string userText, string sortOrder, int page)
        {
            ViewBag.search = userText;
            ViewBag.sort = string.IsNullOrEmpty(sortOrder) ? "desc_name" : "";

            IQueryable<Employee> employees = _db.Employees.Include(x => x.EmployeeSkills).ThenInclude(y =>y.Skill);
            if (!string.IsNullOrEmpty(userText))
            {
                userText = userText.ToLower();
                employees = employees.Where(x=>x.EmployeeName.ToLower().Contains(userText));
            }
            switch (sortOrder)
            {
                case "desc_name":
                    employees = employees.OrderByDescending(x => x.EmployeeName);
                    break;
                default:
                    employees = employees.OrderBy(x => x.EmployeeName);
                    break;

            }
            if (page <= 0) page = 1 ;
            int pageSize = 2;
            ViewBag.pSize = pageSize;
            return View( await PaginatedList<Employee>.CreateAsync(employees,page, pageSize));
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult AddNewSkill(int id)
        {
            ViewBag.skills = new SelectList(_db.Skills, "SkillId", "SkillName", id.ToString() ?? "");
            return PartialView("_addSkill");
        }
        [HttpPost]
        public IActionResult Create(EmployeeVM employeeVM, int[] skillId)
        {
            if(ModelState.IsValid)
            {
                Employee employee = new Employee()
                {
                    EmployeeName = employeeVM.EmployeeName,
                    JoinDate = employeeVM.JoinDate,
                    Salary = employeeVM.Salary,
                    ActiverStatus = employeeVM.ActiverStatus,

                };
                var file = employeeVM.PictureFile;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFile = Path.GetFileName(employeeVM.PictureFile.FileName);
                string fileToSave = Path.Combine(webroot,folder,imgFile);
                if(file != null)
                {
                    using(var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        employeeVM.PictureFile.CopyTo(stream);
                        employee.Picture= "/" + folder + "/" + imgFile;
                    }
                }
                foreach (var item in skillId)
                {
                    EmployeeSkill employeeSkill = new EmployeeSkill()
                    {
                        Employee = employee,
                        EmployeeId = employee.EmployeeId,
                        SkillId = item
                    };
                    _db.EmployeeSkills.Add(employeeSkill);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete (int id)
        {
            var employee = _db.Employees.Find(id);
            if(id != null)
            {
                var delete = _db.EmployeeSkills.Where(e=>e.EmployeeId == id).ToList();
                _db.EmployeeSkills.RemoveRange(delete);
                _db.Employees.Remove(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(EmployeeVM employeeVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee()
                {
                    EmployeeId = employeeVM.EmployeeId,
                    EmployeeName = employeeVM.EmployeeName,
                    JoinDate = employeeVM.JoinDate,
                    Salary = employeeVM.Salary,
                    ActiverStatus=employeeVM.ActiverStatus
                };
                var file = employeeVM.PictureFile;

                if (file != null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFile = Path.GetFileName(employeeVM.PictureFile.FileName);
                    string filetoSave = Path.Combine(webroot, folder, imgFile);
                    using (var stream = new FileStream(filetoSave, FileMode.Create))
                    {
                        employeeVM.PictureFile.CopyTo(stream);
                        employee.Picture = "/" + folder + "/" + imgFile;
                    }
                }
                else
                {
                    employee.Picture = employeeVM.Picture;
                }
                var existSkill = _db.EmployeeSkills.Where(x => x.EmployeeId == employee.EmployeeId).ToList();
                foreach (var item in existSkill)
                {
                    _db.EmployeeSkills.RemoveRange(@item);
                }
                foreach (var item in skillId)
                {
                    EmployeeSkill employeeSkill = new EmployeeSkill()
                    {
                        EmployeeId = employee.EmployeeId,
                        SkillId = item
                    };
                    _db.EmployeeSkills.Add(employeeSkill);
                }
                _db.Update(employee);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

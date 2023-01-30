using CRUD_Oparation_PC.Data;
using CRUD_Oparation_PC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Oparation_PC.Controllers
{
    public class CRUDController : Controller
    {
        private readonly CRUDDbContext _context;
        private readonly IWebHostEnvironment _webhostenviroment;
        public CRUDController(CRUDDbContext context,IWebHostEnvironment webhostenviroment)
        {
            _context = context;
            _webhostenviroment = webhostenviroment;
        }

        //Code for cascadding dropdown
        //public JsonResult Country()
        //{
        //    List<Country> countryList = new List<Country>();
        //    countryList = _context.Countries.ToList();
        //    return Json(countryList);

        //}
        public JsonResult StateGetStatesByCountryId(int countryId)
        {
            List<State> statesList = new List<State>();
            statesList = _context.States.Where(dv => dv.Country.Id == countryId).ToList();
            return Json(statesList);

        }
        public JsonResult GetCitiesByStateId(int stateId)
        {
            List<City> citiesList = new List<City>();
            citiesList = _context.Cities.Where(dv => dv.State.Id == stateId).ToList();
            return Json(citiesList);
        }

        public async Task<IActionResult> Index()
        {
            var indexdata = _context.Employees.Include(c => c.City).Include(s => s.State).Include(c => c.Country);
            var viewdata = await indexdata.ToListAsync();
            return View(viewdata);
        }
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee, IFormFile pictureFile)
        {
            if (ModelState.IsValid)
            {
                if (pictureFile != null && pictureFile.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/images",
                    pictureFile.FileName); 
                using (var stream = new FileStream(path, FileMode.Create))
                    {
                        pictureFile.CopyTo(stream);
                    }
                    employee.Picture = $"{pictureFile.FileName}";
                }
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName",
            employee.CityId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id",
            "CountryName", employee.CountryId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName",
            employee.StateId);
            return View(employee);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.Include(C => C.Country).Include(C => C.City).Include(s => s.State).FirstOrDefaultAsync(m => m.Id == id);
            if(employee==null)
            {
                return NotFound();
            }
            return View(employee);

        }
        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id ==null || _context.Employees==null){
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(Id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "id", "CountryName", employee.CountryId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName", employee.StateId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", employee.CityId);
            return View(employee);
        }
        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(Employee employee,IFormFile pictureFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var emp = await _context.Employees.FindAsync(employee.Id);
                    if(pictureFile !=null && pictureFile.Length > 0){
                        var path = Path.Combine(Directory.GetCurrentDirectory(), pictureFile.FileName);
                        using (var stream=new FileStream(path, FileMode.Create))
                        {
                            pictureFile.CopyTo(stream);
                        }
                        employee.Picture = $"{pictureFile.FileName}";
                    }
                    else
                    {
                        employee.Picture = emp.Picture;
                    }
                    emp.Picture = employee.Picture;
                    emp.Name = employee.Name;
                    emp.Address = employee.Address;
                    emp.Gender = employee.Gender;
                    emp.Ssc = employee.Ssc;
                    emp.Hsc = employee.Hsc;
                    emp.Bsc = employee.Bsc;
                    emp.Msc = employee.Msc;
                    emp.CountryId = employee.CountryId;
                    emp.StateId = employee.StateId;
                    emp.CityId = employee.CityId;
                    _context.Update(emp);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!EmployeeExits(employee.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName",employee.CountryId);
            ViewData["StateId"] = new SelectList(_context.States, "Id", "StateName",employee.StateId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName",employee.CityId);
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int?id)
        {
            if(id==null || _context.Employees == null)
            {
                return NotFound();
            }
            var employee=await _context.Employees.Include(C => C.Country).Include(St => St.State).Include(ct => ct.City).FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set'CRUDDbContext.Employees'is null");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }







    }
}

using System.Linq.Expressions;
using System.Security.Policy;
using AutoMapper;
using IKEA.BLL.Models;
using IKEA.BLL.Models.Departments;
using IKEA.BLL.Services;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;

        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment environment,IMapper mapper)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _environment = environment;
            _mapper = mapper;
        }
        #endregion
        #region Index
        [HttpGet]
        //BaseUrl/Department/Index
        public async Task<IActionResult> Index()
        {
            ViewData["Message"] = "Hello ViewData";
            ViewBag.Message = "Hello ViewBag";
            var departments = await _departmentService.GetAllDepartmentsAsync();
            return View(departments);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
         public async Task<IActionResult> Create()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken] //علشان اللي لازم يعدل من الابلكيشن نفسه
        public async Task<IActionResult> Create(DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var createdDepartment = _mapper.Map<CreatedDepartmentDto>(departmentVM);
                var result = await _departmentService.CreateDerpartmenetAsync(createdDepartment);
                if (result > 0)
                {
                    TempData["Message"] = "Dempartment Is Created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Department Has Not Been Created";
                    message = "Sorry The Department Has Not Been Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentVM);
                }
                 
            }
            
            catch(Exception ex)
            {
                //1- Log Exception
                _logger.LogError(ex, ex.Message);
                //2- Set Frindly Message
                if(_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(departmentVM);
                }
                else
                {
                    message = "Sorry The Department Has Not Been Created";
                    return View("Error", message);
                }

            }
        }
        #endregion
        #endregion
        #region Details
        [HttpGet]
         public async Task<IActionResult> Details(int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(department);

        }
        #endregion
        #region  Edit
        #region Get
        [HttpGet]
         public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400

            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();//404
            }
            var departmentVM = _mapper.Map<DepartmentDetailsToReturnDto, DepartmentEditViewModel>(department);
            return View(departmentVM);

        }
        #endregion
        #region  Post
        [HttpPost]
        [ValidateAntiForgeryToken] //علشان اللي لازم يعدل من الابلكيشن نفسه
        public async Task<IActionResult> Edit (int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                //Manual Mapping
                //var updatedDepartment = new UpdateDepartmentDto()
                //{
                //    Id = id,
                //    Code = departmentVM.Code,
                //    Name = departmentVM.Name,
                //    Description = departmentVM.Description,
                //    CreationDate = departmentVM.CreationDate


                //};
                var updatedDepartment = _mapper.Map<DepartmentEditViewModel, UpdateDepartmentDto>(departmentVM);
                var result = await _departmentService.UpdateDepartmentAsync(updatedDepartment)>0;
                if(result)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry , An Error Occured While Updating The Department";

            }
             catch(Exception ex)
            {
                //1-
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry , An Error Occured While Updating The Department";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);


        }
        #endregion
        #endregion
        #region Delete
        [HttpGet]
         public async Task<IActionResult> Delete (int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(department);
        }
        //---------------------
        [HttpPost]
        [ValidateAntiForgeryToken] //علشان اللي لازم يعدل من الابلكيشن نفسه
        public async Task<IActionResult> Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = await _departmentService.DeletedDepartmentAsync(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));

                }
                message = "An Error Ocurred During Deleting The Department";
            }
             catch(Exception ex)
            {
                //1-
                _logger.LogError(ex, ex.Message);
                //2- 
                message = _environment.IsDevelopment() ? ex.Message : "An Error Ocurred During Deleting The Department";


            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


    } 
}

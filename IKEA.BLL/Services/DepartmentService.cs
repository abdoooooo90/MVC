using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace IKEA.BLL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
        public IEnumerable<DepartmentToReturnDto> GetAllDepartments()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAllAsQuerable().Select(department=> new DepartmentToReturnDto
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                
                CreationDate = department.CreationDate
            }).AsNoTracking().ToList();




            return departments;
        }

        public DepartmentDetailsToReturnDto? GetDepartmentById(int id)
        {
            var department = _unitOfWork.DepartmentRepository.GetById(id);
             if (department is { })
            {
                return new DepartmentDetailsToReturnDto()
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    Description = department.Description,

                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModificationBy = department.LastModificationBy,
                    LastModificationOn = department.LastModificationOn,

                };
            }
            return null;
            
        }
        public int CreateDerpartmenet(CreatedDepartmentDto departmentDto)
        {
            var Createddepartment = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                LastModificationBy = 1,
                LastModificationOn= DateTime.UtcNow,
            };
             _unitOfWork.DepartmentRepository.Add(Createddepartment);
            return _unitOfWork.Complete();
        }

        public int UpdateDepartment(UpdateDepartmentDto departmentDto)
        {
            var UpdatedDepartment = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,

                LastModificationBy = 1,
                LastModificationOn = DateTime.UtcNow,
            };
             _unitOfWork.DepartmentRepository.Update(UpdatedDepartment);
            return _unitOfWork.Complete();
        }
        public bool DeletedDepartment(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = departmentRepo.GetById(id);
             if (department is not null)
            {
                departmentRepo.Delete(department);
            }
            return _unitOfWork.Complete() > 0;
        }

        
    }
}

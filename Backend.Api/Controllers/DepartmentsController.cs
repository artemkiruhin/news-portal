using Backend.Core.Database.UnitOfWork;
using Backend.Core.Models.DTOs.Response;
using Backend.Core.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IUnitOfWork _database;
        private readonly CreateDepartmentUseCase _createDepartmentUseCase;
        private readonly UpdateDepartmentUseCase _updateDepartmentUseCase;

        public DepartmentsController(IUnitOfWork database, CreateDepartmentUseCase createDepartmentUseCase, UpdateDepartmentUseCase updateDepartmentUseCase)
        {
            _database = database;
            _createDepartmentUseCase = createDepartmentUseCase;
            _updateDepartmentUseCase = updateDepartmentUseCase;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll(string? name, CancellationToken ct)
        {
            try
            {

                var departments = !string.IsNullOrEmpty(name)
                    ? await _database.DepartmentRepository.GetByNameAsync(name, ct)
                    : await _database.DepartmentRepository.GetAllAsync(ct);
                
                var dtos = departments.Select(department => new DepartmentResponse(
                    department.Id,
                    department.Name,
                    department.Employees.Count,
                    department.Posts.Count,
                    department.CreatedAt
                ));
                
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("id/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            try
            {
                var department = await _database.DepartmentRepository.GetByIdAsync(id, ct);
                var dto = new DepartmentResponse(
                    department.Id,
                    department.Name,
                    department.Employees.Count,
                    department.Posts.Count,
                    department.CreatedAt
                );
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> Create(string name, CancellationToken ct)
        {
            try
            {
                var result = await _createDepartmentUseCase.ExecuteAsync(name, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPatch("edit")]
        public async Task<IActionResult> Edit(Guid id, string name, CancellationToken ct)
        {
            try
            {
                var result = await _updateDepartmentUseCase.ExecuteAsync(id, name, ct);
                return result.IsSuccess ? Ok(result.Value) : BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

using AutoMapper;
using Blogs.Core.Constants;
using Blogs.Core.Dto;
using Blogs.Core.Interfaces.Base;
using Blogs.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.Api.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion


        #region Constructor
        public CategoryController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion


        #region Actions

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                return NotFound($"There is no category with id = {id}");
            return Ok(category);
        }


        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();
            return Ok(categories);
        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto categoryDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapping 
            var category = _mapper.Map<Category>(categoryDto);

            await _unitOfWork.Categories.AddAsync(category);

            if (_unitOfWork.Complete() > 0)
                return Created("Created Successfully",category);

            return BadRequest("Error While Saving");
        }

        [Authorize(Roles =Roles.Admin)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryDb = await _unitOfWork.Categories.GetByIdAsync(category.CategoryId);

            if (categoryDb is null)
                return NotFound($"There is no category with id = {category.CategoryId}");

            // Update name
            categoryDb.Name = category.Name;
             _unitOfWork.Categories.Update(categoryDb);

            if (_unitOfWork.Complete() > 0)
                return Ok(category);

            return BadRequest("Error While Saving");
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category is null)
                return NotFound($"There is no category with id = {id}");

            _unitOfWork.Categories.Delete(category);

            if (_unitOfWork.Complete() > 0)
                return Ok("Deleted Successfully");

            return BadRequest("Error While Saving");

        }

        #endregion



    }
}

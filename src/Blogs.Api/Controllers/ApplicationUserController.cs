using AutoMapper;
using Blogs.Core.Dto;
using Blogs.Core.Interfaces.Base;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blogs.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAnyOrigin")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion


        #region Constructor
        public ApplicationUserController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region Actions
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {

            var user = await _unitOfWork.ApplicationUsers.FindAsync(u=>u.Id==id);

            if (user is null)
                return NotFound($"There is no user with id={id}");

            var result = _mapper.Map<GetApplicationUserDto>(user);

            return Ok(result);

        }
        #endregion
    }
}

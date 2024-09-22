using AutoMapper;
using Blogs.Core.Constants;
using Blogs.Core.Dto;
using Blogs.Core.Interfaces.Base;
using Blogs.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blogs.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {

        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion


        #region Constructor
        public CommentsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion


        #region Actions


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            var comment = await _unitOfWork.Comments.GetByIdAsync(id);

            if (comment is null)
                return NotFound($"There is no comment with id={id}");
            
            return Ok(comment);

        }


        [HttpGet("List/{postId}")]
        public async Task<IActionResult> GetAll(int postId)
        {
            var postDb = await _unitOfWork.Posts.GetByIdAsync(postId);

            if (postDb is null)
                return NotFound($"There is no post with id = {postId}");

            var comments = await _unitOfWork.Comments.FindAllAsync(c=>c.PostID==postId);
            return Ok(comments);

        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _unitOfWork.Comments.GetAllAsync();
            return Ok(comments);

        }

        [Authorize]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postDb = await _unitOfWork.Posts.GetByIdAsync(commentDto.PostID);

            if (postDb == null)
                return NotFound($"There is no post with id = {commentDto.PostID}");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.FindAsync(u => u.UserName == userName);

            var commnet = await _unitOfWork.Comments.CreateCommentAsync(commentDto, applicationUser.Id);

            if (!(_unitOfWork.Complete() > 0))
                return BadRequest("Error While saving the Comment");

            return Ok(commnet);
        }
        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentDb = await _unitOfWork.Comments.GetByIdAsync(commentDto.Id);

            if (commentDb is null)
                return NotFound($"There is no comment with id={commentDto.Id}");

            commentDb.Content = commentDto.Content;
            commentDb.CreatedAt = DateTime.Now;

            if (_unitOfWork.Complete() > 0)
                return Ok(commentDb);

            return BadRequest("Error While Saving");
        }

        [Authorize ]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);

            if (comment is null)
                return NotFound($"There is no comment with id = {id}");


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.FindAsync(u => u.UserName == userName);

            if(comment.UserId!=applicationUser.Id)
                return BadRequest("This is not your own comment to Update It");

            _unitOfWork.Comments.Delete(comment);

            if (_unitOfWork.Complete() > 0)
                return Ok("Deleted Successfully");

            return BadRequest("Error While Saving");

        }



        #endregion
    }
}

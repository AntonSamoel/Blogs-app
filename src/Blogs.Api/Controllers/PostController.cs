using AutoMapper;
using Blogs.Core.Dto;
using Blogs.Core.Interfaces.Base;
using Blogs.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Blogs.Api.Controllers
{
    [EnableCors("AllowAnyOrigin")]
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class PostController : ControllerBase
    {

        #region Fields
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion


        #region Constructor
        public PostController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            var post = await _unitOfWork.Posts.FindAsync(p=>p.PostId==id,new string[] { "Comments" });

            if(post  is null)
                return NotFound($"There is no post with id={id}");
            var result = _mapper.Map<GetPostDto>(post);
            return Ok(result);

        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetByUserId([FromRoute] string id)
        {
            var posts = await _unitOfWork.Posts.FindAllAsync(p=>p.UserId==id, new string[] { "Comments" });

            var result = _mapper.Map<List<GetPostDto>>(posts);

            return Ok(result);
        }

        [HttpGet("Filter/{categoryId}")]
        public async Task<IActionResult> FilterByCategory([FromRoute] int categoryId,int? pageNumber = null)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category is null)
                return NotFound($"There is no category with id={categoryId}");

            PaginationDTO<Post>? posts;

            posts = await _unitOfWork.Posts.GetPaginatedPostsAsync(p => p.CategoryID==categoryId, pageNumber, 10);
 
            var result = _mapper.Map<PaginationDTO<GetPostDto>>(posts);

            return Ok(result);
        }

        [HttpGet("Serach")]
        public async Task<IActionResult> SearchByDescription(int? pageNumber = null, string ? words = null)
        {

            PaginationDTO<Post>? posts;

            if(words is null)
            {
                posts = await _unitOfWork.Posts.GetPaginatedPostsAsync(pageNumber, 10);
            }
            else
            {
                posts = await _unitOfWork.Posts.GetPaginatedPostsAsync(p => p.Description.ToLower().Contains(words.ToLower()),pageNumber, 10);
            }

            var result = _mapper.Map<PaginationDTO<GetPostDto>>(posts);

            return Ok(result);
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _unitOfWork.Posts.FindAllAsync(p=>p.PostId!=null,new string[] { "Comments" });
            var result = _mapper.Map<List<GetPostDto>>(posts);
            return Ok(result);

        }

        [HttpGet("PaginatedList")]
        public async Task<IActionResult> GetPaginatedList(int? pageNumber = null)
        {
            var posts = await _unitOfWork.Posts.GetPaginatedPostsAsync(pageNumber,10);
            var result = _mapper.Map<PaginationDTO<GetPostDto>>(posts);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreatePostDto createPostDto)
        {
            
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.FindAsync(u=>u.UserName==userName);

           var post = await _unitOfWork.Posts.CreatePost(createPostDto, applicationUser.Id);

            if (!(_unitOfWork.Complete() > 0))
                return BadRequest("Error While saving the post");

            var result = _mapper.Map<GetPostDto>(post); 
            return Ok(result);

        }

        [Authorize]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromForm] UpdatePostDto postDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postDb = await _unitOfWork.Posts.GetByIdAsync(postDto.PostId);

            if (postDb is null)
                return NotFound($"There is no post with id = {postDto.PostId}");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.FindAsync(u => u.UserName == userName);

            if (postDb.UserId != applicationUser.Id)
                return BadRequest("This is not your own post to Update It");

            var result = _unitOfWork.Posts.UpdatePost(postDb, postDto);

            if (_unitOfWork.Complete() > 0)
                return Ok(_mapper.Map<GetPostDto>(result));

            return BadRequest("Error While Saving");
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);

            if (post is null)
                return NotFound($"There is no post with id = {id}");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.FindAsync(u => u.UserName == userName);

            if (post.UserId != applicationUser.Id)
                return BadRequest("This is not your own post to Delete It");

            // TODO: Delete All comments First
            var comments =await _unitOfWork.Comments.FindAllAsync(c=>c.PostID== id);
            _unitOfWork.Comments.DeleteRange(comments);

            _unitOfWork.Posts.DeletePost(post);

            if (_unitOfWork.Complete() > 0)
                return Ok("Deleted Successfully");

            return BadRequest("Error While Saving");

        }

        [Authorize]
        [HttpDelete("Image/{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);

            if (post is null)
                return NotFound($"There is no post with id = {id}");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userName = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var applicationUser = await _unitOfWork.ApplicationUsers.FindAsync(u => u.UserName == userName);

            if (post.UserId != applicationUser.Id)
                return BadRequest("This is not your own post to Delete its image");

            if(post.Image is null)
                return BadRequest("There is no image to be deleted");

            bool result = _unitOfWork.Posts.DeleteImage(post);
            if (result)
            {
                if (_unitOfWork.Complete() > 0)
                    return Ok("Image Deleted Successfully");
            }

            return BadRequest("Error While Deleting");

        }
    }
}

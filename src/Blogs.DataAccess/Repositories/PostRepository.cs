using Blogs.Core.Dto;
using Blogs.Core.Interfaces;
using Blogs.Core.Models;
using Blogs.DataAccess.Data;
using Blogs.DataAccess.Repositories.Base;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Linq.Expressions;

namespace Blogs.DataAccess.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext context;

        private readonly IImageService _imageService;
        private readonly static string _imagesPath ="assets/images/posts";
        public PostRepository(ApplicationDbContext context,IImageService imageService) : base(context)
        {
            this.context = context;
            _imageService = imageService;

        }

        public async Task<PaginationDTO<Post>> GetPaginatedPostsAsync(int? pageNumber, int pageSize)
        {
            var totalCount = await context.Post.CountAsync();
            int pageNum = pageNumber ?? 0;
            List<Post> items;

            if(pageNumber is null)
            {
                items = context.Post.ToList();
            }
            else
            {
                items = await _context.Post
                .Include(p => p.Comments)
                .OrderBy(p => p.PostId)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToListAsync();
            }
            return new PaginationDTO<Post>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNum,
                PageSize = pageSize
            };
        }

        public async Task<PaginationDTO<Post>> GetPaginatedPostsAsync(Expression<Func<Post, bool>> criteria,int? pageNumber, int pageSize)
        {
            var result = context.Post.Where(criteria);

            var totalCount = await result.CountAsync();
            int pageNum = pageNumber ?? 0;
            List<Post> items;

            if (pageNumber is null)
            {
                items = result.ToList();
            }
            else
            {
                items = await result
                .Include(p => p.Comments)
                .OrderBy(p => p.PostId)
                .Skip(pageSize * (pageNum - 1))
                .Take(pageSize)
                .ToListAsync();
            }
            return new PaginationDTO<Post>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNum,
                PageSize = pageSize
            };
        }

        public async Task<Post> CreatePost(CreatePostDto createPostDto, string userId)
        {

            var fileResult = _imageService.SaveImage(createPostDto.Image!, _imagesPath);
            Post post = new()
            {
                UserId  = userId,
                CreatedAt = DateTime.UtcNow,
                Description = createPostDto.Description,
                CategoryID = createPostDto.CategoryId
            };

            if (fileResult.Item1 == 1)
            {
                post.Image = fileResult.Item2;
            }

            return await AddAsync(post);
        }
        public void DeletePost(Post post)
        {
            if(post.Image is not null)
                _imageService.DeleteImage(post.Image, _imagesPath);

            Delete(post);
        }

        public Post UpdatePost(Post postDb, UpdatePostDto updatePostDto)
        {

            if (updatePostDto.Image is not null)
            {
                if (postDb.Image is not null)
                    _imageService.DeleteImage(postDb.Image, _imagesPath);

            var fileResult = _imageService.SaveImage(updatePostDto.Image!, _imagesPath);

             if (fileResult.Item1 == 1)
            {
                    postDb.Image = fileResult.Item2;
            }

            }

            if(updatePostDto.CategoryId is not null)
                postDb.CategoryID = updatePostDto.CategoryId;

           postDb.Description = updatePostDto.Description;

            return Update(postDb);
        }

        public bool DeleteImage(Post postDb)
        {

            if (postDb.Image is not null)
            {
                _imageService.DeleteImage(postDb.Image, _imagesPath);
                postDb.Image = null;
                return true;
            }
            return false;
        }
    }
}

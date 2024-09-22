using Blogs.Core.Dto;
using Blogs.Core.Interfaces.Base;
using Blogs.Core.Models;
using System.Linq.Expressions;

namespace Blogs.Core.Interfaces
{
    public interface IPostRepository : IBaseRepository<Post>
    {
        public Task<Post> CreatePost(CreatePostDto createPostDto, string userId);
        public void DeletePost(Post post);
        public Post UpdatePost(Post postDb, UpdatePostDto updatePostDto);
        public bool DeleteImage(Post postDb);
        public Task<PaginationDTO<Post>> GetPaginatedPostsAsync(int? pageNumber, int pageSize);
        public Task<PaginationDTO<Post>> GetPaginatedPostsAsync(Expression<Func<Post, bool>> criteria, int? pageNumber, int pageSize);
    }
}

using Blogs.Core.Dto;
using Blogs.Core.Interfaces;
using Blogs.Core.Interfaces.Base;
using Blogs.Core.Models;
using Blogs.DataAccess.Data;
using Blogs.DataAccess.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.DataAccess.Repositories
{
    public class CommnetRepository : BaseRepository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommnetRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Comment> CreateCommentAsync(CreateCommentDto commentDto, string userId)
        {
            Comment comment = new()
            {
                UserId = userId,
                Content = commentDto.Content,
                CreatedAt = DateTime.Now,
                PostID = commentDto.PostID
            };

            return await AddAsync(comment);
        }
    }
}

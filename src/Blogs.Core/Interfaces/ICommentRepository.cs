using Blogs.Core.Dto;
using Blogs.Core.Interfaces.Base;
using Blogs.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Core.Interfaces
{
    public interface ICommentRepository:IBaseRepository<Comment>
    {
        public Task<Comment> CreateCommentAsync(CreateCommentDto commentDto, string userId);
    }
}

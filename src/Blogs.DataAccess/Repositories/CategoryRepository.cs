using Blogs.Core.Interfaces;
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
    public class CategoryRepository : BaseRepository<Category> , ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

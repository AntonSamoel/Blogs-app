using Blogs.Core.Interfaces;
using Blogs.Core.Models.AuthModels;
using Blogs.DataAccess.Data;
using Blogs.DataAccess.Repositories.Base;

namespace Blogs.DataAccess.Repositories
{
    public class AplicationUsersRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public AplicationUsersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

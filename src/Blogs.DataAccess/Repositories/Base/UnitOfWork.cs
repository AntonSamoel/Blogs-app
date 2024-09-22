using Blogs.Core.Interfaces;
using Blogs.Core.Interfaces.Base;
using Blogs.DataAccess.Data;


namespace Blogs.DataAccess.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICategoryRepository Categories { get; private set; }
        public IPostRepository Posts { get; private set; }
        public ICommentRepository Comments { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }


        public UnitOfWork(ApplicationDbContext context,IImageService imageService)
        {
            _context = context;

            Categories = new CategoryRepository(context);
            Posts = new PostRepository(context,imageService);
            Comments = new CommnetRepository(context);
            ApplicationUsers = new AplicationUsersRepository(context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

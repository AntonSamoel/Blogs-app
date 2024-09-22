namespace Blogs.Core.Interfaces.Base
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }
        IPostRepository Posts { get; }
        IApplicationUserRepository ApplicationUsers { get; }
        int Complete();
    }
}

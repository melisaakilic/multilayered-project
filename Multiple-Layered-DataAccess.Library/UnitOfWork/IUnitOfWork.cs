namespace Multiple_Layered_DataAccess.Library.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Transaction operations
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();

        // Repositories
        IRepository<User> Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderProduct> OrderProducts { get; }

        // Main operations
        void SaveChanges();
        Task SaveChangesAsync();

        
    }
}

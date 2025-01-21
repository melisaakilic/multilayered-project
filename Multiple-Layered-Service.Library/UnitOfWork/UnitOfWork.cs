namespace Multiple_Layered_DataAccess.Library.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly AppDbContext _context;

        private IDbContextTransaction _transaction;
        private IRepository<User> _users;
        private IRepository<Product> _products;
        private IRepository<Order> _orders;
        private IRepository<OrderProduct> _orderProducts;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
           
        }

        public IRepository<User> Users => _users ??= new Repository<User>(_context);

        public IRepository<Product> Products => _products ??= new Repository<Product>(_context);

        public IRepository<Order> Orders => _orders ??= new Repository<Order>(_context);

        public IRepository<OrderProduct> OrderProducts => _orderProducts ??= new Repository<OrderProduct>(_context);

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await _transaction.RollbackAsync();
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            _transaction?.Dispose();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}

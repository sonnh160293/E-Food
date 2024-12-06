using FoodOnline.DataAccess.DataAccess;
using FoodOnline.Domain.Abstract;

namespace FoodOnline.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FoodDbContext _context;

        private IBranchRepository? _branchRepository;
        private ICategoryRepository? _categoryRepository;
        private IProductRepository? _productRepository;
        private IProductDetailRepository? _productDetailRepository;
        private IProductRelatedRepository? _productRelatedRepository;
        private IUserAddressRepository? _userAddressRepository;
        private ICartRepository? _cartRepository;
        private IOrderDetailRepository? _orderDetailRepository;
        private IOrderRepository? _orderRepository;
        private IOrderStatusRepository? _orderStatusRepository;
        public UnitOfWork(FoodDbContext context)
        {
            _context = context;
        }

        public IBranchRepository BranchRepository => _branchRepository ?? new BranchRepository(_context);
        public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(_context);
        public IProductRepository ProductRepository => _productRepository ?? new ProductRepository(_context);
        public IProductDetailRepository ProductDetailRepository => _productDetailRepository ?? new ProductDetailRepository(_context);
        public IProductRelatedRepository ProductRelatedRepository => _productRelatedRepository ?? new ProductRelatedRepository(_context);
        public IUserAddressRepository UserAddressRepository => _userAddressRepository ?? new UserAddressRepository(_context);
        public ICartRepository CartRepository => _cartRepository ?? new CartRepository(_context);
        public IOrderStatusRepository OrderStatusRepository => _orderStatusRepository ?? new OrderStatusRepository(_context);
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository ?? new OrderDetailRepository(_context);
        public IOrderRepository OrderRepository => _orderRepository ?? new OrderRepository(_context);


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}

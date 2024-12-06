namespace FoodOnline.Domain.Abstract
{
    public interface IUnitOfWork
    {
        IBranchRepository BranchRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductDetailRepository ProductDetailRepository { get; }

        IProductRelatedRepository ProductRelatedRepository { get; }
        IUserAddressRepository UserAddressRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderStatusRepository OrderStatusRepository { get; }
        IOrderDetailRepository OrderDetailRepository { get; }
        void Dispose();
        Task SaveChangesAsync();
    }
}

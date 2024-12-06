namespace FoodOnline.Application.IService
{
    public interface ICommonService
    {
        Task<string> GetCurrentUser();
        Task<string?> GetCurrentUserId();
        Task<string?> GetCurrentUserRole();
        Task<int?> GetCurrentUserBranch();
        void Set<T>(string key, T value);
        T Get<T>(string key);
    }
}

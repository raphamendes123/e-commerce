namespace Core.Domain.Repository.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}

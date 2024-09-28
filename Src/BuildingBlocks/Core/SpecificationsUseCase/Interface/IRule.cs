namespace Core.SpecificationsUseCase.Interface
{
    public interface IRule<in TEntity>
    {
        string ErrorMessage { get; }

        Task<bool> ValidateAsync(TEntity entity);
    }
}

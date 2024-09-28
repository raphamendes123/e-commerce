namespace Core.SpecificationsUseCase.Interface
{
    public interface IValidator<in TEntity>
    {
        Task<ValidationUseCase> ValidateAsync(TEntity entity);
    }
}

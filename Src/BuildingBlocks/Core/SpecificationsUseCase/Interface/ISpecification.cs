namespace Core.SpecificationsUseCase.Interface
{
    public interface ISpecification<in T>
    {
        Task<bool> IsSatisfiedBy(T entity);
    }
}

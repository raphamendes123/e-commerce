using Core.SpecificationsFunc;
using Core.SpecificationsUseCase.Interface;

namespace Core.SpecificationsUseCase
{
    public class Rule<TEntity> : IRule<TEntity>
    {
        private readonly ISpecification<TEntity> _specification;

        public string ErrorMessage { get; }

        public Rule(ISpecification<TEntity> spec, string errorMessage)
        {
            _specification = spec;
            ErrorMessage = errorMessage;
        }

        public Rule(Specification<TEntity> spec, string errorMessage)
        {
            _specification = (ISpecification<TEntity>?)spec;
            ErrorMessage = errorMessage;
        }
        

        public async Task<bool> ValidateAsync(TEntity entity)
        {
            return await _specification.IsSatisfiedBy(entity);
        }
    }
}

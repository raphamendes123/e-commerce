using Core.SpecificationsUseCase.Interface;

namespace Core.SpecificationsUseCase
{
    public class Validator<TEntity> : IValidator<TEntity> where TEntity : class
    {
        private readonly Dictionary<string, IRule<TEntity>> _rules;

        public Validator()
        {
            _rules = new Dictionary<string, IRule<TEntity>>();
        }

        public async Task<ValidationUseCase> ValidateAsync(TEntity entity)
        {
            ValidationUseCase validationResult = new ValidationUseCase();
            foreach (KeyValuePair<string, IRule<TEntity>> rule in _rules)
            {
                if (await rule.Value.ValidateAsync(entity))
                {
                    validationResult.Add(new ValidationError(rule.Key, rule.Value.ErrorMessage));
                }
            }

            return validationResult;
        }

        protected virtual void Add(string name, IRule<TEntity> rule)
        {
            _rules.Add(name, rule);
        }
         

        protected IRule<TEntity> GetRule(string name)
        {
            return _rules[name];
        }

        protected virtual void Remove(string name)
        {
            _rules.Remove(name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Samhammer.Validation
{
    public class ValidationContext<TModel, TResult> : IValidationContext<TResult> where TResult : IValidationResult, new()
    {
        private readonly List<Func<TModel, Task<TResult>>> validations = new List<Func<TModel, Task<TResult>>>();

        private readonly Func<Task<TModel>> loadFunc;

        private TModel model;

        public ValidationContext(TModel model)
        {
            this.model = model;
        }

        public ValidationContext(Func<Task<TModel>> loadFunc)
        {
            this.loadFunc = loadFunc;
        }

        public ValidationContext<TModel, TResult> Add(Func<TModel, TResult> validateFunc)
        {
            return AddAsync(m => Task.FromResult(validateFunc(m)));
        }

        public ValidationContext<TModel, TResult> AddAsync(Func<TModel, Task<TResult>> validateFunc)
        {
            validations.Add(validateFunc);
            return this;
        }

        public async Task<TResult> ValidateAsync()
        {
            if (loadFunc != null)
            {
                model = await loadFunc();
            }

            foreach (var validation in validations)
            {
                var result = await validation(model);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return new TResult { Succeeded = true };
        }
    }

    public interface IValidationContext<TResult> where TResult : IValidationResult
    {
        Task<TResult> ValidateAsync();
    }
}

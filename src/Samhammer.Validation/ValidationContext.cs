using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Samhammer.Validation
{
    public class ValidationContext<TModel, TResult> : IValidationContext<TResult> where TResult : IValidationResult, new()
    {
        private readonly List<Func<TModel, Task<TResult>>> validations = new List<Func<TModel, Task<TResult>>>();

        public Func<Task<TModel>> LoadFunc { get; set; }

        public TModel Model { get; set; }

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
            if (LoadFunc != null)
            {
                Model = await LoadFunc();
            }

            foreach (var validation in validations)
            {
                var result = await validation(Model);
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

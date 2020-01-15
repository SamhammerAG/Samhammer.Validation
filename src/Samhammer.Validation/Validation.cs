using System;
using System.Threading.Tasks;

namespace Samhammer.Validation
{
    public class Validation<TResult> where TResult : IValidationResult, new()
    {
        public ValidationContext<TModel, TResult> Load<TModel>(TModel model)
        {
            return new ValidationContext<TModel, TResult>(model);
        }

        public ValidationContext<TModel, TResult> Load<TModel>(Func<Task<TModel>> loadFunc)
        {
            return new ValidationContext<TModel, TResult>(loadFunc);
        }

        public static async Task<TResult> ValidateAllAsync(params IValidationContext<TResult>[] contexts)
        {
            foreach (var context in contexts)
            {
                var result = await context.ValidateAsync();
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return new TResult { Succeeded = true };
        }
    }
}

namespace Samhammer.Validation
{
    public class ValidationResult : IValidationResult
    {
        public bool Succeeded { get; set; }
    }

    public interface IValidationResult
    {
        bool Succeeded { get; set; }
    }
}

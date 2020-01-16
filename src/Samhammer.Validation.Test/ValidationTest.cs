using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Samhammer.Validation.Test
{
    public class ValidationTest
    {
        [Theory]
        [InlineData("test", true)]
        [InlineData("", false)]
        public async Task Validate(string input, bool expected)
        {
            var validation = new Validation<ValidationResult>()
                .Load(input)
                .Add(SampleRule);

            var result = await validation.ValidateAsync();

            result.Succeeded.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", 1, true)]
        [InlineData("test", 0, false)]
        [InlineData("", 1, false)]
        [InlineData("", 0, false)]
        public async Task ValidateAll(string input1, int input2, bool expected)
        {
            var validation1 = new Validation<ValidationResult>()
                .Load(input1)
                .Add(SampleRule);

            var validation2 = new Validation<ValidationResult>()
                .Load(input2)
                .Add(SampleRule);

            var result = await Validation<ValidationResult>.ValidateAllAsync(validation1, validation2);

            result.Succeeded.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", true)]
        [InlineData("", false)]
        public async Task ValidateWithLoadFunc(string input, bool expected)
        {
            string LoadFunc() => input;

            var validation = new Validation<ValidationResult>()
                .Load(LoadFunc)
                .Add(SampleRule);

            var result = await validation.ValidateAsync();

            result.Succeeded.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", true, ErrorCode.Ok)]
        [InlineData("", false, ErrorCode.Error)]
        public async Task ValidateWithErrorCode(string input, bool expected, ErrorCode expectedCode)
        {
            var validation = new Validation<CustomValidationResult>()
                .Load(input)
                .Add(SampleRuleWithErrorCode);

            var result = await validation.ValidateAsync();

            result.Succeeded.Should().Be(expected);
            result.ErrorCode.Should().Be(expectedCode);
        }

        public static ValidationResult SampleRule(string input)
        {
            return string.IsNullOrEmpty(input)
                ? new ValidationResult { Succeeded = false }
                : new ValidationResult { Succeeded = true };
        }

        public static ValidationResult SampleRule(int input)
        {
            return input == 0
                ? new ValidationResult { Succeeded = false }
                : new ValidationResult { Succeeded = true };
        }

        public static CustomValidationResult SampleRuleWithErrorCode(string input)
        {
            return string.IsNullOrEmpty(input)
                ? new CustomValidationResult(ErrorCode.Error)
                : new CustomValidationResult();
        }

        // custom results can be defined, e.g. for returning errorCode by rules
        public class CustomValidationResult : ValidationResult
        {
            public ErrorCode ErrorCode { get; set; }

            public CustomValidationResult()
            {
                Succeeded = true;
                ErrorCode = ErrorCode.Ok;
            }

            public CustomValidationResult(ErrorCode errorCode)
            {
                Succeeded = false;
                ErrorCode = errorCode;
            }
        }

        public enum ErrorCode
        {
            Ok,
            Error,
        }
    }
}

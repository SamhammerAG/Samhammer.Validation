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
            var context = new Validation<ValidationResult>()
                .Load(input)
                .Add(SampleRule);

            var result = await context.ValidateAsync();

            result.Succeeded.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", 1, true)]
        [InlineData("test", 0, false)]
        [InlineData("", 1, false)]
        [InlineData("", 0, false)]
        public async Task ValidateAll(string input1, int input2, bool expected)
        {
            var context1 = new Validation<ValidationResult>()
                .Load(input1)
                .Add(SampleRule);

            var context2 = new Validation<ValidationResult>()
                .Load(input2)
                .Add(SampleRule2);

            var result = await Validation<ValidationResult>.ValidateAllAsync(context1, context2);

            result.Succeeded.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", true)]
        [InlineData("", false)]
        public async Task ValidateWithLoadFunc(string input, bool expected)
        {
            string LoadFunc() => input;

            var context = new Validation<ValidationResult>()
                .Load(LoadFunc)
                .Add(SampleRule);

            var result = await context.ValidateAsync();

            result.Succeeded.Should().Be(expected);
        }

        [Theory]
        [InlineData("test", true, ErrorCode.Ok)]
        [InlineData("", false, ErrorCode.Error)]
        public async Task ValidateWithErrorCode(string input, bool expected, ErrorCode expectedCode)
        {
            var context = new Validation<CustomValidationResult>()
                .Load(input)
                .Add(SampleRuleWithErrorCode);

            var result = await context.ValidateAsync();

            result.Succeeded.Should().Be(expected);
            result.ErrorCode.Should().Be(expectedCode);
        }

        public static ValidationResult SampleRule(string input)
        {
            return new ValidationResult { Succeeded = !string.IsNullOrEmpty(input) };
        }

        public static ValidationResult SampleRule2(int input)
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

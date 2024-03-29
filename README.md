## Usage

#### How to add this to your project:
- reference this package to your project: https://www.nuget.org/packages/Samhammer.Validation/

#### Validate a model ####

```csharp
var context = new Validation<ValidationResult>()
    .Load("test")
    .Add(SampleRule);

var result = await context.ValidateAsync();

if (!result.Succeeded)
{
    // TODO handle validation error
}

public ValidationResult SampleRule(string input)
{
    return new ValidationResult { Succeeded = input != null };
}
```

#### Validate a model, loaded by func ####

```csharp
async Task<Model> LoadModel() => await Repository.GetById(id);

var context = new Validation<ValidationContract>()
    .Load(LoadModel)
    .Add(SampleRule);
```

#### Validate multiple models ####

```csharp
var context1 = new Validation<ValidationResult>()
    .Load(input1)
    .Add(SampleRule);

var context2 = new Validation<ValidationResult>()
    .Load(input2)
    .Add(SampleRule2);

var result = await Validation<ValidationResult>.ValidateAllAsync(context1, context2);
```

#### Validate with custom result class ####

You can define your own result class with additional fields.  
This can be used to add something like an errorCode or an errorMessage by your rules.

```csharp
var context = new Validation<CustomValidationResult>()
    .Load(input)
    .Add(SampleRuleWithErrorCode);

var result = await context.ValidateAsync();

public static CustomValidationResult SampleRuleWithErrorCode(string input)
{
    return string.IsNullOrEmpty(input)
        ? new CustomValidationResult(ErrorCode.Error)
        : new CustomValidationResult();
}
```

```csharp
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
```

## Contribute

#### How to publish package
- Create a tag and let the github action do the publishing for you

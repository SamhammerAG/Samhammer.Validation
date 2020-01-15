# Description
- define model to validate
- add multiple rules
- execute validation
- use the validation result

## Usage

#### How to add this to your project:
- reference this package to your project: https://www.nuget.org/packages/Samhammer.DependencyInjection/

#### Validate a model ####

```csharp
var validation = new Validation<ValidationResult>()
    .Load(input)
    .Add(SampleRule);

var result = await validation.ValidateAsync();

if (!result.Succeeded)
{
    // TODO handle validation error
}

public ValidationResult SampleRule(string input)
{
    return string.IsNullOrEmpty(input)
        ? new ValidationResult { Succeeded = false }
        : new ValidationResult { Succeeded = true };
}

#### Validate a model, loaded by func ####
TODO

#### Validate multiple models ####
TODO

#### Validate with custom result class ####
TODO
```

## Contribute

#### How to publish package
- set package version in Samhammer.Validation.csproj
- create git tag
- dotnet pack -c Release
- nuget push .\bin\Release\Samhammer.Validation.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json

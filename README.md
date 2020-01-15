# Description

## Usage

## Contribute

#### How to publish package
- set package version in Samhammer.Validation.csproj
- create git tag
- dotnet pack -c Release
- nuget push .\bin\Release\Samhammer.Validation.*.nupkg NUGET_API_KEY -src https://api.nuget.org/v3/index.json
- (optional) nuget setapikey NUGET_API_KEY -source https://api.nuget.org/v3/index.json

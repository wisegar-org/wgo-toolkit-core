dotnet build -c Release
dotnet pack -c Release -o ./nupkg
dotnet nuget push ./nupkg/Wisegar.Toolkit.Services.1.0.1.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
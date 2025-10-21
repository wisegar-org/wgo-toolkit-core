dotnet build -c Release
dotnet pack -c Release -o ./nupkg
dotnet nuget push ./nupkg/Wisegar.Toolkit.Models.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
SET ASPNETCORE_ENVIRONMENT=development
dotnet build ./../Owlvey.Falcon.Authority.sln -v q
dotnet run --project ./../src/Owlvey.Falcon.Authority.Presentation/Owlvey.Falcon.Authority.Presentation.csproj --URLS=http://localhost:5000/ --configuration Release

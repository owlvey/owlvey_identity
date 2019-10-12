rm -rf ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer/Migrations

rm ./../src/Owlvey.Falcon.Authority.Presentation/FalconAuthDb.db

pushd ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer

dotnet ef migrations add ConfigurationDb -c PersistedGrantDbContext --project ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer/Owlvey.Falcon.Authority.Infra.Data.SqlServer.csproj -o Migrations/ConfigurationDb
dotnet ef migrations add PersistedGrantDb -c ConfigurationDbContext --project ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer/Owlvey.Falcon.Authority.Infra.Data.SqlServer.csproj -o Migrations/PersistedGrantDb
dotnet ef migrations add Initial -c FalconAuthDbContext --project ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer/Owlvey.Falcon.Authority.Infra.Data.SqlServer.csproj

popd
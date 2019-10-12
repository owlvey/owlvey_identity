pushd "./../src/Owlvey.Falcon.Authority.Presentation"
del /f FalconAuthDb.db
popd
pushd "./../src/Owlvey.Falcon.Authority.Infra.Data.Sqlite"
rmdir Migrations /s /q

dotnet ef migrations add ConfigurationDb -c PersistedGrantDbContext --project ../../src/Owlvey.Falcon.Authority.Infra.Data.Sqlite/Owlvey.Falcon.Authority.Infra.Data.Sqlite.csproj -o Migrations/ConfigurationDb
dotnet ef migrations add PersistedGrantDb -c ConfigurationDbContext --project ../../src/Owlvey.Falcon.Authority.Infra.Data.Sqlite/Owlvey.Falcon.Authority.Infra.Data.Sqlite.csproj -o Migrations/PersistedGrantDb
dotnet ef migrations add Initial -c FalconAuthDbContext --project ../../src/Owlvey.Falcon.Authority.Infra.Data.Sqlite/Owlvey.Falcon.Authority.Infra.Data.Sqlite.csproj

popd
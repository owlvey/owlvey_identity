rm -rf ./../src/Owlvey.Falcon.Authority.Infra.Data.Sqlite/Migrations

rm ./../src/Owlvey.Falcon.Authority.Presentation/FalconAuthDb.db

pushd ./../src/Owlvey.Falcon.Authority.Infra.Data.Sqlite

dotnet ef migrations add ConfigurationDb -c PersistedGrantDbContext  -o Migrations/ConfigurationDb
dotnet ef migrations add PersistedGrantDb -c ConfigurationDbContext -o Migrations/PersistedGrantDb
dotnet ef migrations add Initial -c FalconAuthDbContext

popd



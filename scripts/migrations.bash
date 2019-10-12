rm -rf ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer/Migrations

rm ./../src/Owlvey.Falcon.Authority.Presentation/AuhtorityFalconDb.db

pushd ./../src/Owlvey.Falcon.Authority.Infra.Data.SqlServer

dotnet ef migrations add ConfigurationDb -c PersistedGrantDbContext
dotnet ef migrations add PersistedGrantDb -c ConfigurationDbContext
dotnet ef migrations add Initial -c FalconAuthDbContext

popd
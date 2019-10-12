pushd "./../src/Owlvey.Falcon.Authority.Presentation"
del /f FalconDb.db
popd
pushd "./../Owlvey.Falcon.Authority.Infra.Data.SqlServer"
rmdir Migrations /s /q

dotnet ef migrations add ConfigurationDb -c PersistedGrantDbContext
dotnet ef migrations add PersistedGrantDb -c ConfigurationDbContext
dotnet ef migrations add Initial -c FalconAuthDbContext

popd
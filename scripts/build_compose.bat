set script_dir=%CD%
set OWLVEY_HOST=192.168.0.13
pushd %~dp0

cd ./../src/Owlvey.Falcon.Authority.Presentation

dotnet restore 

dotnet clean

dotnet publish --no-restore -o bin/Publish

docker build -t owlveylocal/authority .

docker tag owlveylocal/authority owlveylocal/authority:dev

popd

docker-compose down
docker-compose up 

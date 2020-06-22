dotnet clean ./Owlvey.Falcon.Authority.sln -v:q

pushd "./container"

docker-compose build

popd

docker tag owlvey/identity localhost:48700/owlvey/identity
docker push localhost:48700/owlvey/identity


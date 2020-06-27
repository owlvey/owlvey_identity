dotnet clean ./Owlvey.Falcon.Authority.sln -v:q

pushd "./containers"

docker-compose build

popd

docker tag owlvey/authority localhost:48700/owlvey/authority
docker push localhost:48700/owlvey/authority


FROM microsoft/dotnet:2.2.0-aspnetcore-runtime
COPY /bin/Publish /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.Authority.Presentation.dll"]

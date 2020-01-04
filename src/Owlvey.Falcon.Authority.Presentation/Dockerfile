FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY /bin/Publish /app
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.Authority.Presentation.dll"]

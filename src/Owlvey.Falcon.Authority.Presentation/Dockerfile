FROM microsoft/dotnet:2.2.0-aspnetcore-runtime
COPY . /app
RUN mkdir -p /app/data
RUN chmod o+x /app/data
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.Authority.Presentation.dll"]

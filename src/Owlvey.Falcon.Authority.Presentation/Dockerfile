FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
COPY . /app
RUN mkdir -p /app/data
RUN chmod o+x /app/data
COPY configuration.sql /app/data
COPY persistedgrant.sql /app/data
COPY authority.sql /app/data
COPY authority.sh /app/data
RUN chmod +x /app/data/configuration.sql
RUN chmod +x /app/data/persistedgrant.sql
RUN chmod +x /app/data/authority.sql
RUN chmod +x /app/data/authority.sh
WORKDIR /app
EXPOSE 80/tcp
ENTRYPOINT ["dotnet", "Owlvey.Falcon.Authority.Presentation.dll"]

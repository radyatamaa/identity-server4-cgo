FROM microsoft/aspnetcore:2.0.5
ARG source
WORKDIR /app
EXPOSE 80
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "host.dll"]

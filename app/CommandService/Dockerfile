FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY CommandService/ ./CommandService/
COPY Common/ ./Common/
COPY OperationContracts/ ./OperationContracts/

RUN dotnet restore CommandService/CommandService.csproj &&\
    dotnet build CommandService/CommandService.csproj -c Release -o /app/build &&\
    dotnet publish CommandService/CommandService.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim
WORKDIR /app

COPY --from=build /app/publish .

HEALTHCHECK --interval=10s --timeout=10s --retries=3 CMD curl --fail http://localhost:80/healthcheck || exit 1
ENTRYPOINT ["dotnet", "CommandService.dll"]
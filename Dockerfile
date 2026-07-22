FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY backend/src/ArchPilot.Domain/ArchPilot.Domain.csproj backend/src/ArchPilot.Domain/
COPY backend/src/ArchPilot.Application/ArchPilot.Application.csproj backend/src/ArchPilot.Application/
COPY backend/src/ArchPilot.Infrastructure/ArchPilot.Infrastructure.csproj backend/src/ArchPilot.Infrastructure/
COPY backend/src/ArchPilot.Persistence/ArchPilot.Persistence.csproj backend/src/ArchPilot.Persistence/
COPY backend/src/ArchPilot.API/ArchPilot.API.csproj backend/src/ArchPilot.API/

RUN dotnet restore backend/src/ArchPilot.API/ArchPilot.API.csproj

COPY backend/src/ backend/src/

RUN dotnet publish backend/src/ArchPilot.API/ArchPilot.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:${PORT:-5157}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 5157

ENTRYPOINT ["dotnet", "ArchPilot.API.dll"]

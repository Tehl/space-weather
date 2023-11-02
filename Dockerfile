FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-app
WORKDIR /src
COPY ["src/SpaceWeather.Domain/SpaceWeather.Domain.csproj", "SpaceWeather.Domain/"]
COPY ["src/SpaceWeather.Bootstrap/SpaceWeather.Bootstrap.csproj", "SpaceWeather.Bootstrap/"]
COPY ["src/SpaceWeather.Sync/SpaceWeather.Sync.csproj", "SpaceWeather.Sync/"]
COPY ["src/SpaceWeather.Api/SpaceWeather.Api.csproj", "SpaceWeather.Api/"]
RUN dotnet restore "SpaceWeather.Bootstrap/SpaceWeather.Bootstrap.csproj" \
    && dotnet restore "SpaceWeather.Sync/SpaceWeather.Sync.csproj" \
    && dotnet restore "SpaceWeather.Api/SpaceWeather.Api.csproj"
COPY src .
RUN dotnet publish "SpaceWeather.Bootstrap/SpaceWeather.Bootstrap.csproj" -c Release -o /app/publish/bootstrap /p:UseAppHost=false \
    && dotnet publish "SpaceWeather.Sync/SpaceWeather.Sync.csproj" -c Release -o /app/publish/sync /p:UseAppHost=false \
    && dotnet publish "SpaceWeather.Api/SpaceWeather.Api.csproj" -c Release -o /app/publish/api /p:UseAppHost=false

FROM node:20.9.0 AS build-ui
WORKDIR /src
COPY ["ui/package*.json", "."]
RUN npm ci
COPY ["ui/src", "src"]
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=build-app /app/publish .
COPY --from=build-ui /src/dist ./wwwroot
ENTRYPOINT ["dotnet", "api/SpaceWeather.Api.dll"]

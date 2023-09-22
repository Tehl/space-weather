#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SpaceWeather.Sync/SpaceWeather.Sync.csproj", "SpaceWeather.Sync/"]
COPY ["SpaceWeather.Domain/SpaceWeather.Domain.csproj", "SpaceWeather.Domain/"]
RUN dotnet restore "SpaceWeather.Sync/SpaceWeather.Sync.csproj"
COPY . .
WORKDIR "/src/SpaceWeather.Sync"
RUN dotnet build "SpaceWeather.Sync.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpaceWeather.Sync.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpaceWeather.Sync.dll"]
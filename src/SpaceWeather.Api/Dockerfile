#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SpaceWeather.Api/SpaceWeather.Api.csproj", "SpaceWeather.Api/"]
COPY ["SpaceWeather.Domain/SpaceWeather.Domain.csproj", "SpaceWeather.Domain/"]
RUN dotnet restore "SpaceWeather.Api/SpaceWeather.Api.csproj"
COPY . .
WORKDIR "/src/SpaceWeather.Api"
RUN dotnet build "SpaceWeather.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpaceWeather.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpaceWeather.Api.dll"]
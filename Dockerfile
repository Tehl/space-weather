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
EXPOSE 80
ENV TZ UTC
RUN apt-get update && \
	apt-get install --no-install-recommends -y \
    cron
RUN touch /tmp/env.txt /tmp/sync.log \
    && echo "15,45 * * * *    . /tmp/env.txt; cd /app/sync && dotnet SpaceWeather.Sync.dll 2>&1 > /tmp/sync.log" > /etc/cron.d/spaceweather \
    && crontab /etc/cron.d/spaceweather
    
WORKDIR /app
COPY exec/ exec/
COPY --from=build-app /app/publish .
COPY --from=build-ui /src/dist ./api/wwwroot
RUN chmod a+x /app/exec/entrypoint.sh
ENTRYPOINT ["./exec/entrypoint.sh"]
CMD cron \
    && cd /app/api \
    && (dotnet SpaceWeather.Api.dll 2>&1 > /tmp/api.log & tail -f /tmp/sync.log /tmp/api.log)

#!/bin/sh

# dump env variables for cronjob to use
awk -v RS='\0' \
  '!/^(HOME|PATH|PWD|SHLVL|TERM|_)/ {gsub("\047", "\047\\\047\047"); print "export \047" $0 "\047"}' \
  /proc/self/environ > /tmp/env.txt

# bootstrap the database
cd /app/bootstrap && dotnet SpaceWeather.Bootstrap.dll
cd /app/sync && dotnet SpaceWeather.Sync.dll

# start the app
exec "$@"
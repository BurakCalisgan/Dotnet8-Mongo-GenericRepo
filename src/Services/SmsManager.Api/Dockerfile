FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/SmsManager.Api/SmsManager.Api.csproj", "src/Services/SmsManager.Api/"]
COPY ["src/Application/SmsManager.Application.EventContract/SmsManager.Application.EventContract.csproj", "src/Application/SmsManager.Application.EventContract/"]
COPY ["src/Application/SmsManager.Application/SmsManager.Application.csproj", "src/Application/SmsManager.Application/"]
COPY ["src/Domain/SmsManager.Domain/SmsManager.Domain.csproj", "src/Domain/SmsManager.Domain/"]
COPY ["src/Infrastructure/SmsManager.Infrastructure/SmsManager.Infrastructure.csproj", "src/Infrastructure/SmsManager.Infrastructure/"]
RUN dotnet restore "src/Services/SmsManager.Api/SmsManager.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/SmsManager.Api"
RUN dotnet build "SmsManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SmsManager.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# NewRelic
USER root
ARG NEW_RELIC_APP_NAME
ARG NEW_RELIC_LICENSE_KEY
ARG CORECLR_PROFILER
ARG CORECLR_ENABLE_PROFILING
ENV NEW_RELIC_DISTRIBUTED_TRACING_ENABLED="true"
ENV CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent
ENV CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so
ENV CORECLR_ENABLE_PROFILING="$CORECLR_ENABLE_PROFILING"
ENV CORECLR_PROFILER="$CORECLR_PROFILER"
ENV NEW_RELIC_LICENSE_KEY="$NEW_RELIC_LICENSE_KEY"
ENV NEW_RELIC_APP_NAME="$NEW_RELIC_APP_NAME"
RUN apt update && apt install -y ca-certificates curl gnupg
RUN curl -sL https://download.newrelic.com/548C16BF.gpg | apt-key add -
RUN echo "deb http://apt.newrelic.com/debian/ newrelic non-free" >> /etc/apt/sources.list.d/newrelic.list
RUN apt update && apt install -y newrelic-dotnet-agent
RUN apt clean && apt autoremove -y && rm -rf /var/lib/apt/lists/*  /var/cache/*
# NewRelic end

ENTRYPOINT ["dotnet", "SmsManager.Api.dll"]

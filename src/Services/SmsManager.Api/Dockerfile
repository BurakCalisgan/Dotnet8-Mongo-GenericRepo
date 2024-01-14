FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/SmsManager.Api/SmsManager.Api.csproj", "src/Services/SmsManager.Api/"]
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
ENTRYPOINT ["dotnet", "SmsManager.Api.dll"]

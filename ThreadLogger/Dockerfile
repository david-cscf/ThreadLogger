
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ThreadLogger/ThreadLogger.csproj", "ThreadLogger/"]
COPY ["ThreadLoggerLibrary/ThreadLoggerLibrary.csproj", "ThreadLoggerLibrary/"]
RUN dotnet restore "./ThreadLogger/./ThreadLogger.csproj"
COPY . .
WORKDIR "/src/ThreadLogger"
RUN dotnet build "./ThreadLogger.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ThreadLogger.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThreadLogger.dll"]
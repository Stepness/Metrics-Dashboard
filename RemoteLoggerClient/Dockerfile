﻿FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RemoteLoggerClient/RemoteLoggerClient.csproj", "RemoteLoggerClient/"]
RUN dotnet restore "RemoteLoggerClient/RemoteLoggerClient.csproj"
COPY . .
WORKDIR "/src/RemoteLoggerClient"
RUN dotnet build "RemoteLoggerClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RemoteLoggerClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RemoteLoggerClient.dll"]

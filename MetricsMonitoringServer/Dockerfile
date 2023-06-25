FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MetricsHub/MetricsHub.csproj", "MetricsHub/"]
RUN dotnet restore "MetricsHub/MetricsHub.csproj"
COPY . .
WORKDIR "/src/MetricsHub"
RUN dotnet build "MetricsHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MetricsHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetricsHub.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MangaMagnet.Api/MangaMagnet.Api.csproj", "MangaMagnet.Api/"]
COPY ["MangaMagnet.Core/MangaMagnet.Core.csproj", "MangaMagnet.Core/"]
RUN dotnet restore "MangaMagnet.Api/MangaMagnet.Api.csproj"
COPY . .
WORKDIR "/src/MangaMagnet.Api"
RUN dotnet build "MangaMagnet.Api.csproj" --no-restore -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MangaMagnet.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MangaMagnet.Api.dll"]

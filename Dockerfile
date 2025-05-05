FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY ["src/FluxConfig.Storage.Domain/FluxConfig.Storage.Domain.csproj", "FluxConfig.Storage.Domain/"]
COPY ["src/FluxConfig.Storage.Infrastructure/FluxConfig.Storage.Infrastructure.csproj", "FluxConfig.Storage.Infrastructure/"]
COPY ["src/FluxConfig.Storage.Api/FluxConfig.Storage.Api.csproj", "FluxConfig.Storage.Api/"]

RUN dotnet restore "FluxConfig.Storage.Api/FluxConfig.Storage.Api.csproj" --arch ${TARGETARCH}
COPY src/. .
RUN dotnet build "FluxConfig.Storage.Api/FluxConfig.Storage.Api.csproj" -c Release --no-restore

WORKDIR "/src/FluxConfig.Storage.Api"
FROM build AS publish
RUN dotnet publish "FluxConfig.Storage.Api.csproj" --arch ${TARGETARCH}  -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxConfig.Storage.Api.dll"]
FROM --platform=${BUILDPLATFORM} mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY ["src/FluxConfig.Storage.Domain/FluxConfig.Storage.Domain.csproj", "FluxConfig.Storage.Domain/"]
COPY ["src/FluxConfig.Storage.Infrastructure/FluxConfig.Storage.Infrastructure.csproj", "FluxConfig.Storage.Infrastructure/"]
COPY ["src/FluxConfig.Storage.Api/FluxConfig.Storage.Api.csproj", "FluxConfig.Storage.Api/"]

RUN dotnet restore "FluxConfig.Storage.Api/FluxConfig.Storage.Api.csproj" --arch ${TARGETARCH}
COPY src/. .
RUN dotnet publish "FluxConfig.Storage.Api/FluxConfig.Storage.Api.csproj" --arch ${TARGETARCH} -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "FluxConfig.Storage.Api.dll"]
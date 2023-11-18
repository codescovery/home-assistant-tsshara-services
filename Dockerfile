ARG BUILD_FROM=ghcr.io/hassio-addons/base:14.3.2 
FROM ${BUILD_FROM} AS base-addon
RUN apk add --no-cache wget curl && \
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY src/ .
WORKDIR /src/TsShara.Services/Application/TsShara.Services.Application
RUN dotnet restore TsShara.Services.Application.csproj
RUN dotnet build TsShara.Services.Application.csproj -c "$BUILD_CONFIGURATION"  -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish TsShara.Services.Application.csproj -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false



# hadolint ignore=DL3006
FROM base-addon AS final
ENV ASPNETCORE_URLS=http://+:80;http://+:443
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true
WORKDIR /app
COPY run.sh  .
RUN chmod a+x run.sh
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
CMD ["/app/run.sh"]
#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TsShara.Services.Worker/TsShara.Services.Worker.csproj", "TsShara.Services.Worker/"]
COPY ["Domain/TsShara.Services.Domain/TsShara.Services.Domain.csproj", "Domain/TsShara.Services.Domain/"]
RUN dotnet restore "./TsShara.Services.Worker/./TsShara.Services.Worker.csproj"
COPY . .
WORKDIR "/src/TsShara.Services.Worker"
RUN dotnet build "./TsShara.Services.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TsShara.Services.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TsShara.Services.Worker.dll"]
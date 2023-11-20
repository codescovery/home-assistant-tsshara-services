#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Application/TsShara.Services.Application/TsShara.Services.Application.csproj", "Application/TsShara.Services.Application/"]
COPY ["Domain/TsShara.Services.Domain/TsShara.Services.Domain.csproj", "Domain/TsShara.Services.Domain/"]
RUN dotnet restore "./Application/TsShara.Services.Application/./TsShara.Services.Application.csproj"
COPY . .
WORKDIR "/src/Application/TsShara.Services.Application"
RUN dotnet build "./TsShara.Services.Application.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TsShara.Services.Application.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TsShara.Services.Application.dll"]
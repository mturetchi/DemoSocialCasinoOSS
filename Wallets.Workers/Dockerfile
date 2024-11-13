#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wallets.Workers/Wallets.Workers.csproj", "Wallets.Workers/"]
COPY ["Wallets.Application/Wallets.Application.csproj", "Wallets.Application/"]
COPY ["Application.Shared/Application.Shared.csproj", "Application.Shared/"]
COPY ["Wallets.Core/Wallets.Core.csproj", "Wallets.Core/"]
COPY ["Core.Shared/Core.Shared.csproj", "Core.Shared/"]
COPY ["Wallets.Infrastructure/Wallets.Infrastructure.csproj", "Wallets.Infrastructure/"]
COPY ["Infrastructure.Shared/Infrastructure.Shared.csproj", "Infrastructure.Shared/"]
COPY ["Workers.Shared/Workers.Shared.csproj", "Workers.Shared/"]
RUN dotnet restore "./Wallets.Workers/Wallets.Workers.csproj"
COPY . .
WORKDIR "/src/Wallets.Workers"
RUN dotnet build "./Wallets.Workers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Wallets.Workers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wallets.Workers.dll"]
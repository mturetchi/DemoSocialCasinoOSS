#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Bets.Workers/Bets.Workers.csproj", "Bets.Workers/"]
COPY ["Bets.Application/Bets.Application.csproj", "Bets.Application/"]
COPY ["Application.Shared/Application.Shared.csproj", "Application.Shared/"]
COPY ["Bets.Contracts/Bets.Contracts.csproj", "Bets.Contracts/"]
COPY ["Bets.Core/Bets.Core.csproj", "Bets.Core/"]
COPY ["Core.Shared/Core.Shared.csproj", "Core.Shared/"]
COPY ["Bets.Infrastructure/Bets.Infrastructure.csproj", "Bets.Infrastructure/"]
COPY ["Infrastructure.Shared/Infrastructure.Shared.csproj", "Infrastructure.Shared/"]
COPY ["Wallets.Application/Wallets.Application.csproj", "Wallets.Application/"]
COPY ["Wallets.Contracts/Wallets.Contracts.csproj", "Wallets.Contracts/"]
COPY ["Wallets.Core/Wallets.Core.csproj", "Wallets.Core/"]
RUN dotnet restore "./Bets.Workers/Bets.Workers.csproj"
COPY . .
WORKDIR "/src/Bets.Workers"
RUN dotnet build "./Bets.Workers.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bets.Workers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bets.Workers.dll"]
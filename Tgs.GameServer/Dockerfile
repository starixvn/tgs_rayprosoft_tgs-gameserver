#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameServer/Tgs.GameServer.csproj", "Tgs.GameServer/"]
COPY ["GameServer.Core/GameServer.Core.csproj", "GameServer.Core/"]
COPY ["GameServer.Common/GameServer.Common.csproj", "GameServer.Common/"]
COPY ["GameServer.Repositories/GameServer.Repositories.csproj", "GameServer.Repositories/"]
RUN dotnet restore "Tgs.GameServer/Tgs.GameServer.csproj"
COPY . .
WORKDIR "/src/Tgs.GameServer"
RUN dotnet build "Tgs.GameServer.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameServer.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameServer.dll"]
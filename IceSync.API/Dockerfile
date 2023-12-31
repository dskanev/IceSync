#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IceSync.API/IceSync.API.csproj", "IceSync.API/"]
COPY ["IceSync.Application/IceSync.Application.csproj", "IceSync.Application/"]
COPY ["IceSync.Infrastructure/IceSync.Infrastructure.csproj", "IceSync.Infrastructure/"]
COPY ["IceSync.Persistance/IceSync.Persistance.csproj", "IceSync.Persistance/"]
RUN dotnet restore "IceSync.API/IceSync.API.csproj"
COPY . .
WORKDIR "/src/IceSync.API"
RUN dotnet build "IceSync.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IceSync.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IceSync.API.dll"]
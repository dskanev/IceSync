#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["IceSync.UI/IceSync.UI.csproj", "IceSync.UI/"]
RUN dotnet restore "IceSync.UI/IceSync.UI.csproj"
COPY . .
WORKDIR "/src/IceSync.UI"
RUN dotnet build "IceSync.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IceSync.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IceSync.UI.dll"]
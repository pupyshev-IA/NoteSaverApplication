#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Abdt.Loyal.NoteSaver.Web/Server/Abdt.Loyal.NoteSaver.Web.Server.csproj", "Abdt.Loyal.NoteSaver.Web/Server/"]
COPY ["Abdt.Loyal.NoteSaver.Web/Client/Abdt.Loyal.NoteSaver.Web.Client.csproj", "Abdt.Loyal.NoteSaver.Web/Client/"]
COPY ["Abdt.Loyal.NoteSaver.Web/Shared/Abdt.Loyal.NoteSaver.Web.Shared.csproj", "Abdt.Loyal.NoteSaver.Web/Shared/"]
RUN dotnet restore "./Abdt.Loyal.NoteSaver.Web/Server/Abdt.Loyal.NoteSaver.Web.Server.csproj"
COPY . .
WORKDIR "/src/Abdt.Loyal.NoteSaver.Web/Server"
RUN dotnet build "./Abdt.Loyal.NoteSaver.Web.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Abdt.Loyal.NoteSaver.Web.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Abdt.Loyal.NoteSaver.Web.Server.dll"]
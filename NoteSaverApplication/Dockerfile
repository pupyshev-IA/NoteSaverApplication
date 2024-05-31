#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["NoteSaverApplication/Abdt.Loyal.NoteSaver.csproj", "NoteSaverApplication/"]
COPY ["Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions/Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions.csproj", "Abdt.Loyal.NoteSaver.BusinessLogic.Abstractions/"]
COPY ["Abdt.Loyal.NoteSaver.Domain/Abdt.Loyal.NoteSaver.Domain.csproj", "Abdt.Loyal.NoteSaver.Domain/"]
COPY ["Abdt.Loyal.NoteSaver.BusinessLogic/Abdt.Loyal.NoteSaver.BusinessLogic.csproj", "Abdt.Loyal.NoteSaver.BusinessLogic/"]
COPY ["Abdt.Loyal.NoteSaver.Repository.Abstractions/Abdt.Loyal.NoteSaver.Repository.Abstractions.csproj", "Abdt.Loyal.NoteSaver.Repository.Abstractions/"]
COPY ["Abdt.Loyal.NoteSaver.Repository/Abdt.Loyal.NoteSaver.Repository.csproj", "Abdt.Loyal.NoteSaver.Repository/"]
RUN dotnet restore "NoteSaverApplication/Abdt.Loyal.NoteSaver.csproj"
COPY . .
WORKDIR "/src/NoteSaverApplication"
RUN dotnet build "Abdt.Loyal.NoteSaver.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Abdt.Loyal.NoteSaver.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Abdt.Loyal.NoteSaver.dll"]
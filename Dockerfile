#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SSLValidator/Server/SSLValidator.Server.csproj", "SSLValidator/Server/"]
COPY ["SSLValidator/Shared/SSLValidator.Shared.csproj", "SSLValidator/Shared/"]
COPY ["SSLValidator/Client/SSLValidator.Client.csproj", "SSLValidator/Client/"]
RUN dotnet restore "SSLValidator/Server/SSLValidator.Server.csproj"
COPY . .
WORKDIR "/src/SSLValidator/Server"
RUN dotnet build "SSLValidator.Server.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "SSLValidator.Server.csproj" -c Development -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SSLValidator.Server.dll"]
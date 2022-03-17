FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN ls
COPY . .
RUN ls
RUN dotnet restore "Server/SSLValidator.Server.csproj"
RUN dotnet build "Server/SSLValidator.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server/SSLValidator.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server/SSLValidator.Server.dll"]
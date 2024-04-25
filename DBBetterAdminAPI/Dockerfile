FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BetterAdminDbAPI.csproj", "."]
RUN dotnet restore "./BetterAdminDbAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BetterAdminDbAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetterAdminDbAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://*:8080

ENTRYPOINT ["dotnet", "BetterAdminDbAPI.dll"]
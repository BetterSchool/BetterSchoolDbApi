FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

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
ENTRYPOINT ["dotnet", "BetterAdminDbAPI.dll"]
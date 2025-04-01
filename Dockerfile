# Gebruik een officiële .NET 9 SDK image als basis
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Stel de werkdirectory in
WORKDIR /app

# Kopieer het project bestand en herstel afhankelijkheden
COPY Spendo-Backend/*.csproj ./
RUN dotnet restore

# Kopieer de resterende bestanden en bouw de applicatie
COPY Spendo-Backend/. ./
RUN dotnet publish -c Release -o /app/publish

# Stel de image in voor het draaien van de app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080

# Stel de omgeving in voor het draaien van de applicatie
ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

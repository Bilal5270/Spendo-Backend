# Bouwfase
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Kopieer csproj en herstel afhankelijkheden
COPY..
RUN dotnet restore  Spendo-Backend/*.csproj
RUN dotnet publish -c Release -o /app/publish
# Kopieer alle bestanden en publiceer de app
COPY Spendo-Backend/. ./


# Basisimage voor de uitvoer
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Kopieer de gepubliceerde bestanden vanuit de buildfase
COPY --from=build /app/publish ./

# Stel de omgeving in (optioneel, bijvoorbeeld voor Development)
ENV ASPNETCORE_ENVIRONMENT Development

# Exporteer poorten voor zowel HTTP als HTTPS
EXPOSE 8080

# Start de applicatie
ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

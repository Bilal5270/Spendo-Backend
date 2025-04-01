# Bouwfase
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

# Kopieer csproj en herstel afhankelijkheden
COPY..
RUN dotnet restore  Spendo-Backend/*.csproj
RUN dotnet publish Spendo-Backend/*.csproj -c Release -o out

# Basisimage voor de uitvoer
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Kopieer de gepubliceerde bestanden vanuit de buildfase
COPY --from=build /app/publish ./

# Exporteer poorten voor zowel HTTP als HTTPS
EXPOSE 8080

# Start de applicatie
ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

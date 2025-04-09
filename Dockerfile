# Gebruik een officiële .NET 9 SDK image als basis
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Stel de werkdirectory in voor de source
WORKDIR /src

# Kopieer volledige backend-map
COPY Spendo-Backend/ ./Spendo-Backend/

# Ga naar de projectmap
WORKDIR /src/Spendo-Backend

# Herstel dependencies
RUN dotnet restore

# Publiceer de app
RUN dotnet publish -c Release -o /app/publish

# Gebruik een lichtere runtime image voor productie
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

# Stel poort in waarop de app luistert
EXPOSE 8080

# Start de app
ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

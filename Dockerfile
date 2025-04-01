FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY . .

RUN dotnet restore Spendo-Backend/Spendo-Backend.csproj

RUN dotnet publish Spendo-Backend/Spendo-Backend.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Kopieer de gepubliceerde bestanden vanuit de buildfase
COPY --from=build /app/publish ./

# Stel de omgeving in (optioneel, bijvoorbeeld voor Development)
ENV ASPNETCORE_ENVIRONMENT Development

EXPOSE 8080

ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

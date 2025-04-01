FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env

WORKDIR /app

COPY . .

RUN dotnet restore Spendo-Backend/Spendo-Backend.csproj

RUN dotnet publish Spendo-Backend/Spendo-Backend.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

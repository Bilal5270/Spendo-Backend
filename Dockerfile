FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY Spendo-Backend/Spendo-Backend.csproj ./Spendo-Backend.csproj
RUN dotnet restore Spendo-Backend.csproj
RUN dotnet publish Spendo-Backend.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
EXPOSE 8523
ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

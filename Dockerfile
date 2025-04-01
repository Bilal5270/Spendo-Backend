FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY Spendo-Backend/*.csproj ./
RUN dotnet restore


COPY Spendo-Backend/. ./
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

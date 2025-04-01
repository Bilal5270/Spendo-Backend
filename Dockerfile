# Stage 1: Build the server
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the csproj file(s) and restore
COPY Spendo-Backend/*.csproj ./
RUN dotnet restore

# Copy the rest of the files and build
COPY . ./
RUN dotnet publish -c Release -o /app/publish 

# Stage 2: Run the server
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5186

ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

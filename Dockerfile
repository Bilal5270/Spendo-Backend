# Dockerfile for the .NET backend (MisterPasta.Server)

# Stage 1: Build the server
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build_server
WORKDIR /app

# Copy the csproj file(s) and restore
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the files and build
COPY . ./
RUN dotnet publish -c Release -o /app

# Stage 2: Run the server
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS server
WORKDIR /app
COPY --from=build_server /app .
EXPOSE 5186

ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

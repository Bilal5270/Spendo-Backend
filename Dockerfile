# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY Spendo-Backend/Spendo-Backend.csproj ./
RUN dotnet restore Spendo-Backend.csproj

# Publish the application
RUN dotnet publish Spendo-Backend.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/out ./

# Expose port 8080
EXPOSE 8523

# Start the application
ENTRYPOINT ["dotnet", "Spendo-Backend.dll"]

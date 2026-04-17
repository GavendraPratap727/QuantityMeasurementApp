# Use the official .NET 9.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["QuantityMeasurementWebAPI.csproj", "./"]
COPY ["../QuantityMeasurementBusinessLayer/QuantityMeasurementBusinessLayer.csproj", "../QuantityMeasurementBusinessLayer/"]
COPY ["../QuantityMeasurementRepositoryLayer/QuantityMeasurementRepositoryLayer.csproj", "../QuantityMeasurementRepositoryLayer/"]
COPY ["../QuantityMeasurementModelLayer/QuantityMeasurementModelLayer.csproj", "../QuantityMeasurementModelLayer/"]
RUN dotnet restore "./QuantityMeasurementWebAPI.csproj"

# Copy the entire source code
COPY . ./
WORKDIR "/src/."

# Build the application
RUN dotnet build "QuantityMeasurementWebAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "QuantityMeasurementWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Create the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the environment variable for ASP.NET Core to use the correct port
ENV ASPNETCORE_URLS=http://+:8080

# Start the application
ENTRYPOINT ["dotnet", "QuantityMeasurementWebAPI.dll"]

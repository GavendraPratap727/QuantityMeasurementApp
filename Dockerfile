# Use the official .NET 9.0 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET 9.0 SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["QuantityMeasurementWebAPI/QuantityMeasurementWebAPI.csproj", "QuantityMeasurementWebAPI/"]
COPY ["QuantityMeasurementBusinessLayer/QuantityMeasurementBusinessLayer.csproj", "QuantityMeasurementBusinessLayer/"]
COPY ["QuantityMeasurementRepositoryLayer/QuantityMeasurementRepositoryLayer.csproj", "QuantityMeasurementRepositoryLayer/"]
COPY ["QuantityMeasurementModelLayer/QuantityMeasurementModelLayer.csproj", "QuantityMeasurementModelLayer/"]

RUN dotnet restore "QuantityMeasurementWebAPI/QuantityMeasurementWebAPI.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/QuantityMeasurementWebAPI"
RUN dotnet build "QuantityMeasurementWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuantityMeasurementWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for Render
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "QuantityMeasurementWebAPI.dll"]

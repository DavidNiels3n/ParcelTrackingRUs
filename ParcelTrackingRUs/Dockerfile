# Stage 1: Base image for running the app (ASP.NET runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
ENV DOTNET_URLS=http://+5001
EXPOSE 5001



# Stage 2: Build image with SDK for compiling and building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the csproj and restore any dependencies (via NuGet)
COPY ["ParcelTrackingRUs.csproj", "ParcelTrackingRUs/"]
RUN dotnet restore "./ParcelTrackingRUs/ParcelTrackingRUs.csproj"

# Copy the entire source code to the container
COPY . /source

# Set the working directory and build the app
WORKDIR "/source"
RUN dotnet build "ParcelTrackingRUs.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish the app to a folder for production
FROM build AS publish
RUN dotnet publish "ParcelTrackingRUs.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image with the base runtime and published app
FROM base AS final
WORKDIR /app

# Copy the published app from the build container
COPY --from=publish /app/publish .

# Set the entry point to start the app
ENTRYPOINT ["dotnet", "ParcelTrackingRUs.dll"]

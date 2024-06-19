# Use the official .NET 8.0 SDK image as the build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project files and restore dependencies
COPY src/reportMicroservice/WebAPI/WebAPI.csproj ./src/reportMicroservice/WebAPI/
COPY src/reportMicroservice/Application/Application.csproj ./src/reportMicroservice/Application/
COPY src/reportMicroservice/Domain/Domain.csproj ./src/reportMicroservice/Domain/
COPY src/reportMicroservice/Infrastructure/Infrastructure.csproj ./src/reportMicroservice/Infrastructure/
RUN dotnet restore ./src/reportMicroservice/WebAPI/WebAPI.csproj

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish ./src/reportMicroservice/WebAPI/WebAPI.csproj -c Release -o /out

# Use the official .NET 8.0 runtime image as the base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Expose the port the app runs on
EXPOSE 81

# SQL Server trusted connection problem
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf

# Default entrypoint for the web application
ENTRYPOINT ["dotnet", "WebAPI.dll"]

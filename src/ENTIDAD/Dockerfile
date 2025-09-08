# Use the official .NET 9 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src/ENTIDAD

# Copy csproj and restore as distinct layers
COPY ENTIDAD.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . ./
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Final stage: output the published files (optional, for use in multi-stage builds)
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

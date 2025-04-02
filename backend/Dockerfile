# syntax=docker/dockerfile:1

################################################################################

# Stage 1: Build the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY ./backend/*.csproj ./
RUN dotnet restore

# Copy remaining files and build the application
COPY . ./


#####
RUN dotnet publish -c Release -o out
################################################################################

# Stage 2: Runtime image (final image).
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build /app/out ./

# Expose the port and define entrypoint
EXPOSE 80
CMD ["dotnet", "cSharpQuiz.dll"]

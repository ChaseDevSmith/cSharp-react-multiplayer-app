# Use the official .NET SDK image as the build image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /source

# Define the architecture for the build (passed in by the builder)
ARG TARGETARCH

# Copy the .csproj file and restore any dependencies (via dotnet restore)
COPY ./source/cSharpQuiz.csproj .
RUN dotnet restore ./cSharpQuiz.csproj -a $TARGETARCH

# Copy the rest of the source code
COPY . ./

# Build the application and publish it to a folder in the container
RUN dotnet publish ./cSharpQuiz.csproj -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app/publish

# Use a smaller base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app
EXPOSE 5501

# Copy the published output from the build image to the runtime image
COPY --from=build /app/publish .

# Switch to a non-privileged user (if you want, to improve security)
USER $APP_UID

# Define the entry point for the application
ENTRYPOINT ["dotnet", "cSharpQuiz.dll"]

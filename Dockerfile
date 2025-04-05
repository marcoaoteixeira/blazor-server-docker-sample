# Get our SDK
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Define layer arguments
ARG TARGETARCH # this argument is provided by Docker
ARG BUILD_CONFIG=Release

# Setting working directory to "src"
WORKDIR /src

# Copy all necessary files
COPY --link ["*.props", "."]
COPY --link ["src/Nameless.BlazorServerDocker.Web/", "Nameless.BlazorServerDocker.Web/"]

# Restore the main project. Remember that we are at "src" working directory,
# so there is not need to specify "src" at the begging of the path.
RUN dotnet restore "Nameless.BlazorServerDocker.Web/Nameless.BlazorServerDocker.Web.csproj" -a ${TARGETARCH}

# Publish our application
RUN dotnet publish "Nameless.BlazorServerDocker.Web/Nameless.BlazorServerDocker.Web.csproj" -c ${BUILD_CONFIG} -a ${TARGETARCH} --no-restore -o /app

# Run our application
FROM mcr.microsoft.com/dotnet/aspnet:9.0

ARG HTTPS_PORT=443
ARG HTTP_PORT=80

# Expose container port
EXPOSE ${HTTPS_PORT}
EXPOSE ${HTTP_PORT}

# Let's set "app" as our working directory from here on.
WORKDIR /app

# Copy everything inside the "publish" folder (look into Publish layer)
# to our final directory, "app"
COPY --from=build /app .

USER ${APP_UID}

# Start our application
ENTRYPOINT ["dotnet", "Nameless.BlazorServerDocker.Web.dll"]
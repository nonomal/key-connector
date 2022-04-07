###############################################
#                 Build stage                 #
###############################################
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

# Copy csproj files as distinct layers
WORKDIR /source
COPY src/KeyConnector/*.csproj ./src/KeyConnector/

# Restore project dependencies and tools
WORKDIR /source/src/KeyConnector
RUN dotnet restore

# Copy required project files
WORKDIR /source
COPY src/KeyConnector/. ./src/KeyConnector/

# Build app
WORKDIR /source/src/KeyConnector
RUN dotnet publish -c release -o /app --no-restore

###############################################
#                  App stage                  #
###############################################
FROM mcr.microsoft.com/dotnet/aspnet:5.0
LABEL com.bitwarden.product="bitwarden"
LABEL com.bitwarden.project="keyconnector"
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000

# Add packages
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        gosu \
        curl \
        libc-dev \
        opensc \
    && rm -rf /var/lib/apt/lists/*

# Install YubiHSM2 SDK
ADD https://developers.yubico.com/YubiHSM2/Releases/yubihsm2-sdk-2021-08-debian10-amd64.tar.gz ./
RUN tar -xzf yubihsm2-sdk-*.tar.gz \
    && rm yubihsm2-sdk-*.tar.gz \
    && dpkg -i yubihsm2-sdk/libyubihsm-http1_*_amd64.deb \
    && dpkg -i yubihsm2-sdk/libyubihsm1_*_amd64.deb \
    && dpkg -i yubihsm2-sdk/yubihsm-pkcs11_*_amd64.deb \
    && apt-get install -f

# Create required directories
RUN mkdir -p /etc/bitwarden/key-connector
RUN mkdir -p /etc/bitwarden/logs
RUN mkdir -p /etc/bitwarden/ca-certificates

# Copy app from build stage
WORKDIR /app
COPY --from=build /app ./

# Copy entrypoint script and make it executable
COPY docker/entrypoint.sh /
RUN chmod +x /entrypoint.sh

# Create non-root user to run app
RUN adduser -s /bin/false -D bitwarden && chown -R bitwarden:bitwarden /app /etc/bitwarden

USER bitwarden:bitwarden
HEALTHCHECK CMD curl -f http://localhost:5000/alive || exit 1
ENTRYPOINT ["/entrypoint.sh"]

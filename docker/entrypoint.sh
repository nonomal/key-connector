#!/bin/bash

if [ ! -f /etc/bitwarden/key-connector/bwkc.pfx ]; then
  openssl req \
  -x509 \
  -newkey rsa:4096 \
  -sha256 \
  -nodes \
  -keyout bwkc.key \
  -out bwkc.crt \
  -subj "/CN=Bitwarden Key Connector" \
  -days 36500

  openssl pkcs12 \
  -export \
  -out /etc/bitwarden/key-connector/bwkc.pfx \
  -inkey bwkc.key \
  -in bwkc.crt \
  -passout pass:$keyConnectorSettings__certificate__filesystemPassword

  rm bwkc.crt
  rm bwkc.key
fi

cp /etc/bitwarden/ca-certificates/*.crt /usr/local/share/ca-certificates/ >/dev/null 2>&1 \
    && update-ca-certificates

exec dotnet /app/KeyConnector.dll

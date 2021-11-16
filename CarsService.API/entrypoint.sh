#!/bin/bash

set -e

echo "Hello worl"
export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef database update; do
    >&2 echo "Migrations executing"
    sleep 1
done

>&2 echo "DB Migrations complete, starting app."

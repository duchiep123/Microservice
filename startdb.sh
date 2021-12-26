#!/bin/bash

set -e
run_cmd="dotnet run --no-build --urls http://0.0.0.0:80 -v d"

echo "Hello world"
export PATH="$PATH:/root/.dotnet/tools"

until dotnet ef database update; do
    >&2 echo "Migrations executing"
    sleep 1
done

>&2 echo "DB Migrations complete, starting app."
exec $run_cmd
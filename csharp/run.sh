#!/bin/bash
set -eou pipefail

if [[ -z "${AOC_SESSION_COOKIE}" ]]; then
    echo "Session token (AOC_SESSION_COOKIE) not set"
    exit
fi

YEAR="${1}"
DAY="${2}"

if [[ -z "${YEAR}" || -z "${DAY}" ]]; then
    YEAR="$(date +%Y)"
    DAY="$(date +%d)"
fi

if [[ "${DAY}" -lt 10 ]]; then
    DAY="0${DAY}"
fi

dotnet test ${YEAR}/${YEAR}.csproj --filter "FullyQualifiedName~AdventOfCode${YEAR}.Day${DAY}"


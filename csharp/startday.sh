#!/bin/bash

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

DAY_NO_ZEROS="$(echo ${DAY} | sed 's/^0*//')"
TEMPLATE="Dayxx.cs"
CODE_FILE="${YEAR}/Day${DAY}/Day${DAY}.cs"
PUZZLE_URL="https://adventofcode.com/${YEAR}/day/${DAY_NO_ZEROS}/input"
PUZZLE_FILE="${YEAR}/Day${DAY}/Puzzle Input.txt"
SAMPLE_FILE="${YEAR}/Day${DAY}/Sample.txt"

if [[ ! -f "${PUZZLE_FILE}" ]]; then
    curl "${PUZZLE_URL}" -H "cookie: session=${AOC_SESSION_COOKIE}" -o "${PUZZLE_FILE}" 2>/dev/null
    truncate -s -1 "${PUZZLE_FILE}"
fi
if [[ ! -f "${CODE_FILE}" ]]; then
    cp "${TEMPLATE}" "${CODE_FILE}"
    sed -i "s/xx/${DAY}/g" "${CODE_FILE}"
    sed -i "s/Dayxx/Day${DAY}/g" "${CODE_FILE}"
fi
if [[ ! -f "${SAMPLE_FILE}" ]]; then
    touch "${SAMPLE_FILE}"
fi
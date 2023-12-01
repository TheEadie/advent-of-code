#!/bin/bash
set -eou pipefail

if [[ -z "${AOC_SESSION_COOKIE}" ]]; then
    echo "Session token (AOC_SESSION_COOKIE) not set"
    exit
fi

YEAR="${1-$(date +%Y)}"
DAY="${2-$(date +%d)}"

# If day is less than 10 and doesn't have a leading zero, add one
if [[ "${DAY}" -lt 10 && "${DAY}" != 0* ]]; then
    DAY="0${DAY}"
fi

DAY_NO_ZEROS="$(echo ${DAY} | sed 's/^0*//')"
TEMPLATE="Dayxx.cs"
CODE_DIR="${YEAR}/Day${DAY}"
CODE_FILE="${YEAR}/Day${DAY}/Day${DAY}.cs"
PUZZLE_URL="https://adventofcode.com/${YEAR}/day/${DAY_NO_ZEROS}"
PUZZLE_INPUT_URL="https://adventofcode.com/${YEAR}/day/${DAY_NO_ZEROS}/input"
PUZZLE_FILE="${YEAR}/Day${DAY}/Puzzle Input.txt"
SAMPLE_FILE="${YEAR}/Day${DAY}/Sample.txt"

mkdir -p "${CODE_DIR}"

if [[ ! -f "${PUZZLE_FILE}" ]]; then
    curl "${PUZZLE_INPUT_URL}" -H "cookie: session=${AOC_SESSION_COOKIE}" -o "${PUZZLE_FILE}" 2>/dev/null
    truncate -s -1 "${PUZZLE_FILE}"
fi
if [[ ! -f "${CODE_FILE}" ]]; then
    # Get the Puzzle Title
    PUZZLE_TEXT="${YEAR}/Day${DAY}/Puzzle.txt"
    curl "${PUZZLE_URL}" -o "${PUZZLE_TEXT}" 2>/dev/null
    PUZZLE_TITLE="$(sed -n 's/.*<article class="day-desc"><h2>--- Day [0-9]*: \([^<]*\) ---<\/h2>.*/\1/p' "${PUZZLE_TEXT}")"
    rm "${PUZZLE_TEXT}"

    # Update the template
    cp "${TEMPLATE}" "${CODE_FILE}"
    sed -i "s/DAYNOZEROPAD/${DAY_NO_ZEROS}/g" "${CODE_FILE}"
    sed -i "s/TITLE/${PUZZLE_TITLE}/g" "${CODE_FILE}"
    sed -i "s/YEAR/${YEAR}/g" "${CODE_FILE}"
    sed -i "s/0000/${YEAR}/g" "${CODE_FILE}"
    sed -i "s/xx/${DAY}/g" "${CODE_FILE}"
    sed -i "s/00/${DAY}/g" "${CODE_FILE}"
    sed -i "s/Dayxx/Day${DAY}/g" "${CODE_FILE}"
fi
if [[ ! -f "${SAMPLE_FILE}" ]]; then
    touch "${SAMPLE_FILE}"
fi

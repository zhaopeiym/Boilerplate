#!/bin/bash
set -e
dotnet new --install /app/File/BoilerplateProject
mkdir -p /app/File/DownloadProject/
rm -rf /app/File/DownloadProject/*

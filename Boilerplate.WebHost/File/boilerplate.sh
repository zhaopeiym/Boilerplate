#!/bin/bash
set -e
#dotnet new --install /app/File/BoilerplateProject
#mkdir -p /app/File/DownloadProject/$1
dotnet new myproject -o /app/File/DownloadProject/$1
cd /app/File/DownloadProject
tar -cvf $1.tar $1

#!/bin/bash
export PATH=$PATH:$HOME/.dotnet
export DOTNET_ROOT=$HOME/.dotnet
export LOGGING__LOGLEVEL__DEFAULT=Information
export LOGGING__LOGLEVEL__MICROSOFT=Warning
CONFIG_PATH=/data/options.json
dotnet TsShara.Services.Application.dll
#!/bin/bash
export PATH=$PATH:$HOME/.dotnet
export DOTNET_ROOT=$HOME/.dotnet
export LOGGING__LOGLEVEL__DEFAULT=Information
export LOGGING__LOGLEVEL__MICROSOFT=Warning
export CONFIG_PATH=/data/options.json

set_environment_variable() {
    local env_variable_name="$1"
    local config_name="$2"

    local config_value="$(bashio::config "$config_name")"
    # print config value to log but not to stdout
    echo "$config_name is set to $config_value" >&2

    
    if [ -n "$config_value" ]; then
        # Variable is defined and not empty
        export "$env_variable_name"="$config_value"
        echo "$env_variable_name is set to $config_value"
    else
        echo "$config_name is not defined or is empty"
    fi
}
set_environment_variable "AppSettings__TsShara__SerialPortName" "serial_portname"
dotnet TsShara.Services.Application.dll

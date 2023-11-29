#!/usr/bin/env bashio
export PATH=$PATH:$HOME/.dotnet
export DOTNET_ROOT=$HOME/.dotnet
export LOGGING__LOGLEVEL__DEFAULT=Information
export LOGGING__LOGLEVEL__MICROSOFT=Warning


set_environment_variable() {
    CONFIG_PATH=/data/options.json
    local env_variable_name="$1"
    local config_name="$2"
    
    local config_value="$(bashio::config "$config_name")"
    # print config value to log but not to stdout
    echo "$config_name is set to $config_value" >&2

    
    if [ -n "$config_value" ] && [ "$config_value" != "null" ]; then
        # Variable is defined and not empty
        export "$env_variable_name"="$config_value"
        echo "$env_variable_name is set to $config_value"
    else
        if [ $# -ge 3 ] && [ -n "$3" ] && [ "$3" != "null" ]; then
            echo "Using default value for $config_name" >&2
            local default_value="$3"
            # Variable is not defined, use default value
            export "$env_variable_name"="$default_value"
            echo "$env_variable_name is set to $default_value"
        else
            echo "$config_name is not defined or is empty and has no default value, skipping..." >&2
        fi
        
    fi
}
set_environment_variable "AppSettings__TsShara__SerialPortName" "serial_portname"
set_environment_variable "ASPNETCORE_URLS" "aspnetcore_urls" "http://+:8099"
set_environment_variable "Api__Endpoint" "swagger_server_configuration"
dotnet TsShara.Services.Application.dll

# About 
This project aims to provide an api addon to check the status of the Ts-Shara nobreaks.

[![Open your Home Assistant instance and show the add add-on repository dialog with a specific repository URL pre-filled.](https://my.home-assistant.io/badges/supervisor_add_addon_repository.svg)](https://my.home-assistant.io/redirect/supervisor_add_addon_repository/?repository_url=https%3A%2F%2Fgithub.com%2Fcodescovery%2Fhome-assistant-tsshara-services)

## Pre-requisites
- Home Assistant
- Supervisor
- Ts-Shara Nobreak
- Ts-Shara Nobreak USB cable connected to the Home Assistant server

## Installation
1. Add the repository to the Supervisor Add-on Store
2. Install the addon
3. Configure the addon
4. Start the addon
5. Check the logs of the addon to see if everything went well


## Configure a Ts-Shara Nobreak Rest-Api Sensor

To configure a Ts-Shara Nobreak Rest-Api Sensor, add the following lines to your configuration.yaml file:

```yaml
# Example configuration.yaml entry
sensor:
  - platform: rest
    name: Ts-Shara Nobreak
    resource: http://localhost:8123/api/ts-shara/status
    value_template: '{{ value_json.status }}'

```

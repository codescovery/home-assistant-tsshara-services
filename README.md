# tsshara-services
Project destinated to monitor Ts-Share No Break status


# Publish to raspberry pi 2+


## Build
```shell
#intall dotnet
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --channel 8.0
dotnet publish --runtime linux-arm64
```

## Copy publish result 
```shell
scp -O -r bin/Release/net8.0/* root@ssh-ha.andrades.cloud:/usr/bin/ts-shara-services
```

## Give scripts permission
```shell
chmod +x /usr/bin/ts-shara-services/TsShara.Services.Application
```


## Run 
```shell
dotnet 
```


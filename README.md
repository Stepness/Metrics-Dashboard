# RemoteLogger

cpuName=$(sysctl -n machdep.cpu.brand_string)
docker run -de "CPU_NAME=$cpuName" logger

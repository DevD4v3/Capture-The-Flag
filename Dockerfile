#
# Build stage/image
#
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app
COPY *.props .

# Copy csproj and restore as distinct layers
COPY ["src/Host/*.csproj", "src/Host/"]
COPY ["src/Host/*.targets", "src/Host/"]
COPY ["src/Application/*.csproj", "src/Application/"]
COPY ["src/Persistence/Persistence.InMemory/*.csproj", "src/Persistence/Persistence.InMemory/"]
COPY ["src/Persistence/Persistence.MariaDB/*.csproj", "src/Persistence/Persistence.MariaDB/"]
COPY ["src/Persistence/Persistence.SQLite/*.csproj", "src/Persistence/Persistence.SQLite/"]
COPY ["src/Persistence/*.props", "src/Persistence/"]

COPY ["external/SampSharp/Directory.Build.props", "external/SampSharp/"]
COPY ["external/SampSharp/Directory.Packages.props", "external/SampSharp/"]
COPY ["external/SampSharp/src/SampSharp.OpenMp.Core/*.csproj", "external/SampSharp/src/SampSharp.OpenMp.Core/"]
COPY ["external/SampSharp/src/SampSharp.OpenMp.Entities/*.csproj", "external/SampSharp/src/SampSharp.OpenMp.Entities/"]
COPY ["external/SampSharp/src/SampSharp.OpenMp.Entities.Commands/*.csproj", "external/SampSharp/src/SampSharp.OpenMp.Entities.Commands/"]
COPY ["external/SampSharp/src/SampSharp.SourceGenerator/*.csproj", "external/SampSharp/src/SampSharp.SourceGenerator/"]
COPY ["external/SampSharp/src/SampSharp.Analyzer/*.csproj", "external/SampSharp/src/SampSharp.Analyzer/"]
COPY ["external/SampSharp/src/SampSharp.CodeFixes/*.csproj", "external/SampSharp/src/SampSharp.CodeFixes/"]

WORKDIR /app/src/Host
RUN dotnet restore

# Copy everything else and build
COPY ["src/", "/app/src/"]
COPY ["external/", "/app/external/"]

RUN dotnet publish -c Release -o /app/out --no-restore

#
# Download open.mp server
#
FROM ubuntu:22.04 AS tools
RUN apt-get update && apt-get install -y --no-install-recommends wget xz-utils
WORKDIR /open-mp

ENV OPENMP_VERSION="1.5.8.3079"
ENV SAMPSHARP_VERSION="2026.1"

RUN wget https://github.com/SampSharp/openmultiplayer-x64-builds/releases/download/v${OPENMP_VERSION}/open.mp-linux-x86_64-dynssl-v${OPENMP_VERSION}.tar.xz --no-check-certificate \
    && tar -xf open.mp-linux-x86_64-dynssl-v${OPENMP_VERSION}.tar.xz \
    && rm -f open.mp-linux-x86_64-dynssl-v${OPENMP_VERSION}.tar.xz

RUN wget https://github.com/DevD4v3/SampSharp/releases/download/v${SAMPSHARP_VERSION}/components-linux.tar.xz --no-check-certificate \
    && tar -xJf components-linux.tar.xz -C Server/components \
    && rm -f components-linux.tar.xz

#
# Final stage/image
#
FROM  mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends \
    openssl \
    libstdc++6 \
    libatomic1 \
    jq \
    tzdata \
    && rm -rf /var/lib/apt/lists/*

COPY --from=tools /open-mp/Server .
COPY --from=build /app/out gamemode
COPY ["gamemodes/*.amx", "gamemodes/"]
COPY ["filterscripts/*.amx", "filterscripts/"]
COPY ["codepages/*.txt", "codepages/"]
COPY ["config.json", "config.json"]
COPY ["entrypoint.sh", "entrypoint.sh"]
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]

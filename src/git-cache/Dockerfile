FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update && apt-get install -y git
RUN build_deps="curl" && \
    apt-get update && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends ${build_deps} ca-certificates && \
    curl -s https://packagecloud.io/install/repositories/github/git-lfs/script.deb.sh | bash && \
    DEBIAN_FRONTEND=noninteractive apt-get install -y --no-install-recommends git-lfs && \
    git lfs install && \
    DEBIAN_FRONTEND=noninteractive apt-get purge -y --auto-remove ${build_deps} && \
    rm -r /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
WORKDIR /src
COPY git-cache.sln ./
COPY git-cache/git-cache.csproj git-cache/
COPY git-cache.mstest/git-cache.mstest.csproj git-cache.mstest/
COPY git-cache.Services/git-cache.Services.csproj git-cache.Services/
COPY git-cache.Services.mstest/git-cache.Services.mstest.csproj git-cache.Services.mstest/
COPY docker-compose.dcproj ./
RUN dotnet restore git-cache.sln -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/git-cache
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "git-cache.dll"]

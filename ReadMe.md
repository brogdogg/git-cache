# Git Cache

## Requirements

### Local Runs

- dotnet core 2.2
- bash

### Docker Runs

- Docker (for Windows)

### Development

- Visual Studio 2017


## Running

Currently the implementation uses bash to execute commands, so it
runs fine on Linux or MacOS. For Windows, it is suggested to run
from a Docker container:

```batch
> docker-compose up
Starting git-cache_git-cache-data_1 ... done
Starting git-cache_git-cache_1      ... done
Attaching to git-cache_git-cache_1, git-cache_git-cache-data_1
git-cache-data_1  | Data container for Git cache
git-cache_git-cache-data_1 exited with code 0
git-cache_1       | Hosting environment: Development
git-cache_1       | Content root path: /app
git-cache_1       | Now listening on: http://[::]:80
git-cache_1       | Application started. Press Ctrl+C to shut down.
```

At this point should be able to clone using the cache:

```batch
> git clone http://localhost:8085/api/Git/github.com/{owner}/{repo}
```

## Configuration

The current default port on the host is `8085`, found in the
main `docker-compose.yml` file.
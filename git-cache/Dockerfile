FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80
RUN apt-get update && apt-get install -y git

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY git-cache.sln ./
COPY git-cache/git-cache.csproj git-cache/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/git-cache
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "git-cache.dll"]

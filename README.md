# AC6 Emblem Browser

This project is being developed to browse AC6 emblems by platform, name, etc.

# Local Setup

## Windows

1. Download [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

2. Install dotnet ef tools by using the following command
   `dotnet tool install dotnet-ef -g`

3. Install [Docker Desktop](https://www.docker.com/products/docker-desktop/)

4. Create a postgres sql container using the following commands in Powershell

   - `docker pull postgres`
   - `docker run -p 5432:5432 --restart unless-stopped --name dev-postgres -v dev-postgres -e POSTGRES_PASSWORD="<pw-here>" -e POSTGRES_INITDB_ARGS="--auth-host=scram-sha-256 --auth-local=scram-sha-256" -d postgres`

5. Download and install [nodejs](https://nodejs.org/en) LTS

6. Run ASP.NET WebAPI project under src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Web

7. Run `npm i` in src/ac6-emblems

8. Run vuejs project using `npm run dev`

## Linux

1. Download [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) if not already installed

2. Install dotnet ef tools by using the following command
   `dotnet tool install dotnet-ef -g`

3. Install [Docker](https://docs.docker.com/desktop/install/linux-install/) for your distro

4. Create a postgres sql container using the following commands in Powershell

   - `docker pull postgres`
   - `docker run -p 5432:5432 --restart unless-stopped --name dev-postgres -v dev-postgres -e POSTGRES_PASSWORD="<pw-here>" -e POSTGRES_INITDB_ARGS="--auth-host=scram-sha-256 --auth-local=scram-sha-256" -d postgres`

5. Download and install [nodejs](https://nodejs.org/en) LTS

6. Run ASP.NET WebAPI project under src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Web

7. Run `npm i` in src/ac6-emblems

8. Run vuejs project using `npm run dev`

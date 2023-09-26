FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#FROM node as vue-build
#WORKDIR /src/ac6-emblems
#RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get install -y nodejs

COPY ["/src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Web/ArmoredCoreSixEmblemBrowser.Web.csproj", "ArmoredCoreSixEmblemBrowser.Web/"]
COPY ["/src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Core/ArmoredCoreSixEmblemBrowser.Core.csproj", "ArmoredCoreSixEmblemBrowser.Core/"]
COPY ["/src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Data/ArmoredCoreSixEmblemBrowser.Data.csproj", "ArmoredCoreSixEmblemBrowser.Data/"]
COPY ["/src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Entities/ArmoredCoreSixEmblemBrowser.Entities.csproj", "ArmoredCoreSixEmblemBrowser.Entities/"]
COPY ["/src/ArmoredCoreSixEmblemBrowser/ArmoredCoreSixEmblemBrowser.Domain/ArmoredCoreSixEmblemBrowser.Domain.csproj", "ArmoredCoreSixEmblemBrowser.Domain/"]

RUN dotnet restore "ArmoredCoreSixEmblemBrowser.Web/ArmoredCoreSixEmblemBrowser.Web.csproj"
COPY /src/ArmoredCoreSixEmblemBrowser .
WORKDIR "/src/ArmoredCoreSixEmblemBrowser.Web"
RUN dotnet build "ArmoredCoreSixEmblemBrowser.Web.csproj" -c Release -o /app/build

WORKDIR /src/ac6-emblems
COPY ["/src/ac6-emblems", "ac6-emblems/"]
RUN npm install
RUN npm run build
COPY ["./dist/**", "ArmoredCoreSixEmblemBrowser.Web/"]

FROM build AS publish
RUN dotnet publish "ArmoredCoreSixEmblemBrowser.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArmoredCoreSixEmblemBrowser.Web.dll"]

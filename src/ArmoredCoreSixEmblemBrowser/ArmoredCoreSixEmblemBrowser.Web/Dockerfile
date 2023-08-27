FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ArmoredCoreSixEmblemBrowser.Web/ArmoredCoreSixEmblemBrowser.Web.csproj", "ArmoredCoreSixEmblemBrowser.Web/"]
RUN dotnet restore "ArmoredCoreSixEmblemBrowser.Web/ArmoredCoreSixEmblemBrowser.Web.csproj"
COPY . .
WORKDIR "/src/ArmoredCoreSixEmblemBrowser.Web"
RUN dotnet build "ArmoredCoreSixEmblemBrowser.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArmoredCoreSixEmblemBrowser.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArmoredCoreSixEmblemBrowser.Web.dll"]

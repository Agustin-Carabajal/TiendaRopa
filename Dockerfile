FROM microsoft.com AS base
WORKDIR /app
EXPOSE 8080

FROM microsoft.com AS build
WORKDIR /src

COPY ["TiendaRopa.Server/TiendaRopa.Server.csproj", "TiendaRopa.Server/"]
COPY ["TiendaRopa.Server.Client/TiendaRopa.Server.Client.csproj", "TiendaRopa.Server.Client/"]
COPY ["TiendaRopa.BD/TiendaRopa.BD.csproj", "TiendaRopa.BD/"]
COPY ["TiendaRopa.Repositorio/TiendaRopa.Repositorio.csproj", "TiendaRopa.Repositorio/"]
COPY ["TiendaRopa.Servicio/TiendaRopa.Servicio.csproj", "TiendaRopa.Servicio/"]
COPY ["TiendaRopa.Shared/TiendaRopa.Shared.csproj", "TiendaRopa.Shared/"]

RUN dotnet restore "TiendaRopa.Server/TiendaRopa.Server.csproj"

COPY . .

WORKDIR "/src/TiendaRopa.Server"
RUN dotnet build "TiendaRopa.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TiendaRopa.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TiendaRopa.Server.dll"]
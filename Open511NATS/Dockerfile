#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Open511NATS/Open511NATS.csproj", "Open511NATS/"]
RUN dotnet restore "Open511NATS/Open511NATS.csproj"
COPY . .
WORKDIR "/src/Open511NATS"
RUN dotnet build "Open511NATS.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Open511NATS.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Open511NATS.dll"]
FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nude.Tg.Bot/Nude.Tg.Bot.csproj", "Nude.Tg.Bot/"]
RUN dotnet restore "Nude.Tg.Bot/Nude.Tg.Bot.csproj"
COPY . .
WORKDIR "/src/Nude.Tg.Bot"
RUN dotnet build "Nude.Tg.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nude.Tg.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nude.Tg.Bot.dll"]

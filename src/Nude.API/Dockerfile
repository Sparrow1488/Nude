FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Nude.API/Nude.API.csproj", "Nude.API/"]
RUN dotnet restore "Nude.API/Nude.API.csproj"
COPY . .
WORKDIR "/src/Nude.API"
RUN dotnet build "Nude.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Nude.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Nude.API.dll"]

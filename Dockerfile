FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["DulceFe.API/DulceFe.API.csproj", "DulceFe.API/"]
RUN dotnet restore "DulceFe.API/DulceFe.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DulceFe.API"
RUN dotnet build "DulceFe.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DulceFe.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# The PORT environment variable is used by Render
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "DulceFe.API.dll"]

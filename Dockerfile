# ========= Build Stage =========
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["CodebridgeTest.API/CodebridgeTest.API.csproj", "CodebridgeTest.API/"]
COPY ["CodebridgeTest.Application/CodebridgeTest.Application.csproj", "CodebridgeTest.Application/"]
COPY ["CodebridgeTest.Core/CodebridgeTest.Core.csproj", "CodebridgeTest.Core/"]
COPY ["CodebridgeTest.Persistence/CodebridgeTest.Persistence.csproj", "CodebridgeTest.Persistence/"]

RUN dotnet restore "CodebridgeTest.API/CodebridgeTest.API.csproj"

COPY . .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR "/src/CodebridgeTest.API"
RUN dotnet publish "CodebridgeTest.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ========= Runtime Stage =========
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Create non-root app user
RUN addgroup --system appgroup && adduser --system appuser --ingroup appgroup

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

USER appuser

ENTRYPOINT ["dotnet", "CodebridgeTest.API.dll"]

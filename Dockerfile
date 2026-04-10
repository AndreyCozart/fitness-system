# ── Build stage ──────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Restore зависимостей
COPY FitnessSystem/FitnessSystem.csproj FitnessSystem/
RUN dotnet restore FitnessSystem/FitnessSystem.csproj

# Копируем весь код и собираем
COPY . .
RUN dotnet publish FitnessSystem/FitnessSystem.csproj -c Release -o /app/publish

# ── Runtime stage ─────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Railway передаёт PORT — слушаем его
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

ENTRYPOINT ["dotnet", "FitnessSystem.dll"]

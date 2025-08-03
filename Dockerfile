# ---- Etapa 1: Compilación (Build) ----
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiar archivos de proyecto (.csproj) y solución (.sln) primero
COPY *.sln .
COPY src/Domain/*.csproj src/Domain/
COPY src/Application/*.csproj src/Application/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY src/Presentation/*.csproj src/Presentation/
COPY MockApi/*.csproj MockApi/
COPY tests/*.csproj tests/
RUN dotnet restore

# Copiar el resto del código fuente
COPY . .

# Publicar la aplicación en modo Release
WORKDIR /app/src/Presentation
RUN dotnet publish -c Release -o /app/publish


# ---- Etapa 2: Ejecución (Final) ----
# Usamos la imagen de "runtime" de .NET 9.0
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS final
WORKDIR /app

# Copiar solo la aplicación compilada
COPY --from=build /app/publish .

# Definir el comando que se ejecutará
ENTRYPOINT ["dotnet", "Presentation.dll"]
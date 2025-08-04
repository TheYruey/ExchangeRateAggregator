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
# Usar la imagen optimizada para ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Exponer el puerto 8080 del contenedor
EXPOSE 8080

# Definir el comando que se ejecutará cuando el contenedor inicie
ENTRYPOINT ["dotnet", "Presentation.dll"]
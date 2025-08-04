## Agregador de Tasas de Cambio (Reto Técnico)

Este proyecto es una solución al reto técnico Comparing Exchange Rate Offers for Banking Clients, cuyo objetivo es desarrollar un servicio de backend que consulta múltiples APIs de tasas de cambio para encontrar y devolver la mejor oferta para un cliente.

El sistema está diseñado como una **API Web RESTful** robusta, eficiente y escalable, siguiendo las mejores prácticas de la industria como la Arquitectura Limpia y los principios SOLID.

## Características Principales

-   **API RESTful**: Expone un endpoint `GET /api/exchange/best-offer` para obtener la mejor tasa de cambio.
    
-   **Documentación Interactiva con Swagger**: Incluye una interfaz de usuario de Swagger para una fácil visualización y prueba de los endpoints.
    
-   **Arquitectura Limpia**: El proyecto está estructurado en capas y carpetas (`Domain/Entities`, `Application/Services`, `Infrastructure/Providers`, `Presentation/Controllers`) para una clara separación de responsabilidades y alta mantenibilidad.
    
-   **Procesamiento Asíncrono**: Utiliza `async/await` y `Task.WhenAll` para consultar todas las APIs de forma concurrente, asegurando una respuesta en el menor tiempo posible.
    
-   **Tolerancia a Fallos**: El sistema es resiliente y seguirá funcionando para seleccionar la mejor oferta incluso si una o más de las APIs consultadas no están disponibles.
    
-   **Cobertura de Pruebas Unitarias**: Incluye un conjunto de pruebas con `xUnit` y `Moq` para garantizar la calidad de la lógica de negocio y de los adaptadores de infraestructura.
    
-   **Containerización**: El proyecto está completamente containerizado con **Docker**, permitiendo una configuración y ejecución consistentes en cualquier entorno.
    

## Tecnologías Utilizadas

-   **Lenguaje**: C#
    
-   **Framework**: ASP.NET Core 9.0
    
-   **Pruebas**: xUnit y Moq
    
-   **Containerización**: Docker
    

## Prerrequisitos

Asegúrate de tener instalado lo siguiente:

-   .NET 9.0 SDK
    
-   Docker Desktop
    
-   Git
    

## Cómo Empezar

1.  Clona el repositorio en tu máquina local:
    
    ```
    git clone https://github.com/TheYruey/ExchangeRateAggregator.git
    cd ExchangeRateAggregator
    
    ```
    
2.  Restaura todas las dependencias del proyecto:
    
    ```
    dotnet restore
    
    ```
    

## Ejecución

Puedes ejecutar el proyecto de tres maneras diferentes:

### 1. Pruebas Unitarias

Para verificar que toda la lógica de negocio y los componentes de infraestructura funcionan correctamente, ejecuta el siguiente comando desde la raíz del proyecto:

```
dotnet test

```

### 2. Ejecución Local

Para correr la API completa localmente, necesitarás dos terminales.

-   **Terminal 1: Iniciar el Servidor Simulado (`MockApi`)** Desde la raíz del proyecto, ejecuta:
    
    ```
    dotnet run --project MockApi
    
    ```
    
    (Anota el puerto en el que se está ejecutando, por ejemplo: `http://localhost:5231`)
    
-   **Terminal 2: Iniciar la Web API (`Presentation`)** Desde la raíz del proyecto, ejecuta:
    
    ```
    dotnet run --project src/Presentation
    
    ```
    
-   **Probar la API:** Abre tu navegador y ve a la dirección de Swagger que aparece en la consola (ej. `http://localhost:5031/swagger`). Desde ahí podrás probar el endpoint `GET /api/exchange/best-offer`.
    

### 3. Ejecución con Docker

Esta es la forma más sencilla de ejecutar el proyecto, ya que abstrae las dependencias.

-   **Paso Previo (Importante):** Para que el contenedor pueda comunicarse con la `MockApi` que corre en tu PC, asegúrate de que la `baseUrl` en `src/Presentation/Program.cs` apunte a `http://host.docker.internal:<puerto>/`.
    
-   **Terminal 1: Iniciar el Servidor Simulado (`MockApi`)**
    
    ```
    dotnet run --project MockApi
    
    ```
    
-   **Terminal 2: Construir y Ejecutar el Contenedor** Desde la raíz del proyecto:
    
    ```
    # 1. Construir la imagen de Docker
    docker build -t exchange-aggregator .
    
    # 2. Ejecutar la aplicación desde el contenedor
    # El flag '-e' es crucial para activar Swagger dentro del contenedor
    docker run --rm -p 8081:8080 -e ASPNETCORE_ENVIRONMENT=Development exchange-aggregator
    
    ```
    
-   **Probar la API:** Abre tu navegador y ve a `http://localhost:8081/swagger` para interactuar con la API que corre dentro del contenedor.
    

## Estructura del Proyecto

El proyecto sigue los principios de Arquitectura Limpia con una estructura granular:

-   **`Domain`**: Contiene las entidades (`Entities`) y las abstracciones/interfaces (`Interfaces`).
    
-   **`Application`**: Orquesta los casos de uso con sus clases de servicio (`Services`).
    
-   **`Infrastructure`**: Implementa servicios externos, como los proveedores de API (`Providers`).
    
-   **`Presentation`**: Expone la aplicación


# Agregador de Tasas de Cambio

Este proyecto es una solución al reto técnico Comparing Exchange Rate Offers for Banking Clients, cuyo objetivo es desarrollar un servicio de backend que consulta múltiples APIs de tasas de cambio para encontrar y devolver la mejor oferta para un cliente.

El sistema está diseñado para ser robusto, eficiente y escalable, siguiendo las mejores prácticas de la industria como la Arquitectura Limpia y los principios SOLID.

## Características Principales

-   **Arquitectura Limpia**: El proyecto está estructurado en capas (`Domain`, `Application`, `Infrastructure`, `Presentation`) para una clara separación de responsabilidades y mantenibilidad.
    
-   **Procesamiento Asíncrono**: Utiliza `async/await` y `Task.WhenAll` para consultar todas las APIs de forma concurrente, asegurando una respuesta en el menor tiempo posible.
    
-   **Tolerancia a Fallos**: El sistema es resiliente y seguirá funcionando para seleccionar la mejor oferta incluso si una o más de las APIs consultadas no están disponibles o devuelven errores.
    
-   **Inyección de Dependencias**: Configurado para desacoplar los componentes, facilitando las pruebas y la extensibilidad.
    
-   **Pruebas Unitarias**: Incluye un conjunto de pruebas unitarias con `xUnit` y `Moq` para garantizar la calidad y el correcto funcionamiento de la lógica de negocio.
    
-   **Containerización**: El proyecto está completamente containerizado con **Docker**, permitiendo una fácil configuración y ejecución en cualquier entorno.
    

## Tecnologías Utilizadas

-   **Lenguaje**: C#
    
-   **Framework**: .NET 9.0
    
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
    git clone [https://github.com/TheYruey/ExchangeRateAggregator.git](https://github.com/TheYruey/ExchangeRateAggregator.git)
    cd ExchangeRateAggregator
    
    ```
    
2.  Restaura todas las dependencias del proyecto:
    
    ```
    dotnet restore
    
    ```
    

## Ejecución

Puedes ejecutar el proyecto de tres maneras diferentes:

### 1. Pruebas Unitarias

Para verificar que toda la lógica de negocio funciona correctamente, ejecuta el siguiente comando desde la raíz del proyecto:

```
dotnet test

```

### 2. Ejecución Local

Para correr la aplicación completa localmente, necesitarás dos terminales.

-   **Terminal 1: Iniciar el Servidor Simulado (`MockApi`)**
    
    ```
    dotnet run --project MockApi
    
    ```
    
    (Anota el puerto en el que se está ejecutando, por ejemplo: `http://localhost:5231`)
    
-   **Terminal** 2: Iniciar la **Aplicación Principal (`Presentation`)** Asegúrate de que la `baseUrl` en `src/Presentation/Program.cs` coincida con el puerto del servidor simulado.
    
    ```
    dotnet run --project src/Presentation
    
    ```
    

### 3. Ejecución con Docker

Esta es la forma más sencilla de ejecutar el proyecto, ya que abstrae las dependencias.

-   **Paso Previo**: Asegúrate de que la `baseUrl` en `src/Presentation/Program.cs` apunte a `http://host.docker.internal:<puerto>/` para que el contenedor pueda comunicarse con la `MockApi` que corre en tu PC.
    
-   **Terminal 1: Iniciar el Servidor Simulado (`MockApi`)**
    
    ```
    dotnet run --project MockApi
    
    ```
    
-   **Terminal 2: Construir y Ejecutar el Contenedor** Desde la raíz del proyecto:
    
    ```
    # 1. Construir la imagen de Docker
    docker build -t exchange-aggregator .
    
    # 2. Ejecutar la aplicación desde el contenedor
    docker run --rm exchange-aggregator
    
    ```
    

## Estructura del Proyecto

El proyecto sigue los principios de Arquitectura Limpia con las siguientes capas:

-   **`Domain`**: Contiene las entidades y reglas de negocio más puras. No tiene dependencias externas.
    
-   **`Application`**: Orquesta los casos de uso. Contiene la lógica de la aplicación y depende de `Domain`.
    
-   **`Infrastructure`**: Contiene las implementaciones de servicios externos, como los clientes para las APIs. Depende de `Application`.
    
-   **`Presentation`**: Es el punto de entrada de la aplicación. Depende de `Infrastructure`.
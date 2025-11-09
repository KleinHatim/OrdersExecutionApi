# OrdersExecutionApi

## Description

`OrdersExecutionApi` est une application ASP.NET Core 8 qui expose une API REST pour exécuter des ordres de trading. L'API permet de soumettre des ordres d'achat ou de vente et retourne les trades correspondants.


## Prérequis

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/fr/downloads/) ou une autre IDE compatible .NET

## Installation

1. **Cloner le Repository**

    ```bash
    git clone https://github.com/KleinHatim/OrdersExecutionApi.git
    cd OrdersExecutionApi
    ```

2. **Restaurer les Packages NuGet**

    ```bash
    dotnet restore
    ```

3. **Build du Projet**

    ```bash
    dotnet build
    ```

## Configuration

Le projet est configuré pour utiliser .NET 8.0 et inclut les packages nécessaires pour fonctionner correctement.

### Swagger

Swagger est utilisé pour documenter et tester l'API. Il est disponible en mode développement.

- **URL** : `https://localhost:<port>/swagger`

## Exécution de l'Application

1. **Démarrer l'Application**

    ```bash
    dotnet run
    ```

2. **Accéder à l'API**

    - **Swagger UI** : `https://localhost:<port>/swagger`
    - **Endpoint d'Exécution des Ordres** : `POST /api/orders/execute`

## Tests Unitaires

Les tests unitaires sont écrits avec xUnit et Moq pour tester la logique métier.

1. **Exécuter les Tests Unitaires**

    ```bash
    dotnet test
    ```
    <img width="1234" height="1292" alt="image" src="https://github.com/user-attachments/assets/0f727407-95ac-4ebe-893d-5959ac8454b8" />


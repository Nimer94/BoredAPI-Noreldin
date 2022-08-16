# BoredAPI-Noreldin

## Introduction
This is a .NET solution for Bored API assignment that containts several projects.

## Architecture
This solution has two projects:

### *BoredApi.Service*
Contains the following structure:

1. Repositories: `IActibityTableRepository` is implemented to perform certain operations into Azure Table.
1. Services: `IBoredAPIManagerService` is a layer implemented to restrict using Repository inside REST controller, so the access of reository will be only permitted inside implementation of this service.
1. Extension classes: Some extension methods needed to integrate Swagger with the project to have Open API documentation for rest endpoints.

### *BoredApi.Service.UnitTest*
Have Unit test implementation to test implemented functionalities.

## How to build?

You can build the project with two ways, first using any IDE supports .NET Core like Microsoft Visual Studio or Rider, the second way using `dotnet` command by navigating to home folder of solution:

```powershell
PM> dotnet build
```

How to run?

Same as what is written in build, using either IDE or dotnet command by:

First nvigate to ./BoredApi.Service/ folder then install related function packages.

```powershell
PM> npm install azure-functions-core-tools -g
```

After packags are installed successfully, run the run command:

```powershell
PM> func start
```
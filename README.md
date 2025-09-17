# Tutorial de Instalación

## Requisitos Previos

- Docker
- .NET 9 SDK
- Node.js y npm

## 1. Iniciar la Base de Datos

Ejecuta el siguiente comando en la raíz del proyecto para levantar la base de datos con Docker:

```powershell
docker compose up
```

## 2. Inicializar la Base de Datos

Ejecuta el script de procedimientos almacenados en la base de datos creada:

- Conéctate a `localhost,1433` usando:
  - Usuario: `sa`
  - Contraseña: `YourStrong!Passw0rd`
- Ejecuta el script ubicado en:

```
src/db/scripts/init-procedures-sqlserver.sql
```

## 3. Iniciar el Backend (.NET 9)

```powershell
cd src/api
dotnet run
```

## 4. Iniciar el Frontend (Node.js)

```powershell
cd src/frontend
npm install
npm run dev
```

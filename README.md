# WorldCupPlanner

Aplicación web para administrar y simular un campeonato mundial de fútbol: equipos, estadios, usuarios, fase de grupos y eliminatorias en un único sistema.

> Proyecto académico colaborativo desarrollado por un equipo de tres integrantes. Se construyó con un flujo de programación tradicional y prácticas orientadas a producción: arquitectura por capas, pruebas automatizadas, persistencia relacional y trabajo mediante ramas y pull requests.

## Qué permite hacer

- Administrar equipos, estadios y usuarios.
- Generar fixtures de fase de grupos de forma reproducible mediante una semilla.
- Calcular clasificaciones y cruces de octavos, cuartos, semifinales, tercer puesto y final.
- Simular partidos de manera individual o masiva.
- Elegir entre un motor aleatorio puro y otro probabilístico basado en ranking FIFA.
- Importar equipos desde archivos estructurados.
- Exportar fixtures y auditorías a CSV y Excel.
- Registrar acciones críticas mediante auditorías.
- Gestionar notificaciones y permisos por rol: Administrador, Editor y Periodista.

## Tecnologías

- .NET 8 y ASP.NET Core Blazor Server
- Entity Framework Core 8
- SQL Server / Azure SQL Edge
- MSTest y Coverlet
- ClosedXML
- Docker y Docker Compose

## Arquitectura

La solución está separada en capas para mantener la lógica de negocio desacoplada de la persistencia y la interfaz:

```text
WorldCupPlanner/
├── Dominio/           # Entidades y reglas centrales
├── Repositorio/       # EF Core, repositorios y migraciones
├── Servicios/         # Casos de uso, DTO, simulación e importación/exportación
├── UI/                # Aplicación Blazor Server
├── TestDominio/       # Pruebas de dominio
├── TestRepositorio/   # Pruebas de persistencia
└── TestServicios/     # Pruebas de servicios
```

La suite contiene más de 200 pruebas automatizadas distribuidas entre dominio, servicios y repositorios.

## Mi contribución

Dentro del equipo de tres integrantes, **Nacho Bordagorry** trabajó principalmente en:

- Diseño e implementación de las capas de dominio y servicios.
- Persistencia, configuración de Entity Framework Core y trabajo con la base de datos.
- Pruebas automatizadas y validación de reglas de negocio.
- Integración transversal con componentes de UI, correcciones y refactorizaciones.

Aunque existió una división principal de responsabilidades, los tres integrantes colaboraron en distintas capas del sistema.

## Ejecución local

### Requisitos

- SDK de .NET 8
- Docker Desktop
- Visual Studio 2022, Rider o VS Code con C# Dev Kit

### 1. Configurar el entorno

Desde la carpeta `WorldCupPlanner`, cree el archivo local `.env` a partir del ejemplo:

```bash
cp .env.example .env
```

Cambie el valor de `DB_PASSWORD` por una contraseña local segura. Luego configure la cadena de conexión sin guardarla en Git:

```bash
# Linux/macOS
export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=WorldCupPlannerDB;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=true;"
```

En PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=WorldCupPlannerDB;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=true;"
```

### 2. Iniciar SQL Server

```bash
docker compose up -d
```

### 3. Restaurar, probar y ejecutar

```bash
dotnet restore WorldCupPlanner.sln
dotnet test WorldCupPlanner.sln
dotnet run --project UI/UI.csproj
```

Las migraciones se aplican al iniciar la aplicación. La URL exacta se muestra en la terminal.

## Credenciales de demostración

El proyecto puede sembrar un administrador exclusivamente para desarrollo local si se configuran estas variables antes de ejecutar la aplicación:

```bash
export DemoAdmin__Email="demo@example.local"
export DemoAdmin__Password="ELIJA_UNA_PASSWORD_LOCAL"
```

En PowerShell:

```powershell
$env:DemoAdmin__Email="demo@example.local"
$env:DemoAdmin__Password="ELIJA_UNA_PASSWORD_LOCAL"
```

Si no se configuran ambas variables, la aplicación no crea ni muestra credenciales de demostración. No reutilice estos valores en un entorno público o productivo.

## Estado

Proyecto académico finalizado y conservado como muestra de arquitectura, testing, persistencia y trabajo colaborativo. No es un servicio en producción.

# WorldCupPlanner

## 1. Breve descripción de la solución

**WorldCupPlanner** es una aplicación web interactiva diseñada para la planificación, administración y simulación de un campeonato mundial de fútbol. Su lógica de negocio gira en torno a la gestión integral de un torneo, permitiendo administrar equipos, estadios, usuarios y los partidos que componen tanto la fase de grupos como las etapas de eliminación directa.

El sistema resuelve la problemática de automatizar la conformación de fixtures complejos y calcular dinámicamente las clasificaciones y cruces. A través de una semilla aleatoria de generación, el sistema distribuye los partidos de la primera fase (fase de grupos) de manera balanceada. Luego, provee motores de simulación (aleatorios y probabilísticos basados en el ranking FIFA) para jugar los partidos de forma individual o masiva. A medida que se obtienen o simulan los resultados de cada grupo, el sistema determina los equipos que avanzan a la segunda fase y genera dinámicamente los emparejamientos de octavos, cuartos, semifinales, tercer puesto y la gran final.

Adicionalmente, el proyecto incluye un robusto sistema de auditoría que registra de forma inalterable las acciones críticas del sistema, mecanismos de exportación de fixtures y auditorías a archivos CSV y hojas de cálculo de Excel (XLSX), un importador masivo de equipos a partir de archivos estructurados, y un módulo de notificaciones para comunicar eventos relevantes del torneo. Todo el sistema está protegido por un esquema de autenticación y autorización basado en roles específicos (Administrador, Editor, Periodista).

---

## 2. Tecnologías utilizadas

El proyecto está desarrollado utilizando tecnologías modernas y consolidadas del ecosistema de Microsoft .NET:

- **Backend / Web Framework**:
  - **.NET 8.0**: Plataforma principal de ejecución.
  - **ASP.NET Core Blazor (Server Interactive Mode)**: Framework para la construcción de la interfaz de usuario interactiva y dinámica desde el lado del servidor.
- **Acceso a Datos y Persistencia**:
  - **Entity Framework Core 8.0.27**: ORM para la interacción con la base de datos y la administración de migraciones.
  - **Microsoft.EntityFrameworkCore.SqlServer**: Proveedor de EF Core para la integración con Microsoft SQL Server.
  - **Microsoft.EntityFrameworkCore.InMemory**: Proveedor en memoria utilizado para la ejecución rápida y aislada de las pruebas unitarias.
- **Base de Datos**:
  - **Microsoft SQL Server / Azure SQL Edge**: Motor de base de datos relacional para el almacenamiento persistente de la información.
- **Herramientas y Librerías de Terceros**:
  - **ClosedXML (0.105.0)**: Biblioteca utilizada para la generación y exportación dinámica de archivos en formato Microsoft Excel (XLSX).
  - **Docker & Docker Compose**: Para la containerización y fácil despliegue de la instancia local de la base de datos SQL Server.
- **Herramientas de Desarrollo y Testing**:
  - **MSTest**: Framework oficial para el diseño y ejecución de pruebas unitarias.
  - **Coverlet.Collector**: Herramienta de recolección de cobertura de código para los tests.

---

## 3. Estructura de la solución

La aplicación sigue una arquitectura limpia estructurada en capas bien definidas, lo que promueve el desacoplamiento y facilita la mantenibilidad y testeabilidad:

```
WorldCupPlanner/
├── Dominio/               # Capa de Dominio (Entidades de negocio y Enums)
├── Repositorio/           # Capa de Persistencia (Contexto EF Core, Repositorios SQL y Migraciones)
├── Servicios/             # Capa de Lógica de Negocio (Servicios, DTOs, Motores de Simulación, Importación/Exportación)
├── UI/                    # Capa de Presentación (Aplicación Blazor Server, Páginas, Componentes y Assets)
├── TestDominio/           # Proyecto de pruebas unitarias para la capa de Dominio
├── TestRepositorio/       # Proyecto de pruebas unitarias para la capa de Persistencia
├── TestServicios/         # Proyecto de pruebas unitarias para la capa de Servicios
├── docker-compose.yml     # Orquestador para levantar localmente el contenedor de base de datos SQL Server
├── global.json            # Configuración y versión del SDK de .NET
└── WorldCupPlanner.sln    # Archivo de solución que engloba todos los proyectos
```

### Detalle de las Capas:
- **Dominio**: Contiene las entidades puras del negocio (`Equipo`, `Estadio`, `Partido`, `Usuario`, `Auditoria`, `Notificacion`) y las enumeraciones asociadas (`Fase`, `Rol`, `Confederacion`, `EstadoPartido`, `TipoIncidencia`). No posee dependencias externas ni de persistencia.
- **Repositorio**: Se encarga de la persistencia de datos. Define el contexto de base de datos (`SqlContexto`) y los repositorios concretos utilizando EF Core. Alberga los scripts autogenerados de Migración para el control de versiones de la base de datos.
- **Servicios**: Es el núcleo lógico de la aplicación. Convierte entidades a DTOs (`*DTO`), aplica reglas de validación de negocio, implementa la generación automática del fixture, los motores de simulación (`MotorProbabilistico` y `MotorAleatorioPuro`), y gestiona exportaciones mediante ClosedXML.
- **UI**: Capa cliente/servidor construida con Blazor. Configura la inyección de dependencias en `Program.cs`, consume los servicios del negocio y define los componentes visuales (`.razor`) estructurados con estilos propios para la administración del campeonato.

---

## 4. Instrucciones para su ejecución

Siga los pasos descritos a continuación para configurar y ejecutar la solución en su entorno de desarrollo local desde cero.

### Requisitos Previos
Asegúrese de tener instalados los siguientes componentes en su sistema:
- **SDK de .NET 8.0** (o posterior).
- **Docker Desktop** (para inicializar el motor de base de datos).
- Un IDE compatible como **Visual Studio 2022**, **JetBrains Rider**, o **VS Code** (con la extensión C# Dev Kit).

### Paso 1: Levantar la Base de Datos con Docker
El proyecto cuenta con un archivo `docker-compose.yml` que levanta una instancia liviana de SQL Server (Azure SQL Edge).
Abra una terminal en la raíz del proyecto y ejecute:
```bash
docker-compose up -d
```
*Esto iniciará un contenedor de SQL Server expuesto en el puerto local `1433` con la contraseña configurada (`Passw1rd`).*

### Paso 2: Restaurar Dependencias e Iniciar la Solución
Desde la raíz del proyecto, ejecute los siguientes comandos en su terminal:
1. **Restaurar paquetes NuGet**:
   ```bash
   dotnet restore
   ```
2. **Compilar la solución**:
   ```bash
   dotnet build
   ```
3. **Ejecutar las pruebas unitarias (Opcional)**:
   ```bash
   dotnet test
   ```
   *Esto comprobará el correcto funcionamiento de los 225 tests unitarios integrados.*

### Paso 3: Configuración de Variables y Cadena de Conexión
La configuración de acceso a datos se encuentra predeterminada en el archivo `UI/appsettings.json` en la raíz del proyecto `UI`.

```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=WorldCupPlannerDB;User Id=sa;Password=Passw1rd;TrustServerCertificate=true;"
  }
```
*Nota: Las migraciones de base de datos se ejecutan automáticamente en el arranque de la aplicación a través de la instrucción `Database.Migrate()` en el contexto, por lo que no es requerido ejecutar comandos adicionales de migración.*

### Paso 4: Compilar y Ejecutar la Aplicación
- **Desde la Terminal**: Navegue al directorio del proyecto `UI` y ejecute la aplicación:
  ```bash
  cd UI
  dotnet run
  ```
- **Desde el IDE**: abra la solución `WorldCupPlanner.sln`, establezca el proyecto **UI** como proyecto de inicio (Startup Project) y presione `F5` o el botón de ejecución de su IDE.

### Acceso a la Aplicación
Una vez que el servidor de desarrollo esté activo:
1. Abra su navegador e ingrese a la dirección: **`http://localhost:5000`** (o **`https://localhost:7204`**).
2. Para acceder con privilegios de administrador/editor y configurar el torneo, use el usuario de prueba preconfigurado:
   - **Email**: `admin@admin.com`
   - **Contraseña**: `Admin123!`

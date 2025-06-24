# VerticalSliceArchitecture

Este proyecto es una solución de ejemplo que implementa el patrón **Vertical Slice Architecture** utilizando **.NET 9**, **ASP.NET Core Minimal APIs**, **Entity Framework Core** y **PostgreSQL**. Está preparado tanto para desarrollo local como para despliegue mediante contenedores **Docker**.

---

## 🧱 Estructura del Proyecto

```
VerticalSliceArchitecture/
├── Web.Api/                      # Proyecto principal de la API
│   ├── Features/                # Slices verticales (ej. Customers)
│   ├── Database/                # Contexto y migraciones de EF Core
│   ├── Entities/                # Entidades del dominio
│   ├── Extensions/              # Métodos de extensión para configuración
│   ├── Middlewares/             # Middlewares y manejadores de excepciones
│   ├── Program.cs               # Punto de entrada principal
│   ├── DependencyInjection.cs   # Configuración de servicios
│   └── appsettings*.json        # Configuraciones por entorno
├── docker-compose.yml           # Orquestación de contenedores
├── docker-compose.override.yml  # Configuración adicional para desarrollo
├── docker-compose.dcproj        # Proyecto Docker Compose (Visual Studio)
├── Directory.Build.props        # Configuración global de compilación
├── Directory.Packages.props     # Versionado centralizado de NuGet
├── VerticalSliceArchitecture.sln # Solución de Visual Studio
└── .containers/pgdata/          # Persistencia de PostgreSQL
```

---

## 🚀 Tecnologías Utilizadas

- [.NET 9](https://dotnet.microsoft.com/)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/)
- [PostgreSQL](https://www.postgresql.org/)
- [FluentValidation](https://docs.fluentvalidation.net/)
- [Docker + Docker Compose](https://www.docker.com/)
- [Swagger/OpenAPI](https://swagger.io/)

---

## 🧩 Arquitectura Vertical Slice

Cada slice representa una funcionalidad completa e independiente. En lugar de organizar el código por capas (Controllers, Services, etc.), cada slice contiene:

- Endpoint(s)
- Validaciones
- Lógica de negocio

Ejemplo de estructura en `Features/Customers`:

```
├── CreateCustomer.cs
├── GetCustomer.cs
```

---

## 🛠 Configuración de Base de Datos

La conexión a PostgreSQL se encuentra en `Web.Api/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "Database": "Host=local.postgres;Database=storedb;Username=postgres;Password=postgres"
  }
}
```

> ⚠️ No uses credenciales en texto plano en producción. Utiliza secretos o variables de entorno.

Las migraciones de EF Core se aplican automáticamente en el entorno de desarrollo.

---

## ▶️ Ejecución Local

### Requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)

### Opción 1: Ejecutar todo con Docker

```bash
docker compose up --build
```

Esto levantará:

- La API en http://localhost:5000 y https://localhost:5001
- PostgreSQL en localhost:5432

### Opción 2: Ejecutar solo la API (sin Docker)

1. Asegúrate de tener PostgreSQL corriendo localmente (puedes usar el contenedor `local.postgres`).
2. Ejecuta la API:

```bash
dotnet run --project Web.Api/Web.Api.csproj
```

---

## 🔗 Endpoints de Ejemplo

- `POST /customers`
- `GET /customers/{customerId}`

Consulta la documentación Swagger en:  
➡️ http://localhost:5000/swagger

---

## ⚙️ Desarrollo

- Las **migraciones** se aplican automáticamente en modo desarrollo.
- Las validaciones se manejan con **FluentValidation**, devolviendo errores en formato `ProblemDetails`.
- Es fácil agregar nuevas features como slices independientes, manteniendo bajo acoplamiento.

---

## 🧪 Scripts Útiles

### Crear nueva migración

```bash
dotnet ef migrations add NombreMigracion --project Web.Api
```

### Aplicar migraciones manualmente

```bash
dotnet ef database update --project Web.Api
```

---

## 📄 Licencia

Este proyecto es solo para fines educativos y de demostración.  
No está destinado para uso en producción sin antes realizar las debidas adaptaciones de seguridad y escalabilidad.

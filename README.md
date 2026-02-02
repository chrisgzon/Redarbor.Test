# RedArbor Employee API

API RESTful para gestión de empleados construida con .NET 8, siguiendo principios SOLID, DDD y patrón CQRS.

## 🏗️ Arquitectura

- **Clean Architecture** con separación de capas
- **CQRS**: Entity Framework para escrituras, Dapper para lecturas
- **DDD**: Domain-Driven Design con Entities, Value Objects y Aggregates
- **SOLID** principles
- **OAuth2/JWT** para autenticación

## 📝 Endpoints

### Autenticación

- **POST** `/api/auth/login` - Iniciar sesión
- **POST** `/api/auth/refresh` - Refrescar token
- **GET** `/api/auth/me` - Obtener usuario actual

### Employees

- **GET** `/api/redarbor` - Obtener todos los empleados
- **GET** `/api/redarbor/{id}` - Obtener empleado por ID
- **POST** `/api/redarbor` - Crear nuevo empleado
- **PUT** `/api/redarbor/{id}` - Actualizar empleado
- **DELETE** `/api/redarbor/{id}` - Eliminar empleado

## 📦 Paquetes Principales

- MediatR - CQRS implementation
- FluentValidation - Validación de modelos
- AutoMapper - Object mapping
- Entity Framework Core - ORM
- Dapper - Micro ORM
- JWT Bearer - Autenticación

## 🔒 Seguridad

- JWT tokens con expiración
- Validación de inputs con FluentValidation
- CORS configurado

## 🚀 Inicio Rápido

### Prerrequisitos

- .NET 8 SDK
- SQL Server
- Docker (opcional)

### Ejecución Local

```bash
# 1. Clonar el repositorio
git clone https://github.com/chrisgzon/Redarbor.Test.git

# 2. Restaurar paquetes
dotnet restore

# 3. Actualizar connection string en appsettings.json

# 4. Crear base de datos
# ejecutar el script init.sql

# 5. Ejecutar la API
dotnet run --project src/Redarbor.Test.API
```

### Ejecución con Docker

```bash
# Construir y ejecutar
cd Redarbor.Test.Api
dotnet publish -c Release
docker-compose up --build
```

---

### ✅ Servicios disponibles

API:
`http://localhost:7262/swagger`

SQL Server:
`localhost,1434`
User: `sa`
Password: `P4ssw0rd*`
Database: `redarbor`

---

### 📌 Notes

- La conexion a la base de datos esta configurada en las variables de entorno en Docker.

---

### Ejecutar `init.sql` script

Propiedades de conexion para ejecutar `init.sql`:

- Host: `localhost`
- Port: `1434`
- User: `sa`
- Password: `P4ssw0rd*`
- Database: `redarbor`
- Script: `init.sql`
- TrustServerCertificate: `true`,
- Encrypt: `false`

---

## 🧪 Testing

```bash
# Ejecutar tests unitarios
dotnet test Redarbor.Test.UnitTests

# Ejecutar tests de integración
dotnet test IntegrationTests
```

Despues de completar los pasos anteriores, y si nada fallo, Podras probar y ejecutar los endpoints del api.

### Datos de Prueba

La base de datos incluye datos de ejemplo:

**Usuarios para Login:**
- Admin: `username: admin` / `password: admin123` -- permisos de escritura en entidad de empleados
- Guest: `username: guest` / `password: guest123`

## Archivo example.json para las pruebas

```json
{
  "CompanyId": 1,
  "CreatedOn": "2000-01-01T00:00:00",
  "DeletedOn": "2000-01-01T00:00:00",
  "Email": "test1@test.test.tmp",
  "Fax": "000.000.000",
  "Name": "test1",
  "Lastlogin": "2000-01-01T00:00:00",
  "Password": "test",
  "PortalId": 1,
  "RoleId": 1,
  "StatusId": 1,
  "Telephone": "000.000.000",
  "UpdatedOn": "2000-01-01T00:00:00",
  "Username": "test1"
}

# ERP UNAJ – Sistema de Gestión Financiera y Contable

Sistema ERP modular desarrollado para la **Universidad Nacional de Juliaca (UNAJ)** como proyecto del curso **Taller de Procesos ERP 2026**.

---

## Equipo de desarrollo

| Rol | Nombre |
|---|---|
| Sprint / Analista de Requisitos | Alvis Danival Quispe Huayta |
| Backend Developer | Elias Ronald Ticona Callata |
| Frontend Developer (WinForms) | Luis Fernando Cutiri Vilca |
| Arquitecto y Admin de Base de Datos | Edison David Supo Cruz |

---

## Stack tecnológico

| Capa | Tecnología |
|---|---|
| Backend API | C# ASP.NET Core 8 Web API + Entity Framework Core |
| Frontend | Windows Forms .NET 8 |
| Base de datos | PostgreSQL 16 (Docker) + PL/pgSQL stored procedures |
| Autenticación | JWT Bearer (access token 1 h + refresh token 7 d) |
| PDF | QuestPDF |
| Excel | ClosedXML |
| Logs | Serilog → consola + archivo rolling daily |
| Tests | xUnit + FluentAssertions + Moq |
| Contenedores | Docker Compose (postgres + adminer) |

---

## Requisitos previos

Antes de ejecutar el proyecto instala lo siguiente:

| Herramienta | Versión mínima |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0 o superior |
| [Docker Desktop](https://www.docker.com/products/docker-desktop) | 4.x |
| Git | cualquier versión reciente |

> **Windows:** asegúrate de que Docker Desktop esté corriendo antes de continuar.
> **WinForms** solo funciona en Windows.

---

## Estructura del proyecto

```
ERP/
├── docker-compose.yml          # PostgreSQL 16 + Adminer
├── .env                        # Variables de entorno (NO se sube a git — créalo tú)
├── sql/
│   └── init.sql                # Esquema 3FN + stored procedures + seed data
├── ERP.slnx
├── ERP.Api/                    # ASP.NET Core Web API
│   ├── Controllers/            # Auth, Empleados, Planillas, Comprobantes, Libros, Reportes
│   ├── Services/               # Lógica de negocio
│   ├── Repositories/           # Acceso a datos
│   ├── Models/                 # Entidades EF Core
│   ├── Data/                   # ErpDbContext
│   ├── Helpers/                # JwtHelper
│   └── Middleware/             # ExceptionMiddleware
├── ERP.WinForms/               # Aplicación de escritorio Windows Forms
│   ├── Forms/                  # Login, Dashboard, Empleados, Planillas, etc.
│   └── Services/               # ApiClient (HTTP singleton)
├── ERP.Shared/
│   └── DTOs/                   # Contratos compartidos API <-> WinForms
└── ERP.Tests/                  # 20 tests unitarios (xUnit)
```

---

## Pasos para ejecutar el proyecto

### 1. Clonar el repositorio

```bash
git clone https://github.com/ellisrotc/ERP.git
cd ERP
```

### 2. Crear el archivo `.env`

Crea un archivo `.env` en la raíz del proyecto (mismo nivel que `docker-compose.yml`):

```env
DB_PASSWORD=ErpUnaj2026!
JWT_SECRET=ErpUnajJwtSecret2026SuperSecure512bits!XYZ
JWT_ISSUER=ERP.UNAJ
JWT_AUDIENCE=ERP.UNAJ.Client
CONNECTION_STRING=Host=localhost;Port=5432;Database=erp_financiero;Username=erp_user;Password=ErpUnaj2026!
```

> El archivo `.env` está en `.gitignore` por seguridad, **debes crearlo manualmente** cada vez que clones el repositorio.

### 3. Levantar la base de datos con Docker

```bash
docker-compose up -d
```

Esto levanta automáticamente:
- **PostgreSQL 16** en `localhost:5432`
- **Adminer** en `http://localhost:8080`

El script `sql/init.sql` se ejecuta solo la primera vez y crea:
- Tablas normalizadas en 3FN
- Stored procedures de planilla y libros contables
- Datos iniciales: roles, tipos de descuento, usuarios y 3 empleados de ejemplo

### 4. Iniciar el Backend (API)

```bash
cd ERP.Api
dotnet run --launch-profile http
```

La API queda disponible en:
- **API:** `http://localhost:5260`
- **Swagger UI:** `http://localhost:5260/swagger`

### 5. Iniciar el Frontend (WinForms)

Abre una **segunda terminal** y ejecuta:

```bash
cd ERP.WinForms
dotnet run
```

Aparecerá la ventana de login. El WinForms se conecta a la API en `http://localhost:5260`.

### 6. Ejecutar los tests

```bash
cd ERP.Tests
dotnet test
```

Resultado esperado: **20 tests pasando, 0 errores**.

---

## Credenciales de acceso

| Usuario | Contraseña | Rol | Módulos disponibles |
|---|---|---|---|
| `admin` | `Admin123!` | Admin | Todo el sistema |
| `rrhh1` | `Admin123!` | RRHH | Empleados, Planillas |
| `contador1` | `Admin123!` | Contador | Comprobantes, Libros, Reportes |

---

## Módulos del sistema

El Dashboard usa un **panel lateral fijo**. Al hacer clic en cada módulo, el contenido se carga en el área derecha **sin abrir nuevas ventanas**.

### Empleados
- Listado completo con DataGridView
- Crear y editar (nombre, DNI, cargo, salario base, tipo de descuento AFP/ONP)
- Desactivar con soft delete (`activo = false`, nunca se borra de la BD)

### Planillas
- Seleccionar período (`YYYY-MM`)
- Calcular planilla para todos los empleados activos via stored procedure PostgreSQL
- Ver: Bruto, AFP (10%), ONP (13%), CTS (bruto/12), Gratificación (bruto/6 en julio y diciembre), Neto
- Descargar boleta de pago individual en **PDF** (QuestPDF)

### Comprobantes
- Registrar facturas, boletas y notas de crédito
- IGV calculado automáticamente: `IGV = Monto / 1.18 × 0.18`
- Validación: RUC debe tener exactamente 11 dígitos numéricos
- Historial con filtro por fecha

### Libros Contables
- Generar Libro de Ventas y Libro de Compras por período
- Exportar a **Excel** con ClosedXML

### Reportes
- Balance general por período
- Estado de resultados por período

---

## Endpoints de la API

| Método | Ruta | Roles | Descripción |
|---|---|---|---|
| POST | `/api/auth/login` | Público | Iniciar sesión |
| POST | `/api/auth/refresh` | Público | Renovar access token |
| GET | `/api/empleados` | Admin, RRHH | Listar empleados |
| POST | `/api/empleados` | Admin, RRHH | Crear empleado |
| PUT | `/api/empleados/{id}` | Admin, RRHH | Editar empleado |
| DELETE | `/api/empleados/{id}` | Admin, RRHH | Desactivar (soft delete) |
| POST | `/api/planillas/calcular?periodo=` | Admin, RRHH | Calcular planilla del período |
| GET | `/api/planillas?periodo=` | Admin, RRHH | Listar planillas calculadas |
| GET | `/api/planillas/{id}/pdf` | Admin, RRHH | Descargar PDF boleta de pago |
| GET | `/api/comprobantes` | Admin, Contador | Listar comprobantes |
| POST | `/api/comprobantes` | Admin, Contador | Registrar comprobante |
| GET | `/api/libros/ventas?periodo=` | Admin, Contador | Generar libro ventas |
| GET | `/api/libros/compras?periodo=` | Admin, Contador | Generar libro compras |
| GET | `/api/libros/ventas/{periodo}/excel` | Admin, Contador | Exportar ventas a Excel |
| GET | `/api/reportes/balance?periodo=` | Admin, Gerente, Contador | Balance general |
| GET | `/api/reportes/resultados?periodo=` | Admin, Gerente, Contador | Estado de resultados |

---

## Adminer – Gestor visual de base de datos

Accede a `http://localhost:8080`:

| Campo | Valor |
|---|---|
| Sistema | PostgreSQL |
| Servidor | `postgres` |
| Usuario | `erp_user` |
| Contraseña | `ErpUnaj2026!` |
| Base de datos | `erp_financiero` |

---

## Reglas de negocio

| Concepto | Regla |
|---|---|
| IGV | 18% fijo: `Monto / 1.18 × 0.18` |
| AFP | 10% del salario bruto |
| ONP | 13% del salario bruto |
| CTS | Salario bruto / 12 |
| Gratificación | Salario bruto / 6 (solo julio y diciembre) |
| Neto | Bruto − AFP/ONP |
| RUC | Exactamente 11 dígitos numéricos |
| Empleados | Soft delete: `activo = false`, nunca DELETE físico |

---

## Comandos de referencia rápida

```bash
# Docker
docker-compose up -d          # Levantar
docker-compose down           # Detener
docker logs erp_postgres      # Ver logs de BD

# API
cd ERP.Api
dotnet run --launch-profile http

# WinForms (requiere Windows)
cd ERP.WinForms
dotnet run

# Tests
cd ERP.Tests
dotnet test --verbosity normal
```

---

## Licencia

Proyecto académico — Universidad Nacional de Juliaca, Taller de Procesos ERP 2026.

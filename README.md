# Dulce Fé Backend - .NET 9.0 API

Este es el backend oficial de la pastelería **Dulce Fé**, desarrollado utilizando **.NET 9.0**, **Entity Framework Core**, y siguiendo los principios de **Domain-Driven Design (DDD)** y **Bounded Contexts**.

## 🚀 Arquitectura del Proyecto

El sistema está dividido en contextos delimitados (Bounded Contexts) para asegurar un código limpio, mantenible y escalable:

- **IAM (Identity and Access Management)**: Gestión de usuarios, autenticación JWT y hashing de contraseñas.
- **Catalog**: Gestión de productos (tortas, postres, bebidas) y categorías.
- **Sales**: Procesamiento de órdenes, carritos de compras y analíticas de ventas.
- **Promotions**: Sistema de cupones y descuentos.
- **Services**: Gestión de servicios adicionales como consultas de catering y suscripciones a talleres.
- **Social**: Gestión de testimonios y reseñas de clientes.
- **Shared**: Infraestructura común, repositorios base y configuración de base de datos.

---

## 🛠️ Requisitos Previos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) (o una instancia en la nube como Aiven)
- [Docker](https://www.docker.com/) (opcional, para despliegue)

---

## 💻 Ejecución Local

### 1. Clonar el repositorio
```bash
git clone <url-del-repositorio>
cd ProyectoPostre/BackEnd
```

### 2. Configurar la Base de Datos
Abre el archivo `DulceFe.API/appsettings.json` y configura tu cadena de conexión:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=dulcefe_db;Uid=root;Pwd=admin;"
  },
  "TokenSettings": {
    "Secret": "1234567890JafethWorrenYngaAmadoA"
  }
}
```

### 3. Ejecutar la aplicación
Desde la raíz del proyecto, ejecuta:

```bash
dotnet build
dotnet run --project DulceFe.API
```

La aplicación se iniciará por defecto en `http://localhost:5200`.

### 4. Acceder a la Documentación (Swagger)
Una vez ejecutándose, abre tu navegador en:
`http://localhost:5200/swagger`

---

## 🐋 Ejecución con Docker

Si prefieres usar Docker, ya existe un `Dockerfile` configurado:

```bash
docker build -t dulcefe-backend .
docker run -p 8080:8080 -e ConnectionStrings__DefaultConnection="tu_string" dulcefe-backend
```

---

## 🌐 Despliegue en Render

Este backend está optimizado para **Render**:
- Utiliza la variable de entorno `PORT` dinámicamente.
- Realiza migraciones automáticas y seeding de datos al iniciar (`context.Database.EnsureCreated()`).
- Expone Swagger incluso en producción para facilitar las pruebas de desarrollo.

---

## 📄 Características de la API

- **CORS**: Configurado para permitir peticiones desde cualquier origen (ajustable en `Program.cs`).
- **Snake Case**: La base de datos y los JSON utilizan `snake_case` por convención.
- **Kebab Case**: Las rutas de los controladores usan `kebab-case` (ej. `/api/v1/admin-products`).
- **Autenticación**: Endpoints protegidos mediante el middleware `UseRequestAuthorization`.
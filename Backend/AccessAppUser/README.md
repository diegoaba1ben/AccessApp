# AccessAppUser

## Propósito del microservicio

Este microservicio forma parte de la solución modular **AccessApp**. Su objetivo es gestionar:

- Usuarios.
- Roles.
- Permisos.

El microservicio está diseñado utilizando **Domain-Driven Design (DDD)** y se comunica con otros microservicios a través de **HTTP** o, en el futuro, **mensajería asíncrona**.

---

## Dependencias

Este proyecto utiliza las siguientes tecnologías y paquetes:

- **ASP.NET Core 8.0**: Para construir la API REST.
- **Entity Framework Core**: Para manejar la persistencia de datos.
- **Pomelo.EntityFrameworkCore.MySql**: Proveedor para MySQL.
- **FluentValidation**: Para validaciones robustas.
- **Swashbuckle.AspNetCore**: Para la documentación y pruebas interactivas de la API (Swagger).
- **Docker**: Para facilitar el despliegue y la portabilidad.

---

## Configuración inicial

### Requisitos previos

1. **Herramientas necesarias:**
   - .NET SDK 8.0
   - Docker
   - MySQL (si no usas Docker para la base de datos)

2. **Configuración del archivo `appsettings.json`:**
   Asegúrate de que el archivo contenga la cadena de conexión adecuada:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=db;Database=AccessApp;User=appuser;Password=yourpassword;"
     }
   }

## Clonación del repositorio

  git clone <https://github.com/tu-usuario/AccessApp.git>
  cd AccessApp/Backend/AccessAppUser

## Restauración de Dependencias

  dotnet restore

## Aplicación de las migraciones de la base de datos

  dotnet ef database update

## Ejecución del proyecto

  dotnet run

## Endpoint principales

  GET/users para obtener todos los usuarios
  POST/users Crea un nuevo usuario
  GET/roles Obtiene todos roles disponibles

## LICENCIA

---

1. **Propósito del microservicio:**
   - Explica el enfoque específico de `AccessAppUser`.

2. **Dependencias:**
   - Enumera las bibliotecas y herramientas clave para que cualquier desarrollador pueda entender lo que necesita.

3. **Configuración inicial:**
   - Describe los requisitos previos y cómo preparar el entorno.

4. **Endpoints principales:**
   - Define los endpoints que el microservicio proporciona actualmente (esto se puede expandir a medida que desarrolles más funcionalidades).

5. **Licencia:**
   - Asegura que la información legal esté disponible.

---

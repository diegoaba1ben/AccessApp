# AccessApp

## Propósito del proyecto

AccessApp es una solución modular diseñada para gestionar usuarios, roles, permisos y más, utilizando una
arquitectura basada en microservicios.

## Estructura del proyecto

│ AccesApp
├───Frontend/ # Interfaz de usuario(React)
├───Backend/ # Serviciosbackend
┃ ┣ AccesAppGateway/ # API para centralizar solicitudes
┃ ┣ AccesAppSecurity/ # Servicio de seguridad para autenticación y autorización
│└───AccessAppUser/ # Servicio de gestión de usuarios.

Backend: Contiene los microservicos para gestionar usuarios y la API principal.

Frontend: Contiene la interfaz de usuario elaborada en React.

## Tecnologías utilizadas

- **Backend**

    -ASP.NET 8.0
    -EntityFrameworkCore
    -Pomelo.entityFrameworkCore.MySql
    -Swashbucle (Swagger)
    -FluentVaidation

- **Frontend**
    -React

_ **Infraestructura**
    -Docker para contenedores
    -MySQL como base de datos principal

## Cómo iniciar el proyecto

1. Clonar el repositorio: `git clone [https://github.com/diegoaba1ben/AccesApp.git](https://github.com/diegoaba1ben/AccesApp.git)`

2. Ir al directorio del proyecto
    cd AccesApp
3. Ejecuta el backend y base de datos con Docker Compose:
docker-compose-yml
4. (Opcional) configura y ejecuta el frontend

## LICENCIA

---

### **Puntos fuertes de este diseño**

1. **Claridad:**
   - Explica tanto la estructura del proyecto como las tecnologías utilizadas.
2. **Estructura del proyecto:**
   - El diagrama ayuda a visualizar cómo están organizados los microservicios.
3. **Pasos iniciales:**
   - Proporciona instrucciones claras para ejecutar el proyecto.

---

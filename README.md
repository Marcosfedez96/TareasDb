# API de Gestión de Tareas - C#

Este proyecto es una API RESTful creada con ASP.NET Core y SQL Server. Permite realizar operaciones CRUD sobre una lista de tareas.

## Tecnologías
- ASP.NET Core
- ADO.NET
- SQL Server
- DTOs y arquitectura limpia
- Swagger

## Crear base de datos con Sql Server.
- CREATE database TareasDb
- CREATE table Tareas(
  Id int primary key Identity,
  Nombre nvarchar(100),
  Prioridad nvarchar(20)
  Completado BIT);

## Funcionalidades
- Obtener tareas (`GET /api/Tarea/GetTareas`)
- Obtener por ID (`GET /api/Tarea/GetTareaId{Id}`)
- Crear nueva tarea (`POST /api/Tarea/PostTarea`)
- Editar (`PUT /api/Tarea/PutTarea{Id}`)
- Marcar como completada (`PATCH /api/Tarea/PatchTarea{Id}/Completar`)
- Eliminar (`DELETE /api/Tarea/DeleteTarea{Id}`)

## Ejecutar el proyecto
1. Clonar el repositorio
2. Configurar la cadena de conexión en appsettings.json:
    "ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR_SQL;Database=TareasDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Ejecutar.




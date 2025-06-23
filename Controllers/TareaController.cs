using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TareasDb.Data;
using TareasDb.DTOs;
using TareasDb.Models;

namespace TareasDb.Controllers;

[ApiController]

[Route("api/[controller]")]

public class TareaController : ControllerBase
{
    private readonly ConnectionDb _connectionDb;

    public TareaController(ConnectionDb connectionDb)
    {
        _connectionDb = connectionDb;
    }


    [HttpGet("test-connection")]
    public IActionResult TestConnection()
    {
        try
        {
            using SqlConnection connection = _connectionDb.GetConnection();
            connection.Open();
            return Ok("Coneccion exitosa a la base de datos.");


        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al conectar:{ex.Message}");
        }
    }

    [HttpGet]
    public ActionResult GetTareas()
    {
        List<Tarea> tareas = new List<Tarea>();

        using SqlConnection connection = _connectionDb.GetConnection();
        connection.Open();

        string query = "SELECT * FROM Tareas;";

        using SqlCommand command = new SqlCommand(query, connection);
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            tareas.Add(new Tarea
            {
                Id = (int)reader["ID"],
                Name = reader["Nombre"].ToString(),
                Priority = reader["Prioridad"].ToString(),
                State = (bool)reader["Completado"]
            });
        }

        return Ok(tareas);
    }
    [HttpPost]
    public ActionResult PostTarea([FromBody] TareaDTO tarea)
    {
        using SqlConnection connection = _connectionDb.GetConnection();
        connection.Open();

        string query = "INSERT INTO Tareas(Nombre,Prioridad,Completado) VALUES(@nombre,@prioridad,0)";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@nombre", tarea.Name);
        command.Parameters.AddWithValue("@prioridad", tarea.Priority);
        command.ExecuteNonQuery();
        return Ok("Operación realizada con éxito");
    }

    [HttpGet("{id}")]
    public ActionResult<Tarea> GetTarea(int id)
    {
        using SqlConnection connection = _connectionDb.GetConnection();
        connection.Open();

        string query = "SELECT Id, Nombre, Prioridad, Completado FROM Tareas where Id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("id", id);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Tarea tarea = new Tarea
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Priority = reader.GetString(2),
                State = reader.GetBoolean(3)
            };
            return Ok(tarea);

        }
        else
        {
            return NotFound($"No se encontró la tarea con el Id {id}");
        }
    }

    [HttpPut("{id}")]
    public ActionResult PutTarea(int id, [FromBody] TareaDTO tareaDto)
    {
        using SqlConnection connection = _connectionDb.GetConnection();
        connection.Open();

        string query = "UPDATE Tareas SET Nombre = @nombre, Prioridad = @prioridad WHERE Id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@nombre", tareaDto.Name);
        command.Parameters.AddWithValue("@prioridad", tareaDto.Priority);

        int filasAfectadas = command.ExecuteNonQuery();

        if (filasAfectadas == 0)
        {
            return NotFound($"no se encontró la tarea con el id {id}.");

        }
        else
        {
            return Ok(tareaDto);
        }
    }
    [HttpPatch("{id}/Completar")]
    public ActionResult PatchTareaEstado(int id, [FromBody] bool completado)
    {
        using SqlConnection connection = _connectionDb.GetConnection();
        connection.Open();

        string query = "Update Tareas Set Completado = @completado WHERE id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);
        command.Parameters.AddWithValue("@completado", completado);

        int filasAfectadas = command.ExecuteNonQuery();

        if (filasAfectadas == 0)
        {
            return BadRequest($"no se encontró la tarea con el id {id}.");
        }
        else
        {
            return Ok($"Tarea marcada como {(completado ? "completado" : "incompleta")}");
        }
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteTarea(int id)
    {
        using SqlConnection connection = _connectionDb.GetConnection();
        connection.Open();

        string query = "Delete from tareas where Id = @id";
        using SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        int filasAfectadas = command.ExecuteNonQuery();

        if(filasAfectadas == 0)
        {
            return NotFound($"no se encontró la tarea con el id {id}.");
        }
        else
        {
            return Ok($"La tarea con el Id {id} a sido borrada correctamente");
        }


    }
}

using System.ComponentModel.DataAnnotations;

namespace Ispit.Todo.Models;

public class TodoTask
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public bool Status { get; set; }
    public int TodolistId { get; set; }
    public virtual Todolist Todolist { get; set; }
}

﻿using System.ComponentModel.DataAnnotations;

namespace Ispit.Todo.Models;

public class Todolist
{
    [Key] public int Id { get; set; }
    [Required] public string Name { get; set; }
    public string UserId { get; set; }
    public virtual TodoTask TodoTasks { get; set; }
}
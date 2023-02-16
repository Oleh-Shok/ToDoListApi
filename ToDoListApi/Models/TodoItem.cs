using FluentValidation;

namespace ToDoListApi.Models;

public class TodoItem
{
    public int TaskId { get; set ; }
    public string? TaskDescription { get; set; }
    public bool IsComplete { get; set; }
}






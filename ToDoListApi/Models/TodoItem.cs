using FluentValidation;

namespace ToDoListApi.Models;

public class TodoItem
{
    public int TaskId { get; set ; }
    public string? TaskDescription { get; set; }
    public bool IsComplete { get; set; }
}

public class TaskValidator : AbstractValidator<TodoItem>
{
    public TaskValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0).WithMessage("taskId must be greater than 0")
            .NotNull().WithMessage("taskId cannot be null");
        RuleFor(x => x.TaskDescription)
            .NotNull().WithMessage("taskDescription cannot be empty. Please write something!")
            .NotEmpty().WithMessage("taskDescription cannot be empty. Please write something!");        
    }
}






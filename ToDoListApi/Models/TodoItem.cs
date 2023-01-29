using FluentValidation;
using System;
using Microsoft.AspNetCore.Mvc;

namespace ToDoListApi.Models;

public class TodoItem
{
    public int taskId { get; set ; }
    public string? taskDescription { get; set; }
    public bool isComplete { get; set; }
}

public class TaskValidator : AbstractValidator<TodoItem>
{
    public TaskValidator()
    {

        RuleFor(x => x.taskId)
            .GreaterThan(0).WithMessage("taskId must be greater than 0")
            .NotNull().WithMessage("taskId cannot be null");
        RuleFor(x => x.taskDescription)
            .NotNull().WithMessage("taskDescription cannot be empty. Please write something!")
            .NotEmpty().WithMessage("taskDescription cannot be empty. Please write something!");
        RuleFor(c => c.isComplete).NotEmpty().WithMessage("isComplete cannot be empty. Please write something!"); 
    }
}

public class IdValidator : AbstractValidator<int>
{
    public IdValidator()
    {
        RuleFor(x => x).GreaterThan(0);
    }
}






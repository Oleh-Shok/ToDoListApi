using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Models;
using Microsoft.Extensions.Caching.Memory;
using FluentValidation;

namespace ToDoListApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{

    private readonly ITodoItemService _todoItemService;
    private readonly IValidator<TodoItem> _taskValidator;
    private readonly IValidator<int> _taskIdValidator;

    public TodoListController(ITodoItemService todoItemService, IValidator<TodoItem> taskValidator, IValidator<int> taskIdValidator)
    {
        _todoItemService = todoItemService;
        _taskValidator = taskValidator;
        _taskIdValidator = taskIdValidator;
    }

    [HttpPost]
    public ActionResult<TodoItem> AddTodoItem(TodoItem newTodoItem)
    {
        var validationResult = _taskValidator.Validate(newTodoItem);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (_todoItemService.Exist(newTodoItem.taskId))
        {
            return Conflict($"Task with id {newTodoItem.taskId} already exists");
        }
        _todoItemService.AddTodoItem(newTodoItem);
        var successMessages = new List<string>
        {
            "Task added successfully"
        };
        return Ok(successMessages);
    }

    [HttpDelete("{taskId}")]
    public ActionResult<TodoItem> RemoveTodoItem(int taskId)
    {
        var validationResult = _taskIdValidator.Validate(taskId);
        if (!validationResult.IsValid)
        {
            return BadRequest();
        }
        if (!_todoItemService.Exist(taskId))
        {
            return NotFound($"Task with id {taskId} not found");
        }
        _todoItemService.RemoveTodoItem(taskId);
        var deleteMessages = new List<string>
        {
            "Task deleted successfully"
        };
        return Ok(deleteMessages);
        
    }

    [HttpGet]
    public ActionResult<List<TodoItem>> GetTodoItem()
    {
        var tasks = _todoItemService.GetTodoItem();
        if (tasks.Count == 0)
        {
            return NotFound("To Do list is empty now. Please add task.");
        }
        return Ok(tasks);
    }

    [HttpPut("{taskId}")]
    public ActionResult<TodoItem> UpdateCase(int taskId, TodoItem updatedTask)
    {
        var validationResult = _taskValidator.Validate(updatedTask);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        if (!_todoItemService.Exist(taskId))
        {
            return NotFound($"Task with id {taskId} not found");
        }

        _todoItemService.UpdateCase(taskId, updatedTask);
        var updateMessages = new List<string>
        {
            "Task updated successfully"
        };
        return Ok(updateMessages);
    }
}

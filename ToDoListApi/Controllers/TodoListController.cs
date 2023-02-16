using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Services;
using ToDoListApi.Models;
using Microsoft.AspNetCore.Authorization;
using ToDoListApi.Models.Exceptions;

namespace ToDoListApi.Controllers;

[ApiController]
[Route("api/toDoList")]
public class TodoListController : ControllerBase
{
    private readonly ITodoItemService _todoItemService;    

    public TodoListController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [Authorize]
    [HttpPost("add-task")]
    public ActionResult<TodoItem> AddTodoItem(TodoItem newTodoItem)
    {
        if (_todoItemService.Exist(newTodoItem.TaskId))
        {
            throw new ValidationException($"Task with id {newTodoItem.TaskId} already exists");
        }
        _todoItemService.AddTodoItem(newTodoItem);
        var successMessages = new List<string>
        {
            "Task added successfully"
        };
        return Ok(successMessages);
    }

    [Authorize]
    [HttpDelete("delete-task")]
    public ActionResult<TodoItem> RemoveTodoItem(int taskId)
    {
        if (!_todoItemService.Exist(taskId))
        {
            throw new NotFoundException($"Task with id {taskId} not found");
        }
        _todoItemService.RemoveTodoItem(taskId);
        var deleteMessages = new List<string>
        {
            "Task deleted successfully"
        };
        return Ok(deleteMessages);
        
    }

    [AllowAnonymous]
    [HttpGet("get-list-of-todoitems")]
    public ActionResult<List<TodoItem>> GetTodoItem()
    {
        var tasks = _todoItemService.GetTodoItem();
        if (tasks.Count == 0)
        {
            throw new NotFoundException("To Do list is empty now. Please add task.");
        }
        return tasks;
    }

    [Authorize]
    [HttpPut("update-task")]
    public ActionResult<TodoItem> UpdateCase(int taskId, TodoItem updatedTask)
    {
        if (!_todoItemService.Exist(taskId))
        {
            throw new NotFoundException($"Task with id {taskId} not found");
        }

        _todoItemService.UpdateCase(taskId, updatedTask);
        var updateMessages = new List<string>
        {
            "Task updated successfully"
        };
        return Ok(updateMessages);
    }
}

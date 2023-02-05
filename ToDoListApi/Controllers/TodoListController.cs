using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Services;
using ToDoListApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace ToDoListApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoListController : ControllerBase
{

    private readonly ITodoItemService _todoItemService;    

    public TodoListController(ITodoItemService todoItemService)
    {
        _todoItemService = todoItemService;
    }

    [Authorize]
    [HttpPost]
    public ActionResult<TodoItem> AddTodoItem(TodoItem newTodoItem)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.MaxAllowedErrors);
        }

        if (_todoItemService.Exist(newTodoItem.TaskId))
        {
            return Conflict($"Task with id {newTodoItem.TaskId} already exists");
        }
        _todoItemService.AddTodoItem(newTodoItem);
        var successMessages = new List<string>
        {
            "Task added successfully"
        };
        return Ok(successMessages);
    }

    [Authorize]
    [HttpDelete("{taskId}")]
    public ActionResult<TodoItem> RemoveTodoItem(int taskId)
    {       
        if (!ModelState.IsValid)
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

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<List<TodoItem>> GetTodoItem()
    {
        var tasks = _todoItemService.GetTodoItem();
        if (tasks.Count == 0)
        {
            return NotFound("To Do list is empty now. Please add task.");
        }
        return tasks;
    }

    [Authorize]
    [HttpPut("{taskId}")]
    public ActionResult<TodoItem> UpdateCase(int taskId, TodoItem updatedTask)
    {        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState.MaxAllowedErrors);
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

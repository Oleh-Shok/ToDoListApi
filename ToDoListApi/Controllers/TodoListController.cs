using Microsoft.AspNetCore.Mvc;
using ToDoListApi.Services;
using ToDoListApi.Models;
using Microsoft.AspNetCore.Authorization;
using ToDoListApi.Models.Exceptions;
using Microsoft.Extensions.Localization;

namespace ToDoListApi.Controllers;

[ApiController]
[Route("api/toDoList")]
public class TodoListController : ControllerBase
{    
    private readonly ITodoItemService _todoItemService;
    private readonly IStringLocalizer _resourceLocalizer;
    public TodoListController(ITodoItemService todoItemService, IStringLocalizer resourceLocalizer)
    {
        _todoItemService = todoItemService;
        _resourceLocalizer = resourceLocalizer;
    }

    [Authorize]
    [HttpPost("add-task")]
    public ActionResult<TodoItem> AddTodoItem(TodoItem newTodoItem)
    {
        if (_todoItemService.Exist(newTodoItem.TaskId))
        {
            throw new ValidationException(_resourceLocalizer["ExistsTask", newTodoItem]);
        }
        _todoItemService.AddTodoItem(newTodoItem);
        var successMessages = new List<string>
        {
            _resourceLocalizer["AddTask", new List<string>()]
        };
        return Ok(successMessages);
    }

    [Authorize]
    [HttpDelete("delete-task")]
    public ActionResult<TodoItem> RemoveTodoItem(int taskId)
    {
        if (!_todoItemService.Exist(taskId))
        {
            throw new NotFoundException(_resourceLocalizer["NotFoundTask", taskId]);
        }
        _todoItemService.RemoveTodoItem(taskId);
        var deleteMessages = new List<string>
        {
            _resourceLocalizer["DeleteTask", new List<string>()]
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
            throw new NotFoundException(_resourceLocalizer["EmptyTodoList", tasks]);
        }
        return tasks;
    }

    [Authorize]
    [HttpPut("update-task")]
    public ActionResult<TodoItem> UpdateCase(int taskId, TodoItem updatedTask)
    {
        if (!_todoItemService.Exist(taskId))
        {
            throw new NotFoundException(_resourceLocalizer["NotFoundTask", taskId]);
        }

        _todoItemService.UpdateCase(taskId, updatedTask);
        var updateMessages = new List<string>
        {
            _resourceLocalizer["UpdateTask", new List<string>()]
        };
        return Ok(updateMessages);
    }
}

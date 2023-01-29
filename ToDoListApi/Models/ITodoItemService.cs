using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToDoListApi.Models;

public interface ITodoItemService
{
   
    public void AddTodoItem(TodoItem newCase);
    public void RemoveTodoItem(int id);
    public List<TodoItem> GetTodoItem();
    bool Exist(int id);
    void UpdateCase(int id, TodoItem newCase);
}

public class TodoItemService : ITodoItemService
{
    private readonly string _pathToFile;
    private List<TodoItem> _tasks;

    public TodoItemService()
    {
        _pathToFile = "tasks.json";
        _tasks = new List<TodoItem>
        {
            new TodoItem { taskId = 1, taskDescription = "Teambuilding at 9 o'clock.", IsComplete = false },
            new TodoItem { taskId = 2, taskDescription = "Send an e-mail to Anatoliy Rostislavovich.", IsComplete = false },
            new TodoItem { taskId = 3, taskDescription = "Call to Sofiya and speaking about workshop at 10 o'clock.", IsComplete = false }
        };
    }

    public void AddTodoItem(TodoItem newTask)
    {
        _tasks = ReadFromFile();
        _tasks.Add(newTask);
        SaveToFile();
    }

    public void RemoveTodoItem(int taskId)
    {
        _tasks = ReadFromFile();
        _tasks.RemoveAll(x => x.taskId == taskId);
        SaveToFile();
    }

    public List<TodoItem> GetTodoItem()
    {
        _tasks = ReadFromFile();
        return _tasks;
    }

    public bool Exist(int taskId)
    {
        return _tasks.Any(x => x.taskId == taskId);
    }

    public void UpdateCase(int taskId, TodoItem newCase)
    {
        _tasks = ReadFromFile();
        var existingCase = _tasks.FirstOrDefault(x => x.taskId == taskId);
        if (existingCase != null)
        {
            existingCase.taskId = newCase.taskId;
            existingCase.taskDescription = newCase.taskDescription;
            existingCase.IsComplete = newCase.IsComplete;
            SaveToFile();
        }
    }

    private void SaveToFile()
    {
        using (var writerTask = new StreamWriter(_pathToFile))
        {
            var json = JsonSerializer.Serialize(_tasks);
            writerTask.Write(json);
        }
    }

    private List<TodoItem> ReadFromFile()
    {
        _tasks = new List<TodoItem>()
        {
            new TodoItem { taskId = 1, taskDescription = "Teambuilding at 9 o'clock.", IsComplete = false },
            new TodoItem { taskId = 2, taskDescription = "Send an e-mail to Anatoliy Rostislavovich.", IsComplete = false },
            new TodoItem { taskId = 3, taskDescription = "Call to Sofiya and speaking about workshop at 10 o'clock.", IsComplete = false }
        };

        if (!File.Exists(_pathToFile))
        {
            File.WriteAllText(_pathToFile, JsonSerializer.Serialize(_tasks));
            return _tasks;

        };

        using (var reader = new StreamReader(_pathToFile))
        {
            var json = reader.ReadToEnd();
            if (string.IsNullOrEmpty(json))
            {
                return _tasks;                
            }
            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        }
    }
}
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Udemy1
{

    internal class TaskManager
    {

        private readonly ITodoRepository repo;
        private readonly string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "database.txt");


        public TaskManager(ITodoRepository repo)
        {

            this.repo = repo;

            if (!File.Exists(path))
            {
                File.Create(path);

                File.WriteAllText(path, "Id|||Title||Deadline\n");
            }
        }

        public void Start()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("Welcome to your Task Manager. Write the action you want to perform (with ID if needed)");
            Console.WriteLine("--------------------------------------------------------------------------------------");

            string action;

            while (true)
            {
                Console.WriteLine("1. Create");
                Console.WriteLine("2. Update");
                Console.WriteLine("3. View");
                Console.WriteLine("4. Delete");
                Console.WriteLine("5. Exit");

                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Input cannot be empty");
                    return;
                }

                action = input.Split(" ")[0].ToLower();

                switch (action)
                {
                    case "create":
                        Console.WriteLine("You have chosen to create a new task.");
                        CreateTask();
                        break;

                    case "view":
                        Console.WriteLine("You have chosen to view tasks");
                        ViewTasks();
                        break;

                    case "delete":
                        if(input.Split(" ").Length < 2)
                        {
                            Console.WriteLine("You need to provide an ID for this action");
                            break;
                        }

                        int id = int.Parse(input.Split(" ")[1]);
                        Console.WriteLine("You have chosen to delete a task");
                        DeleteTask(id);
                        break;

                    case "update":
                        if (input.Split(" ").Length < 2)
                        {
                            Console.WriteLine("You need to provide an ID for this action");
                            break;
                        }

                        int id2 = int.Parse(input.Split(" ")[1]);
                        Console.WriteLine("You have chosen to update a task");
                        UpdateTask(id2);
                        break;
                    case "exit":
                        return;

                    default:
                        Console.WriteLine("Action is not available.");
                        break;
                }
            }
        }

        public void ViewTasks()
        {
            Console.WriteLine("Here are your tasks:");
            repo.ViewAll();

        }

        public void CreateTask()
        {

            Console.WriteLine("Enter task title.");
            string? title = Console.ReadLine();

            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("");
                return;
            }

            Console.WriteLine("Enter task deadline.");

            string? deadlineInput = Console.ReadLine();

            if (!DateTime.TryParse(deadlineInput, out DateTime deadline))
            {
                Console.WriteLine("Wrong datetime format.");
                return;
            }

            repo.Save(new Task(title, deadline,0));

        }

        public void DeleteTask(int taskID)
        {
            if (taskID < 1)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }

            repo.Delete(taskID);

        }

        public void UpdateTask(int taskID)
        {
            if(taskID < 1)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }

            var currentTask = repo.Get(taskID);

            if (currentTask is null)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }

            Console.WriteLine("Write title or deadline depending on what you want to change");
            var change = Console.ReadLine();
            Console.WriteLine("Write new value");
            var newVal = Console.ReadLine();

            if (string.IsNullOrEmpty(change) || string.IsNullOrEmpty(newVal))
            {
                Console.WriteLine("Input cannot be empty");
                return;
            }

            switch (change)
            {
                case ("title"):
                    {
                        currentTask.title = newVal;
                        break;
                    }
                case ("deadline"):
                    {
                        if (!DateTime.TryParse(newVal, out DateTime deadline))
                        {
                            Console.WriteLine("Wrong datetime format.");
                            return;
                        }

                        currentTask.deadline = DateTime.Parse(newVal);
                        break;
                    }
                default:
                    Console.WriteLine("Invalid input");
                    break;

            }

            repo.Update(currentTask);

        }
    }

    internal class TodoTxtRepository : ITodoRepository
    {
        private readonly string path;

        private readonly List<Task> taskList = [];

        private int idPointer = 1;
        //NOTE: Can use GUID for ids too

        public TodoTxtRepository(string databaseFilePath)
        {
            path = databaseFilePath;
            LoadTasks();
        }

        public IReadOnlyCollection<Task> Tasks => taskList.AsReadOnly();

        /// <summary>
        /// Reads all lines in file and parse all fields of a task. Add each task to taskList.
        /// </summary>
        private void LoadTasks()
        {
            var lines = File.ReadAllLines(path).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                var line = lines[i];
                var splitLine = line.Split("||");
                int id = int.Parse(splitLine[0]);

                if (id > idPointer)
                {
                    idPointer = id+1;
                }

                string title = splitLine[1];
                DateTime deadline = DateTime.Parse(splitLine[2]);
                taskList.Add(new Task(title, deadline, id));
            }
        }

        /// <summary>
        /// Saves all tasks found in taskList to file.
        /// </summary>
        private void SaveAllTasks()
        {
            var lines = new List<string>();
            lines.Add("Id||Title||Deadline");
            foreach (var task in taskList)
            {
                lines.Add($"{task.id}||{task.title}||{task.deadline:dd/MM/yyyy}");
            }

            var context = string.Join(Environment.NewLine, lines);
            File.WriteAllText(path, context);
        }

        /// <summary>
        /// Uses taskID to find the task to be deleted in taskList. Removes found task and calls SaveAllTasks().
        /// </summary>
        /// <param name="taskID">ID of task to be deleted</param>
        public void Delete(int taskID)
        {

            Task? foundedTask = null;

            //approach with linq
            foundedTask = taskList.FirstOrDefault(item => item.id == taskID);
            //TODO: can use Get() instead but then would i still need to implement the error?

            if (foundedTask is null)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }
            else
            {
                Console.WriteLine("Task {0}-{1} has been deleted", taskID, foundedTask.title);
                taskList.Remove(foundedTask);
                SaveAllTasks();
            }

        }

        public Task? Get(int id)
        {
            return taskList.First(task => task.id == id);
        }

        public void Update(Task task)
        {
            var updatedTask = taskList.FirstOrDefault(t => t.id == task.id);

            ArgumentNullException.ThrowIfNull(updatedTask, "Task with that ID does not exist");

            updatedTask.title = task.title;
            updatedTask.deadline = task.deadline;
            Console.WriteLine("You have updated {0}-{1}, with the deadline {2:dd/MM/yyyy}", updatedTask.id, updatedTask.title, updatedTask.deadline);

            SaveAllTasks();

        }

        public void Save(Task task)
        {
            
            task.id = idPointer;
            taskList.Add(task);
            idPointer++;
            SaveAllTasks();
            Console.WriteLine("You have created {0}-{1}, with the deadline {2:dd/MM/yyyy}", task.id, task.title, task.deadline);

        }

        /// <summary>
        /// Prints all tasks in taskList with taskid, title, and deadline.
        /// </summary>
        public void ViewAll()
        {
            foreach (var task in taskList)
            {
                Console.WriteLine("{0}-{1}, deadline: {2:dd/MM/yyyy}", task.id, task.title, task.deadline);
            }

        }
    }

}

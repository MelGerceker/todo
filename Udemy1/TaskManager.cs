using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Udemy1
{

    internal class TaskManager
    {
        public List<Task> taskList;

        public int idPointer = 1;
        private readonly ITodoRepository repo;
        private readonly string path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "database.txt");


        public TaskManager(ITodoRepository repo)
        {
            taskList= new List<Task>(); //initialization can be simplified?

            this.repo = repo;

            if (!File.Exists(path))
            {
                File.Create(path); // should i call close()

                File.WriteAllText(path, "Id|||Title||Deadline\n");
                //Console.WriteLine(Path.GetFullPath(path));
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
            for (int i = 0; i < taskList.Count; i++)
            {
                if (taskList[i] is not null)
                {
                    var task = taskList[i];

                    if (task is null)
                    {
                        Console.WriteLine("Task with that ID does not exist");
                        return;
                    }

                    Console.WriteLine("{0}-{1}, deadline: {2:dd/MM/yyyy}", task.id, task.title, task.deadline);
                }
            }
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

            taskList.Add(new Task(title, deadline, idPointer));

            Console.WriteLine("You have created {0}-{1}, with the deadline {2:dd/MM/yyyy}", idPointer, title, deadline);

            //NOTE: can use ToString("D") for id formatting, can add 0 padding.

            repo.Save(new Task(title, deadline, idPointer));

            idPointer++;

        }

        public void DeleteTask(int taskID)
        {
            if (taskID < 1)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }
            // SOLID
            // single responsibility

            repo.Delete(taskID);

            for (int id=0; id< taskList.Count; id++)
            {
                if (taskList[id].id == taskID)
                {
                    Task? currentTask = taskList[id];

                    if (currentTask is null)
                    {
                        Console.WriteLine("Task with that ID does not exist");
                        return;
                    }


                    taskList.RemoveAt(id);
                    Console.WriteLine("Task {0}-{1} has been deleted", taskID, currentTask.title);

                    break;
                }
            }


        }

        public void UpdateTask(int taskID)
        {
            if(taskID > taskList.Count || taskID < 1)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }

            Task? currentTask = taskList[taskID - 1];

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
            //when i call update here, do i have to check for null/try parse again?

        }
    }

    internal class TodoTxtRepository : ITodoRepository
    {
        private readonly string path;

        private readonly List<Task> taskList = [];

        public TodoTxtRepository(string databaseFilePath)
        {
            path = databaseFilePath;
            LoadTasks();
        }

        public IReadOnlyCollection<Task> Tasks => taskList.AsReadOnly();

        private void LoadTasks()
        {
            var lines = File.ReadAllLines(path).ToList();
            for (int i = 1; i < lines.Count; i++)
            {
                var line = lines[i];
                var splitLine = line.Split("||");
                int id = int.Parse(splitLine[0]);
                string title = splitLine[1];
                DateTime deadline = DateTime.Parse(splitLine[2]);
                taskList.Add(new Task(title, deadline, id));
            }
        }

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

        public void Delete(int taskID)
        {
            // how to find the task in the file?
            //id is unque but title may have numbers so look for ||| after the id

            Task? foundedTask = null;
            // first approach
            //foreach (var item in taskList)
            //{
            //    if(item.id == taskID)
            //    {                     
            //        Console.WriteLine("Found task to delete: " + item.title);
            //        foundedTask = item;
            //        break;
            //    }
            //}

            // second approach with linq
            foundedTask = taskList.FirstOrDefault(item => item.id == taskID);

            if (foundedTask is null)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }
            else
            {
                taskList.Remove(foundedTask);
                SaveAllTasks();
            }

        }

        /// <summary>
        /// list all tasks or just get one by id?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
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
            SaveAllTasks();

            // update 5
            // title
            // value from console
            /*
            var t1 = Get(5);
            if(t1 is null)
            {
                Console.WriteLine("Not found");
            }

            var command = Console.ReadLine();
            

            var newValue = Console.ReadLine();

            if (command?.ToLowerInvariant() == "title")
            {
                t1.title = newValue;
            }
            else
            {
                t1.deadline = DateTime.Parse(newValue);
            }

            Update(t1);
            */

            /*

            var lines = File.ReadAllLines(path).ToList();

            foreach (var line in lines)
            {
                string readID = line.Split("|||")[0];

                if (readID == task.id.ToString())
                {
                    Console.WriteLine("Found line to change: " + line);

                    lines.Remove(line);
                    File.WriteAllLines(path, lines);
                    break;

                }
            }

            switch (change)
            {
                case ("title"):
                    {
                        File.WriteAllText(path, $"{task.id}|||{newVal}||{task.deadline:dd/MM/yyyy}\n", Encoding.UTF8);
                        File.WriteAllLines(path, lines);

                        break;
                    }
                case ("deadline"):
                    {
                        File.WriteAllText(path, $"{task.id}|||{task.title}||{newVal:dd/MM/yyyy}\n", Encoding.UTF8);
                        File.WriteAllLines(path, lines);

                        break;
                    }
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
            */
        }

        public void Save(Task task)
        {
           taskList.Add(task);
            SaveAllTasks();
        }
    }

}

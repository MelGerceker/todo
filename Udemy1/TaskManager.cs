namespace Udemy1
{

    internal class TaskManager
    {
        public List<Task> taskList;

        public int idPointer = 1;
        private readonly ITodoRepository repo;


        public TaskManager(ITodoRepository repo)
        {
            taskList= new List<Task>();

            this.repo = repo;
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

            //test repo
            //repo.Save(new Task("title", DateTime.Now, 1));

            taskList.Add(new Task(title, deadline, idPointer));

            Console.WriteLine("You have created {0}-{1}, with the deadline {2:dd/MM/yyyy}", idPointer, title, deadline);

            //NOTE: can use ToString("D") for id formatting, can add 0 padding.

            idPointer++;

        }

        public void DeleteTask(int taskID)
        {
            if (taskID > taskList.Count || taskID < 1)
            {
                Console.WriteLine("Task with that ID does not exist");
                return;
            }

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

        }

    }

    internal class TodoDatabaseRepository : ITodoRepository
    {
        public void Save(Task task)
        {
            
        }

        public void Delete(int id)
        {
        }

        public void Update(Task task)
        {
        }

        public Task Get(int id)
        {
            return new Task("title", DateTime.Now, id);
        }
    }

    internal class TodoTxtRepository : ITodoRepository
    {
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Get(int id)
        {
            throw new NotImplementedException();
        }

        public void Save(Task task)
        {
            
        }

        public void Update(Task task)
        {
            throw new NotImplementedException();
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Udemy1
{

    internal class TaskManager
    {
        public string action;
        public Task[] taskList = new Task[10];

        public int idPointer = 0;


        public TaskManager()
        {
            start();

        }

        public void start()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("Welcome to your Task Manager. Write the action you want to perform (with ID if needed)");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("1. Create");
            Console.WriteLine("2. Update Task");
            Console.WriteLine("3. View");
            Console.WriteLine("4. Delete Task");

            String input = Console.ReadLine();
            action = input.Split(" ")[0];


            switch (action)
            {
                case "create":
                    Console.WriteLine("You have chosen to create a new task.");
                    createTask();

                    start();
                    break;

                case "view":
                    Console.WriteLine("You have chosen to view tasks");
                    viewTasks();
                    start();
                    break;

                case "delete":
                    int id = int.Parse(input.Split(" ")[1]);
                    Console.WriteLine("You have chosen to delete a task");
                    deleteTask(id);
                    break;

                case "update":
                    int id2 = int.Parse(input.Split(" ")[1]);
                    Console.WriteLine("You have chosen to update a task");
                    updateTask(id2);
                    start();
                    break;


                default:
                    Console.WriteLine("Action is not available.");
                    start();
                    break;
            }
        }

        public void viewTasks()
        {
            Console.WriteLine("Here are your tasks:");
            for (int i = 0; i < taskList.Length; i++)
            {
                if (taskList[i] != null)
                {
                    Console.WriteLine("{0}-{1}, deadline: {2:dd/MM/yyyy}", taskList[i].id, taskList[i].title, taskList[i].deadline);
                }
            }
        }   

        public void createTask()
        {
            //TODO: need to add max number of tasks reached!!

            Console.WriteLine("Enter task title.");
            String title = Console.ReadLine();
            Console.WriteLine("Enter task deadline.");
            DateTime deadline = DateTime.Parse(Console.ReadLine());
            int id = idPointer + 1;

            taskList[idPointer] = new Task(title, deadline,id);
            idPointer++;

            Console.WriteLine("You have created {0}-{1}, with the deadline {2:dd/MM/yyyy}", id, title, deadline);


            //TODO: look at a way to display 000, 001, instead of 1 digit for id

            //TODO: since i chose an array, i can add a variable emptyPointer
            // it would point to the first deleted task and when creating a new task check if emptyPointer is null first


        }



        public void deleteTask(int taskID)
        {

            Task currentTask = taskList[taskID-1];

            taskList[taskID-1] = null; //better way to delete the object itself?

            System.Console.WriteLine("Task {0}-{1} has been deleted", taskID, currentTask.title);


        }

        public void updateTask(int taskID)
        {
            Task currentTask = taskList[taskID - 1];

            if (currentTask == null)
            {
                System.Console.WriteLine("Task with that ID does not exist");
                return;
            }

            Console.WriteLine("Write title or deadline depending on what you want to change");
            String change = Console.ReadLine();
            Console.WriteLine("Write new value");
            String newVal = Console.ReadLine();


            switch (change)
            {
                case ("title"):
                    {
                        currentTask.title = newVal;
                        break;
                    }
                case ("deadline"):
                    {
                        currentTask.deadline = DateTime.Parse(newVal);
                        break;
                    }
                default:
                    Console.WriteLine("Invalid input");
                    break;

            }

        }

    }
}

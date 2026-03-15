// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;
using System.Text;
using Udemy1;


//Some notes from the udemy course: ----------------------------

//ReadKey() function

//Console.WriteLine("{0,20}", 123);


// OR is ||
// AND is &&



int[,] matrix = new int[3, 2] { { 1, 2 }, { 2, 3 }, { 3, 4 } };
//Console.WriteLine(matrix[1,0]);

//TryParse() for string to int conversion

//StringBuilder, mutable to save mem

StringBuilder sb = new StringBuilder();
sb.Append("Mel");
//Console.WriteLine(sb);

// MAIN -------------------------------------------------
Console.Title = "Mel's Console";


DateTime currentDate = DateTime.Now;
Console.WriteLine("{0,86:dd/MM/yyyy HH:mm:ss}", currentDate);

TaskManager tm = new TaskManager();
tm.start();
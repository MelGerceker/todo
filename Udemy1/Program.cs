// See https://aka.ms/new-console-template for more information
using System.Text;
using Udemy1;

//Some notes from the udemy course: ----------------------------

//ReadKey() function

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

var tm = new TaskManager(new TodoTxtRepository());
tm.Start();


// v2
// Convert to List

// v3
// implement file repository for database
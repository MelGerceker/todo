using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Udemy1
{
    internal class Task
    {
        public string title;
        public DateTime deadline;
        public int id;


        public Task(string title, DateTime deadline,int id )
        {
            this.title = title;
            this.deadline = deadline;
            this.id = id;
        }

    }


}



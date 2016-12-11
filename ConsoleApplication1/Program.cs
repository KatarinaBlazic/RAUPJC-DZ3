using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        private const string ConnectionString =
            "Server=(localdb)\\MSSQLLocalDB;Database=TodoItemDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        static void Main(string[] args)
        {
            TodoDbContext context = new TodoDbContext(ConnectionString);
            TodoSqlRepository todoRepository = new TodoSqlRepository(context);

            Guid user1 = Guid.NewGuid();
            TodoItem item1 = new TodoItem("item1", user1);
            TodoItem item2 = new TodoItem("item2", user1);
            todoRepository.Add(item1);
            todoRepository.Add(item2);

            List<TodoItem> items1 = todoRepository.GetAll(user1);
            items1.ForEach(i => Console.WriteLine(i.Text));

            //update item2 -> item22
            item2.Text = "item22";
            todoRepository.Update(item2,user1);

            items1 = todoRepository.GetAll(user1);
            Console.WriteLine();
            items1.ForEach(i => Console.WriteLine(i.Text));

            //remove item1
            todoRepository.Remove(item1.Id, user1);
            Console.WriteLine();
            items1.ForEach(i => Console.WriteLine(i.Text));

            Console.Write("Press any key to exit.");
            Console.ReadKey();




        }
    }
}

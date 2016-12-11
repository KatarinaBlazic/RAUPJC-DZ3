using Interfacaes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class TodoSqlRepository : ITodoRepository
    {

        private readonly TodoDbContext _context;
        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            ///using(_context)
            {
                TodoItem item = _context.TodoItem.Where(t => t.Id.Equals(todoId)).FirstOrDefault();
                if(item == null)
                {
                    return null;
                }
                else if(!item.UserId.Equals(userId))
                {
                    throw new TodoAccessDeniedException(String.Format("User {0} is not the owner of the requested Todo item.",userId));
                }
                else
                {
                    return item;
                }
            }
        }

        public void Add(TodoItem todoItem)
        {
           /// using (_context)
            {
                TodoItem item = _context.TodoItem.Where(t => t.Id.Equals(todoItem.Id)).FirstOrDefault();
                if (item == null)
                {
                    _context.TodoItem.Add(todoItem);
                    _context.SaveChanges();
                }
                else
                {
                    throw new DuplicateTodoItemException(String.Format("Duplicate id: {0}", todoItem.Id));
                }
            }

        }

        public bool Remove(Guid todoId, Guid userId)
        {
           /// using (_context)
            {
                TodoItem item = _context.TodoItem.Where(t => t.Id.Equals(todoId)).FirstOrDefault();
                if (item == null)
                {
                    return false;
                }
                else if (!item.UserId.Equals(userId))
                {
                    throw new TodoAccessDeniedException(String.Format("User {0} is not the owner of the Todo item.", userId));
                }
                else
                {
                    Console.WriteLine("Brisem");
                    _context.TodoItem.Remove(item);
                    _context.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    _context.SaveChanges();
                    return true;
                }
            }
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            ///using (_context)
            {
                TodoItem item = _context.TodoItem.Where(t => t.Id.Equals(todoItem.Id)).FirstOrDefault();
                if (item == null)
                {
                    _context.TodoItem.Add(todoItem);
                    _context.SaveChanges();
                }
                else if (!item.UserId.Equals(userId))
                {
                    throw new TodoAccessDeniedException(String.Format("User {0} is not the owner of the Todo item.", userId));
                }
                else
                {
                    _context.Entry(todoItem).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                }
            }

        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
           /// using (_context)
            {
                TodoItem item = _context.TodoItem.Where(t => t.Id.Equals(todoId)).FirstOrDefault();
                if (item == null)
                {
                    return false;
                }
                else if (!item.UserId.Equals(userId))
                {
                    throw new TodoAccessDeniedException(String.Format("User {0} is not the owner of the Todo item.", userId));
                }
                else
                {
                    // _dbCtx.Entry(stud).State = System.Data.Entity.EntityState.Modified;
                    _context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
            }
        }

        public List<TodoItem> GetAll(Guid userId)
        {
           /// using(_context)
            {
                List<TodoItem> items = _context.TodoItem.Where(t => t.UserId.Equals(userId)).OrderByDescending(t => t.DateCreated).ToList();
                return items;
            }
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            ////using (_context)
            {
                List<TodoItem> items = _context.TodoItem.Where(t => t.UserId.Equals(userId) && t.DateCompleted==null).ToList();
                return items;
            }

        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            ///using (_context)
            {
                List<TodoItem> items = _context.TodoItem.Where(t => t.UserId.Equals(userId) && t.DateCompleted != null).ToList();
                return items;
            }
        }

       public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            ///using (_context)
            {
                List<TodoItem> items = _context.TodoItem.Where(t => t.UserId.Equals(userId) && filterFunction(t)).ToList();
                return items;
            }
        }




    }
}

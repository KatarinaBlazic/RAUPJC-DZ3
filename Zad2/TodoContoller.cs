using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zad2
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }
    }
}

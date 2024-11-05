using AppToDoList.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppToDoList.Controllers
{
    public class TodoController : Controller
    {
        private AppDbContext db;
        public TodoController(AppDbContext context)
        {
            db = context;
        }

        // GET: TodoController
        public async Task<IActionResult> Index()
        {
            
            return View(await db.Tasks.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Add(string desc)
        {
            await db.Tasks.AddAsync(new TaskToDo
            {
                Id = Guid.NewGuid(),
                Description = desc,
                Done = false
            });
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string desc, Guid id)
        {
            string? action = Request.Form["action"];
            Console.WriteLine(action);
            TaskToDo? task = await db.Tasks.FirstOrDefaultAsync(x => x.Id == id);
            if (action == "Р")
            {
                task.Description = desc;
                db.SaveChanges();
            }
            else if (action == "У")
            {
                db.Tasks.Remove(task);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

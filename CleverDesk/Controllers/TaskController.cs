using Microsoft.AspNetCore.Mvc;
using CleverDesk.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CleverDesk.Models.Data;

namespace CleverDesk.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;
        public TaskController(AppDbContext context)
        {
            _context = context;
        }
        private int LoggedInUserId => int.Parse(User.FindFirstValue("UserId"));

        public IActionResult Index()
        {
            var tasks = _context.Tasks.Where(t => t.UserId == LoggedInUserId).OrderBy(d => d.CreatedAt).ToList();
            return View(tasks);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = LoggedInUserId;
                _context.Tasks.Add(task);
                _context.SaveChanges();
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Task added";
                return RedirectToAction("Index");
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult ToggleStatus(int id, bool completed)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                TempData["ToastType"] = "info";
                TempData["ToastMessage"] = "Task not found";
            }
            else 
            {
                task.IsCompleted = completed;
                _context.SaveChanges();
            }
            return Ok();
        }


        public IActionResult Edit(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id && t.UserId == LoggedInUserId);

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskItem task)
        {
            task.UserId = LoggedInUserId;
            if (ModelState.IsValid)
            {
                _context.Tasks.Update(task);
                _context.SaveChanges();
                TempData["ToastType"] = "success";
                TempData["ToastMessage"] = "Task modified";
                return RedirectToAction("Index");
            }
            return View(task);
        }

        public IActionResult Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task == null)
            {
                TempData["ToastType"] = "info";
                TempData["ToastMessage"] = "Task not found";
            }
            else
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }

            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Task deleted";
            return RedirectToAction("Index");
        }
    }
}

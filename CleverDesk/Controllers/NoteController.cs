using CleverDesk.Models;
using CleverDesk.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CleverDesk.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        private readonly AppDbContext _context;
        private int LoggedInUserId => int.Parse(User.FindFirstValue("UserId"));

        public NoteController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult ShowAllNoteBooks()
        {
            var notebooks = _context.Notebooks
                .Where(n => n.UserId == LoggedInUserId)
                .Select(n => new Notebook
                {
                    Id = n.Id,
                    Title = n.Title,
                    CoverImageUrl = n.CoverImageUrl
                })
                .ToList();

            return View(notebooks);
        }


        [HttpPost]
        public async Task<IActionResult> CreateNoteBook(string Title, IFormFile? CoverImage)
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                TempData["ToastType"] = "error";
                TempData["ToastMessage"] = "Please provide title";
                return RedirectToAction("ShowAllNoteBooks");
            }

            string? imageUrl = null;

            if (CoverImage != null && CoverImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(CoverImage.FileName);
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/notebookcovers", fileName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await CoverImage.CopyToAsync(stream);
                }

                imageUrl = "/notebookcovers/" + fileName;
            }

            var notebook = new Notebook
            {
                Title = Title,
                CoverImageUrl = imageUrl,
                UserId = LoggedInUserId
            };

            _context.Notebooks.Add(notebook);
            await _context.SaveChangesAsync();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Notebook created";
            return RedirectToAction("NoteBookDetails", "Note", new { notebookId = notebook.Id });
        }

        public IActionResult NoteBookDetails(int notebookId)
        {
            var notebook = _context.Notebooks
                .Include(n => n.Notes)
                .FirstOrDefault(n => n.Id == notebookId);

            if (notebook == null)
                return NotFound();

            return View(notebook);
        }

        [HttpGet]
        public IActionResult CreateNote(int notebookId, string noteBookTitle)
        {
            var note = new Note
            {
                NotebookId = notebookId
            };
            TempData["NoteBookTitle"] = noteBookTitle;

            return View(note);
        }

        [HttpGet]
        public IActionResult EditNote(int noteId)
        {
            var mar = _context.Notes.Include(c => c.Notebook).FirstOrDefault(s => s.Id == noteId);
            return View(mar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNote(Note note)
        {
            if (note.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    _context.Notes.Add(note);
                    await _context.SaveChangesAsync();
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Note added";
                    return RedirectToAction("NoteBookDetails", "Note", new { notebookId = note.NotebookId });
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Notes.Update(note);
                    await _context.SaveChangesAsync();
                    TempData["ToastType"] = "success";
                    TempData["ToastMessage"] = "Note updated";
                    return RedirectToAction("NoteBookDetails", "Note", new { notebookId = note.NotebookId });
                }
            }

            return View(note);
        }


        public async Task<IActionResult> NoteDetails(int id)
        {
            var note = await _context.Notes
                .Include(m => m.Notebook)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return NotFound();
            }

            return View(note); 
        }

    }
}

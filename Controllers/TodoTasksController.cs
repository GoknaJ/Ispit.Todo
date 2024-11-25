using Ispit.Todo.Data;
using Ispit.Todo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ispit.Todo.Controllers;

[Authorize]
public class TodoTasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public TodoTasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: TodoTasks
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.TodoTasks.Include(t => t.Todolist);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: TodoTasks/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoTask = await _context.TodoTasks
            .Include(t => t.Todolist)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todoTask == null)
        {
            return NotFound();
        }

        return View(todoTask);
    }

    // GET: TodoTasks/Create
    public IActionResult Create()
    {
        ViewData["TodolistId"] = new SelectList(_context.Todolists, "Id", "Name");
        return View();
    }

    // POST: TodoTasks/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Status,TodolistId")] TodoTask todoTask)
    {
        ModelState.Remove(nameof(todoTask.Todolist));

        if (ModelState.IsValid)
        {
            _context.Add(todoTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["TodolistId"] = new SelectList(_context.Todolists, "Id", "Name", todoTask.TodolistId);
        return View(todoTask);
    }

    // GET: TodoTasks/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoTask = await _context.TodoTasks.FindAsync(id);
        if (todoTask == null)
        {
            return NotFound();
        }
        ViewData["TodolistId"] = new SelectList(_context.Todolists, "Id", "Name", todoTask.TodolistId);
        return View(todoTask);
    }

    // POST: TodoTasks/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Status,TodolistId")] TodoTask todoTask)
    {
        ModelState.Remove(nameof(todoTask.Todolist));

        if (id != todoTask.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(todoTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoTaskExists(todoTask.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["TodolistId"] = new SelectList(_context.Todolists, "Id", "Name", todoTask.TodolistId);
        return View(todoTask);
    }

    // GET: TodoTasks/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoTask = await _context.TodoTasks
            .Include(t => t.Todolist)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todoTask == null)
        {
            return NotFound();
        }

        return View(todoTask);
    }

    // POST: TodoTasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var todoTask = await _context.TodoTasks.FindAsync(id);
        if (todoTask != null)
        {
            _context.TodoTasks.Remove(todoTask);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TodoTaskExists(int id)
    {
        return _context.TodoTasks.Any(e => e.Id == id);
    }
}

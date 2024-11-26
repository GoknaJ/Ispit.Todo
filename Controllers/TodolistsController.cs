using Ispit.Todo.Data;
using Ispit.Todo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Ispit.Todo.Controllers;

[Authorize]
public class TodolistsController : Controller
{
    private readonly ApplicationDbContext _context;

    public TodolistsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Todolists
    public async Task<IActionResult> Index()
    {
        return View(await _context.Todolists.ToListAsync());
    }

    // GET: Todolists/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todolist = await _context.Todolists
            .Include(t => t.TodoTasks)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (todolist == null)
        {
            return NotFound();
        }

        return View(todolist);
    }



    // GET: Todolists/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Todolists/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    public async Task<IActionResult> Create([Bind("Id,Name,UserId,TodoTasks")] Todolist todolist)
    {
        if (ModelState.IsValid)
        {
            if (todolist.TodoTasks != null)
            {
                foreach (var task in todolist.TodoTasks)
                {
                    task.TodolistId = todolist.Id;  // Dodajemo TodolistId za svaki zadatak
                }
            }
            _context.Add(todolist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(todolist);
    }




    // GET: Todolists/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todolist = await _context.Todolists.FindAsync(id);
        if (todolist == null)
        {
            return NotFound();
        }
        return View(todolist);
    }

    // POST: Todolists/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,UserId")] Todolist todolist)
    {
        ModelState.Remove(nameof(todolist.TodoTasks));

        if (id != todolist.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(todolist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodolistExists(todolist.Id))
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
        return View(todolist);
    }

    // GET: Todolists/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todolist = await _context.Todolists
            .FirstOrDefaultAsync(m => m.Id == id);
        if (todolist == null)
        {
            return NotFound();
        }

        return View(todolist);
    }

    // POST: Todolists/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var todolist = await _context.Todolists.FindAsync(id);
        if (todolist != null)
        {
            _context.Todolists.Remove(todolist);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TodolistExists(int id)
    {
        return _context.Todolists.Any(e => e.Id == id);
    }
}

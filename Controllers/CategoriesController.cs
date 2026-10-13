using Categories.Data;
using Categories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Categories.Controllers;

public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var category = await _context.Categories.Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id);
        return category == null ? NotFound() : View(category);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,DisplayOrder")] Category category)
    {
        if (!ModelState.IsValid) return View(category);
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var category = await _context.Categories.FindAsync(id);
        return category == null ? NotFound() : View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DisplayOrder")] Category category)
    {
        if (id != category.Id) return NotFound();
        if (!ModelState.IsValid) return View(category);
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var category = await _context.Categories.FindAsync(id);
        return category == null ? NotFound() : View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return NotFound();
        if (await _context.Products.AnyAsync(p => p.IdCategory == id))
        {
            ModelState.AddModelError("", "Категорията има продукти и не може да се изтрие.");
            return View(category);
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

using Categories.Data;
using Categories.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Categories.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId)
    {
        var products = _context.Products.Include(p => p.Category).AsQueryable();
        if (categoryId != null) products = products.Where(p => p.IdCategory == categoryId);
        return View(await products.OrderBy(p => p.Name).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var product = await _context.Products.Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product == null ? NotFound() : View(product);
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategories();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,IdCategory")] Product product)
    {
        if (!await _context.Categories.AnyAsync(c => c.Id == product.IdCategory))
            ModelState.AddModelError("IdCategory", "Изберете категория.");
        if (!ModelState.IsValid)
        {
            await LoadCategories(product.IdCategory);
            return View(product);
        }
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        await LoadCategories(product.IdCategory);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IdCategory")] Product product)
    {
        if (id != product.Id) return NotFound();
        if (!await _context.Categories.AnyAsync(c => c.Id == product.IdCategory))
            ModelState.AddModelError("IdCategory", "Изберете категория.");
        if (!ModelState.IsValid)
        {
            await LoadCategories(product.IdCategory);
            return View(product);
        }
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var product = await _context.Products.Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        return product == null ? NotFound() : View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategories(int? selected = null)
    {
        ViewBag.Categories = new SelectList(
            await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync(),
            "Id", "Name", selected);
    }
}

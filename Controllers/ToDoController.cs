using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using todoHW.Data;
using todoHW.Models;

namespace todoHW.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CetUser> _userManager;

        public ToDoController(ApplicationDbContext context, UserManager<CetUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ToDo
        public async Task<IActionResult> Index(SearchViewModel searchModel)
        {

            /* var applicationDbContext2 = _context.TodoItems.Include(t => t.Category)
                 .Where(t => showall || !t.IsCompleted ).OrderBy(t => t.DueDate);
            */
            var CetUser = await _userManager.GetUserAsync(HttpContext.User);
            var query = _context.ToDo.Include(t => t.Category).Where(t => t.CetUserId == CetUser.Id); // select * from TodoItems t inner join Categories c on t.CategoryId=c.Id

            if (!searchModel.ShowAll)
            {
                query = query.Where(t => !t.isCompleted); // where t.Iscompleted=0
            }
            if (!String.IsNullOrWhiteSpace(searchModel.SearchText))
            {
                query = query.Where(t => t.Title.Contains(searchModel.SearchText)); // where t.Title like '%serchtext%'
            }

            query = query.OrderBy(t => t.DueDate); // order by DueDate
            searchModel.Result = await query.ToListAsync();

            return View(searchModel);
            /*var cetUser = await _userManager.GetUserAsync(HttpContext.User);

            var applicationDbContext = _context.ToDo.Include(t => t.Category);
            //.Where(t=> t.isCompleted == false).OrderBy(t=>t.RemainingHour)
            return View(await applicationDbContext.ToListAsync());*/
        }

        // GET: ToDo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDo
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // GET: ToDo/Create
        public IActionResult Create()
        {
            ViewBag.CategorySelectList = new SelectList(_context.Categories, "ID", "Name");
            return View();
        }

        // POST: ToDo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,isCompleted,DueDate,CreatedDate,CategoryID")] ToDo toDo)
        {


            var cetUser = await _userManager.GetUserAsync(HttpContext.User);

            toDo.CetUserId = cetUser.Id;

            if (ModelState.IsValid)
            {
                _context.Add(toDo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", toDo.CategoryID);
            return View(toDo);
        }

        // GET: ToDo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDo.FindAsync(id);
            if (toDo == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", toDo.CategoryID);
            return View(toDo);
        }

        // POST: ToDo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,isCompleted,DueDate,CreatedDate,CategoryID")] ToDo toDo)
        {
            if (id != toDo.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoExists(toDo.ID))
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
            ViewData["CategoryID"] = new SelectList(_context.Categories, "ID", "Name", toDo.CategoryID);
            return View(toDo);
        }

        // GET: ToDo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDo = await _context.ToDo
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (toDo == null)
            {
                return NotFound();
            }

            return View(toDo);
        }

        // POST: ToDo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDo = await _context.ToDo.FindAsync(id);
            _context.ToDo.Remove(toDo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> MakeComplete(int ID)
        {
            var todoItemItem = _context.ToDo.FirstOrDefault(t=>t.ID==ID);
            if(todoItemItem == null)
            {
                return NotFound();
            }
            todoItemItem.isCompleted = true;
            todoItemItem.CompletedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MakeIncomplete(int ID)
        {
            var todoItemItem = _context.ToDo.FirstOrDefault(t => t.ID == ID);
            if (todoItemItem == null)
            {
                return NotFound();
            }
            todoItemItem.isCompleted = false;
            todoItemItem.CompletedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ToDoExists(int id)
        {
            return _context.ToDo.Any(e => e.ID == id);
        }
    }
}

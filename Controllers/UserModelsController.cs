﻿using LearnAPI.AppDbContext;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [ApiController]
    public class UserModelsController : Controller
    {
        private readonly BookNookDbContext _context;

        public UserModelsController(BookNookDbContext context)
        {
            _context = context;
        }

        // GET: UserModels
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Users.ToListAsync());
        //}

        //// GET: UserModels/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userModel = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userModel);
        //}

        //// GET: UserModels/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: UserModels/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Username,Email,PasswordHash,Role,FirstName,LastName,PhoneNumber,DateCreated,IsActive")] UserModel userModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(userModel);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(userModel);
        //}

        //// GET: UserModels/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userModel = await _context.Users.FindAsync(id);
        //    if (userModel == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(userModel);
        //}

        //// POST: UserModels/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Email,PasswordHash,Role,FirstName,LastName,PhoneNumber,DateCreated,IsActive")] UserModel userModel)
        //{
        //    if (id != userModel.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(userModel);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!UserModelExists(userModel.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(userModel);
        //}

        //// GET: UserModels/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var userModel = await _context.Users
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (userModel == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(userModel);
        //}

        //// POST: UserModels/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var userModel = await _context.Users.FindAsync(id);
        //    if (userModel != null)
        //    {
        //        _context.Users.Remove(userModel);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool UserModelExists(int id)
        //{
        //    return _context.Users.Any(e => e.Id == id);
        //}
    }
}

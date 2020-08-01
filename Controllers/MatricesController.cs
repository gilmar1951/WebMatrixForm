using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatrixForm.Data;
using MatrixForm.Models;
using MatrixForm.Services;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

namespace MatrixForm.Controllers
{
    public class MatricesController : Controller
    {
        private readonly MatrixFormContext _context;
        private readonly MatrixService _matrixService;

        public MatricesController(MatrixFormContext context, MatrixService matrixService)
        {
            _context = context;
            _matrixService = matrixService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _matrixService.FindAllAsync();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Matrix obj)
        {
            if (!ModelState.IsValid)
            {
                var departaments = await _matrixService.FindAllAsync();
                return View();
            }
            await _matrixService.InsertAsync(obj);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _matrixService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _matrixService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _matrixService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _matrixService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Matrix obj)
        {
            if (!ModelState.IsValid)
            {
                var departaments = await _matrixService.FindAllAsync();
                return View();
            }

            if (id != obj.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }
            try
            {
                await _matrixService.UpdateAsync(obj);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public async Task<IActionResult> MoveUp(Matrix obj)
                {
                    if (null == obj)
                    {
                        return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
                    }
                    try
                    {
                        await _matrixService.MoveUpAsync(obj);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (NotFoundException e)
                    {
                        return RedirectToAction(nameof(Error), new { message = e.Message });
                    }
                    catch (DbConcurrencyException e)
                    {
                        return RedirectToAction(nameof(Error), new { message = e.Message });
                    }
                }*/

/*        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MoveUp(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _matrixService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            await _matrixService.MoveUpAsync(obj);
            return RedirectToAction(nameof(Index));
        }*/

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier };

            return View(viewModel);
        }
    }
}

﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyKitchen.Data;
using MyKitchen.Models;
using MyKitchen.Models.FoodItems;
using System.Security.Claims;
namespace MyKitchen.Controllers
{

    //TODO this is experiment to facilitate unit testing based on https://www.jerriepelser.com/blog/resolve-dbcontext-as-interface-in-aspnet5-ioc-container/


    [Authorize]
    public class FoodItemsController : Controller
    {

        private IFoodItemRepository repository { get; set; }
        private IMyKitchenDataContext ctx { get; set; }
        private readonly ILogger _logger;

        
        public int PageSize = 10;

        public FoodItemsController(IFoodItemRepository repo, IMyKitchenDataContext context,ILogger<FoodItemsController> logger,IHttpContextAccessor )
        {
            ctx = context;
            repository = repo;
            _logger = logger;
        }

        // GET: FoodItems
        public IActionResult Index(int currentPage = 1)
        {
            var viewModel = new FoodItemIndexViewModel()
            {
                FoodItems = repository.FoodItems.OrderBy(x => x.FoodItemName).Skip((currentPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = currentPage,ItemsPerPage = PageSize, TotalItems = repository.FoodItems.Count() },
   
            };


            return View(viewModel);
        }

        // GET: FoodItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await repository.FoodItems
                .FirstOrDefaultAsync(m => m.FoodItemID == id);
            if (foodItem == null)
            {
                return NotFound();
            }

            var viewModel = new FoodItemDetailsViewModel()
            {
                FoodItem = foodItem
            };

            return View(viewModel);
        }

        // GET: FoodItems/Create
        public IActionResult Create()
        {
            var viewModel = new FoodItemCreateViewModel()
            {
                FoodItem = new FoodItem(),
                FoodGroups = ctx.FoodGroups.AsEnumerable()

            };

            return View(viewModel);
        }

        // POST: FoodItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FoodItemID,FoodItemName,FoddDescription,Cost,FoodGroupID")] FoodItem foodItem)
        {
            if (ModelState.IsValid)
            {
                await repository.Add(foodItem);
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new FoodItemCreateViewModel()
            {
                FoodGroups = ctx.FoodGroups.AsEnumerable(),
                FoodItem = new FoodItem()
            };


            return View(viewModel);
        }

        // GET: FoodItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await repository.Find(id.Value);
            if (foodItem == null)
            {
                return NotFound();
            }

            var viewModel = new FoodItemEditViewModel()
            {
                FoodGroups = ctx.FoodGroups.AsEnumerable(),
                FoodItem = foodItem
            };

            return View(viewModel);
        }

        // POST: FoodItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FoodItem foodItem)
        {
            if (id != foodItem.FoodItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    repository.Update(foodItem);
                    await repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodItemExists(foodItem.FoodItemID))
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


            var viewModel = new FoodItemEditViewModel()
            {
                FoodItem = foodItem,
                FoodGroups = ctx.FoodGroups
            };




            return View(viewModel);
        }

        // GET: FoodItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var foodItem = await repository.FoodItems
                .FirstOrDefaultAsync(m => m.FoodItemID == id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        // POST: FoodItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var foodItem = await repository.Find(id);
            repository.Remove(foodItem);
            await repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodItemExists(int id)
        {
            return repository.FoodItems.Any(e => e.FoodItemID == id);
        }
    }
}

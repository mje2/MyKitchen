using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyKitchen.Data;
using MyKitchen.Models;
using System.Threading.Tasks;
using MyKitchen.Models.Meals;
using MyKitchen.BL;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyKitchen.Controllers
{
    [Authorize]
    public class MealBuilderController : Controller
    {
        private readonly IFoodItemRepository foodItemRepository;
        private readonly IMealRepository mealRepository;

        UserInfo AppUser {get; set;}

        public MealBuilderController(IFoodItemRepository foodItemRepo,IMealRepository mealRepo,UserInfo user)
        {
            AppUser = user;
            foodItemRepository = foodItemRepo;
            mealRepository = mealRepo;
        }

        // GET: /<controller>/
        public IActionResult Index(int mealListPage = 1)
        {
            var viewModel1 = new MealBuilderIndexViewModel()
            {
                Meals = mealRepository.GetMeals(),
                MealListPagingInfo = new PagingInfo() { CurrentPage = mealListPage,ItemsPerPage = 15,TotalItems = mealRepository.Count()}
            };

            return View(viewModel1);
        }

        public IActionResult Create()
        {
            var mealFactory = new MealFactory();
            Meal meal = mealFactory.NewMeal();

            var viewModel = new MealBuilderCreateViewModel()
            {
                FoodItems = foodItemRepository.GetFoodItems()
            };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNewMeal(MealBuilderCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                await mealRepository.Add(model.Meal);

            }

            return RedirectToAction("Index");

        }

        public IActionResult SelectFoodItemsForMeal(int mealId,int currentPage = 1)
        {

            var PageSize = 10;

            var meal = mealRepository.Find(mealId);
            var viewModel = new MealBuilderSelectFoodItemsViewModel()
            {
                FoodItems = foodItemRepository.GetFoodItemsForUser(this.AppUser.User).OrderBy(x => x.FoodItemName).Skip((currentPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = currentPage, ItemsPerPage = PageSize, TotalItems = foodItemRepository.GetFoodItemsForUser(this.AppUser.User).Count() },
                TheMeal = meal
                
            };

            return View(viewModel);
        }

        public IActionResult AddToMeal(int currentPage,int mealId,int id)
        {
            var meal = mealRepository.Find(mealId);
            FoodItem foodItem = foodItemRepository.Find(id).GetAwaiter().GetResult();

            if (meal.ContainsFoodItem(foodItem.FoodItemID))
            {
                ModelState.AddModelError(string.Empty,"This Food Item has already been added to this meal");
            }
            else
            {
                meal.AddFoodItemToMeal(foodItem.FoodItemID);
                mealRepository.SaveChanges();
                ViewBag.Message =  "Food Item Added to Meal.";
            }

            var PageSize = 10;

            var viewModel = new MealBuilderSelectFoodItemsViewModel()
            {
                FoodItems = foodItemRepository.GetFoodItemsForUser(AppUser.User).OrderBy(x => x.FoodItemName).Skip((currentPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo { CurrentPage = currentPage, ItemsPerPage = PageSize, TotalItems = foodItemRepository.GetFoodItemsForUser(AppUser.User).Count() },
                TheMeal = meal

            };

            return View("SelectFoodItemsForMeal", viewModel);
        }

        public IActionResult MealDetails(int mealID)
        {
            var meal = mealRepository.Find(mealID);
            var viewModel = new MealBuilderMealDetails_VM();
            viewModel.Meal = meal;

            return View(viewModel);

        }

        public IActionResult DeleteMeal(int mealid)
        {
            var meal = mealRepository.Find(mealid);
            mealRepository.Remove(meal);
            mealRepository.SaveChanges();

            //TempData[""]
            var viewModel1 = new MealBuilderIndexViewModel()
            {
                Meals = mealRepository.GetMeals(),
                MealListPagingInfo = new PagingInfo() { CurrentPage = 1, ItemsPerPage = 15, TotalItems = mealRepository.Count() }
            };

            return View("Index",viewModel1);
        }

        public IActionResult Edit()
        {
            throw new System.NotImplementedException();
        }
    }
}

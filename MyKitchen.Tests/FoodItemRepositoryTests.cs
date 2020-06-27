using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyKitchen.Data;
using MyKitchen.Models;
using Xunit;

namespace MyKitchen.Tests
{
    //Tests of this Repository also test the SQL Server database and Entity Framework Entity classes
    public class FoodItemRepositoryTests:IClassFixture<SharedTestContext>
    {
        public SharedTestContext Fixture { get; private set; }

        public FoodItemRepositoryTests(SharedTestContext fixture)
        {
            this.Fixture = fixture;
        }


        [Fact]
        public void Add_Test()
        {
            //dependencies
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<EFFoodItemRepository>();
            //end dependencies

             WithTestDatabase.Run((ApplicationDbContext context) => {

                var repo = new EFFoodItemRepository(context,logger);
                var datetime = DateTime.Now.ToString();
                var foodItemName = "CanAddFoodItem " + datetime; 

                var newFoodItem = new FoodItem(){
                    FoodItemName = "CanAddFoodItem " + datetime,
                    FoodDescription = "Test Description"
                };

                repo.Add(newFoodItem).Wait();;

                Assert.True(context.FoodItems.Any(x => x.FoodItemName == foodItemName));

            });

            //make sure item has been added.
        }

        [Fact]
        public void AddFoodForUser_Test()
        {
            var rand = new System.Random(DateTime.Now.Millisecond);
            var user = this.Fixture.TestUserManager.Users.FirstOrDefault();

            if(user == null)
            {
                //Fail
                Assert.True(false);
            }

            var name = $"Food_Item_For_User_Test_{user.Id}-{rand}";

            var newFoodItem = new FoodItem(){
                FoodItemName = name,
                FoodDescription = "Test Description"
            };

            Fixture.TestFoodItemRepository.Add(newFoodItem).Wait();
            Fixture.TestFoodItemRepository.AddFoodForUser(user,newFoodItem).Wait();;

            Assert.True(Fixture.ApDbContext.FoodItems.Any(x => x.FoodItemName == name));
            Assert.True(Fixture.ApDbContext.UserFoodItems.Include(x => x.AppUser).Any(x => x.AppUser.Id == user.Id && x.FoodItemID == newFoodItem.FoodItemID));
        }

        [Fact]
        public void Find_Test()
        {
            var item = Fixture.ApDbContext.FoodItems.FirstOrDefault();
            if(item == null) {Assert.True(false,"no food items available for testing");}

            var foundItem = Fixture.TestFoodItemRepository.Find(item.FoodItemID).GetAwaiter().GetResult();
            if(foundItem != null){

                Assert.True(true, $"found {item.FoodItemName}");
            }
            else{

                Assert.True(false,"could not find food item");
            }

        }

        [Fact]
        public void Update_Test()
        {
            //attempt to update the description of a the first item.
            var foodItem = Fixture.ApDbContext.FoodItems.FirstOrDefault();
            Assert.NotNull(foodItem);

            var changedDescription = "New_DESC_" + Guid.NewGuid();
            var testFoodItemId = foodItem.FoodItemID;

            foodItem.FoodDescription = changedDescription;
            Fixture.TestFoodItemRepository.Update(foodItem);

            //Assert
            var changedFoodItem = Fixture.ApDbContext.FoodItems.FirstOrDefault(x => x.FoodItemID == testFoodItemId);
            Assert.NotNull(changedFoodItem);

            Assert.True(changedFoodItem.FoodDescription == changedDescription);

        }

        [Fact(Skip = "In Development")]
        public void GetRandomItemTest()
        {
        }

        [Fact]
        public void CanDeleteFoodItem()
        {

            var item = Fixture.ApDbContext.FoodItems.FirstOrDefault(x => x.FoodItemName.Contains("Food_Item_For_User"));
            if(item == null) Assert.True(false);

            //delete item
            Fixture.TestFoodItemRepository.Remove(item);
        }






    }

}
using MyKitchen.Data;
using Xunit;
using System;
using System.Linq;

namespace MyKitchen.Tests
{

    public class RegistrationTests :IClassFixture<SharedTestContext>{



            SharedTestContext Fixture;

            public RegistrationTests(SharedTestContext fixture)
            {
                this.Fixture = fixture;
            }


            [Fact]
            public void RegisterNewUser1()
            {
                //append guid so test coan be repeated
                var guid = Guid.NewGuid().ToString();
                var newUser = new ApplicationUser(){Email = $"{guid}@test.com",UserName = $"{guid}@test.com"};
                var result =  Fixture.TestUserManager.CreateAsync(newUser, "aA11111!").GetAwaiter().GetResult();

                Assert.True(result.Succeeded);

            }

            [Fact]
            public void RegisterNewUser2()
            {
                //append guid so test coan be repeated
                var guid = Guid.NewGuid().ToString();
                var newUser = new ApplicationUser(){Email = $"{guid}@test.com", UserName=$"{guid}@test.com"};
                var result =  Fixture.TestUserManager.CreateAsync(newUser, "aA11111!").GetAwaiter().GetResult();

                Assert.True(result.Succeeded);


            }

            [Fact]
            public void FoodItemsAreUserIsolated()
            {

                var guid1 = Guid.NewGuid();
                var guid2 = Guid.NewGuid();

                Assert.True(true);
                if(Fixture.ApDbContext.Users.Count() < 2){
                    Assert.True(false, "Inconclusive - need 2 users in DB");
                }

                var users = Fixture.ApDbContext.Users.Take(2).ToList();

                //get user 1
                //get user 2
                var user1 = Fixture.TestUserManager.FindByIdAsync(users[0].Id).GetAwaiter().GetResult();
                var user2 = Fixture.TestUserManager.FindByIdAsync(users[1].Id).GetAwaiter().GetResult();;

                //food item 1
                //food item 2
                var foodItem1 = new FoodItem(){
                    FoodItemName = guid1.ToString(),
                    FoodDescription = "Test Item 3" + guid1
                };

                var foodItem2 = new FoodItem(){
                    FoodItemName = guid2.ToString(),
                    FoodDescription = "Test Item 3" + guid2
                };

                //add food item for user 1
                Fixture.TestFoodItemRepository.Add(foodItem1).GetAwaiter().GetResult();
                Fixture.TestFoodItemRepository.AddFoodForUser(user1,foodItem1).GetAwaiter().GetResult();

                //get food items for user 2
                Fixture.TestFoodItemRepository.Add(foodItem2).GetAwaiter().GetResult();;
                Fixture.TestFoodItemRepository.AddFoodForUser(user2,foodItem2).GetAwaiter().GetResult();

                //Assert
                Assert.True(Fixture.ApDbContext.FoodItems.Any(x => x.FoodItemID == foodItem1.FoodItemID));
                Assert.True(Fixture.ApDbContext.FoodItems.Any(x => x.FoodItemID == foodItem2.FoodItemID));

                //Assert User Food Items
                Assert.True(Fixture.ApDbContext.UserFoodItems.Any(x => x.FoodItemID == foodItem1.FoodItemID && x.AppUser.Id == user1.Id));
                Assert.True(Fixture.ApDbContext.UserFoodItems.Any(x => x.FoodItemID == foodItem2.FoodItemID && x.AppUser.Id == user2.Id));
    }
}
}

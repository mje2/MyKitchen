namespace MyKitchen.Data
{
    public class MealFactory
    {
        public MealFactory(ApplicationDbContext ctx) { }
        public Meal NewMeal()
        {
            var name = "Satisfying Meal 1";
            var rtn = new Meal() {MealName = name};
            return rtn;
        }
    }

}

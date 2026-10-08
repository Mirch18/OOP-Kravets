namespace Lab4v12;

public class Food
{
    private string _name = string.Empty;
    private int _calories;

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public int Calories
    {
        get => _calories;
        set => _calories = value;
    }

    public Food(string name, int calories)
    {
        Name = name;
        Calories = calories;
    }

    public virtual void Eat()
    {
        Console.WriteLine($"Їжа \"{Name}\" споживається. Калорійність: {Calories} ккал.");
    }

    public string GetFoodType()
    {
        return "Їжа";
    }
}

public class Fruit : Food
{
    private int _sweetnessLevel;

    public int SweetnessLevel
    {
        get => _sweetnessLevel;
        set => _sweetnessLevel = value;
    }

    public Fruit(string name, int calories, int sweetnessLevel)
        : base(name, calories)
    {
        SweetnessLevel = sweetnessLevel;
    }

    public override void Eat()
    {
        Console.WriteLine(
            $"Фрукт \"{Name}\" споживається. Калорійність: {Calories} ккал, " +
            $"рівень солодкості: {SweetnessLevel}/10.");
    }

    public void Peel()
    {
        Console.WriteLine($"Фрукт \"{Name}\" очищено від шкірки.");
    }

    public new string GetFoodType()
    {
        return "Фрукт";
    }
}

public class Vegetable : Food
{
    private bool _isLeafy;

    public bool IsLeafy
    {
        get => _isLeafy;
        set => _isLeafy = value;
    }

    public Vegetable(string name, int calories, bool isLeafy)
        : base(name, calories)
    {
        IsLeafy = isLeafy;
    }

    public override void Eat()
    {
        string leafyDescription = IsLeafy ? "листовий" : "не листовий";
        Console.WriteLine(
            $"Овоч \"{Name}\" споживається. Калорійність: {Calories} ккал, " +
            $"тип: {leafyDescription}.");
    }

    public void Chop()
    {
        Console.WriteLine($"Овоч \"{Name}\" нарізано.");
    }
}

public static class Program
{
    public static void Main()
    {
        Food food = new Food("Хліб", 265);
        Food fruitAsFood = new Fruit("Яблуко", 52, 8);
        Food vegetableAsFood = new Vegetable("Шпинат", 23, true);

        Console.WriteLine("Демонстрація поліморфізму:");
        food.Eat();
        fruitAsFood.Eat();
        vegetableAsFood.Eat();

        Fruit fruit = new Fruit("Банан", 89, 9);
        Vegetable vegetable = new Vegetable("Морква", 41, false);

        Console.WriteLine();
        Console.WriteLine("Унікальні методи похідних класів:");
        fruit.Peel();
        vegetable.Chop();

        Console.WriteLine();
        Console.WriteLine("Демонстрація приховування методу за допомогою new:");
        Console.WriteLine($"Food: {fruitAsFood.GetFoodType()}");
        Console.WriteLine($"Fruit: {fruit.GetFoodType()}");
    }
}

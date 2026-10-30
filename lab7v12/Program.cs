namespace Lab7v12;

public static class Program
{
    public static void Main()
    {
        IRepository<Car> repository = new InMemoryRepository<Car>();

        Console.WriteLine("1. Додавання автомобілів (Add):");
        repository.Add(new Car(1, "Toyota", "Camry", 2020));
        repository.Add(new Car(2, "BMW", "X5", 2018));
        repository.Add(new Car(3, "Tesla", "Model 3", 2022));
        PrintAll(repository);

        Console.WriteLine();
        Console.WriteLine("2. Отримання автомобіля за Id = 2 (GetById):");
        Console.WriteLine(repository.GetById(2));

        Console.WriteLine();
        Console.WriteLine("3. Оновлення автомобіля з Id = 1 (Update):");
        repository.Update(new Car(1, "Toyota", "Corolla", 2021));
        Console.WriteLine(repository.GetById(1));

        Console.WriteLine();
        Console.WriteLine("4. Видалення автомобіля з Id = 3 (Delete):");
        repository.Delete(3);
        PrintAll(repository);

        Console.WriteLine();
        Console.WriteLine("5. Обробка помилкових ситуацій:");
        TryExecute(() => repository.GetById(10));
        TryExecute(() => repository.Update(new Car(10, "Audi", "A4", 2019)));
        TryExecute(() => repository.Delete(10));
        TryExecute(() => repository.Add(new Car(1, "Honda", "Civic", 2017)));
    }

    private static void PrintAll(IRepository<Car> repository)
    {
        foreach (Car car in repository.GetAll())
        {
            Console.WriteLine($"  {car}");
        }
    }

    private static void TryExecute(Action action)
    {
        try
        {
            action();
        }
        catch (KeyNotFoundException ex)
        {
            Console.WriteLine($"  Помилка: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Помилка: {ex.Message}");
        }
    }
}

namespace Lab8v12;

public static class CollectionHelper<T>
{
    public static IEnumerable<T> OrderBy<TKey>(IEnumerable<T> collection, Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keySelector);

        List<T> items = new List<T>(collection);
        List<TKey> keys = new List<TKey>(items.Count);

        foreach (T item in items)
        {
            keys.Add(keySelector(item));
        }

        for (int i = 1; i < items.Count; i++)
        {
            T currentItem = items[i];
            TKey currentKey = keys[i];
            int j = i - 1;

            while (j >= 0 && CompareKeys(keys[j], currentKey) > 0)
            {
                items[j + 1] = items[j];
                keys[j + 1] = keys[j];
                j--;
            }

            items[j + 1] = currentItem;
            keys[j + 1] = currentKey;
        }

        return items;
    }

    private static int CompareKeys<TKey>(TKey first, TKey second)
        where TKey : IComparable<TKey>
    {
        if (first is null && second is null)
        {
            return 0;
        }

        if (first is null)
        {
            return -1;
        }

        if (second is null)
        {
            return 1;
        }

        return first.CompareTo(second);
    }
}

public class Product
{
    public string Name { get; }

    public string Category { get; }

    public decimal Price { get; }

    public Product(string name, string category, decimal price)
    {
        Name = name;
        Category = category;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} ({Category}) - {Price:0.00} грн";
    }
}

public static class Program
{
    public static void Main()
    {
        List<string> words = ["кіт", "бегемот", "слон", "їжак", "жираф", "ведмідь", "лев"];

        Console.WriteLine("1. List<string>: сортування за довжиною рядка");
        PrintInline("Початковий список", words);
        IEnumerable<string> wordsByLength = CollectionHelper<string>.OrderBy(words, word => word.Length);
        PrintInline("Відсортовано (word => word.Length)", wordsByLength);
        PrintInline("Початковий список не змінився", words);

        Console.WriteLine();
        Console.WriteLine("2. List<Product>: сортування за ціною та за назвою");
        List<Product> products =
        [
            new Product("Ноутбук", "Електроніка", 32999.99m),
            new Product("Чашка", "Посуд", 149.50m),
            new Product("Навушники", "Електроніка", 1899.00m),
            new Product("Книга", "Книги", 349.00m),
            new Product("Сковорода", "Посуд", 899.00m)
        ];

        PrintList("Початковий список", products);
        IEnumerable<Product> productsByPrice = CollectionHelper<Product>.OrderBy(products, product => product.Price);
        PrintList("Відсортовано за ціною (product => product.Price)", productsByPrice);
        IEnumerable<Product> productsByName = CollectionHelper<Product>.OrderBy(products, product => product.Name);
        PrintList("Відсортовано за назвою (product => product.Name)", productsByName);

        Console.WriteLine();
        Console.WriteLine("3. List<int>: сортування за значенням і за модулем");
        List<int> numbers = [42, -7, 15, 0, 8, -30, 3];
        PrintInline("Початковий список", numbers);
        PrintInline("За значенням (number => number)", CollectionHelper<int>.OrderBy(numbers, number => number));
        PrintInline("За модулем (number => Math.Abs(number))", CollectionHelper<int>.OrderBy(numbers, number => Math.Abs(number)));

        Console.WriteLine();
        Console.WriteLine("4. Граничний випадок: порожня колекція");
        List<int> emptyList = [];
        IEnumerable<int> sortedEmpty = CollectionHelper<int>.OrderBy(emptyList, number => number);
        Console.WriteLine($"Кількість елементів після сортування: {sortedEmpty.Count()}");
    }

    private static void PrintInline<TItem>(string title, IEnumerable<TItem> items)
    {
        Console.WriteLine($"{title}: {string.Join(", ", items)}");
    }

    private static void PrintList<TItem>(string title, IEnumerable<TItem> items)
    {
        Console.WriteLine($"{title}:");
        foreach (TItem item in items)
        {
            Console.WriteLine($"  - {item}");
        }
    }
}

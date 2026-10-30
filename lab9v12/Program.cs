using System.Diagnostics;

namespace Lab9v12;

public class ImageFile
{
    public string FileName { get; }

    public int SizeKb { get; }

    public ImageFile(string fileName, int sizeKb)
    {
        FileName = fileName;
        SizeKb = sizeKb;
    }

    public override string ToString()
    {
        return $"{FileName} ({SizeKb} КБ)";
    }
}

public class ImageProcessingSystem
{
    private readonly HashSet<string> _uniqueFileNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    private readonly Queue<ImageFile> _processingQueue = new Queue<ImageFile>();
    private readonly Stack<string> _filterHistory = new Stack<string>();

    public int UniqueFileCount => _uniqueFileNames.Count;

    public int QueueCount => _processingQueue.Count;

    public int FilterHistoryCount => _filterHistory.Count;

    public bool RegisterImage(ImageFile image)
    {
        if (!_uniqueFileNames.Add(image.FileName))
        {
            return false;
        }

        _processingQueue.Enqueue(image);
        return true;
    }

    public bool IsRegistered(string fileName)
    {
        return _uniqueFileNames.Contains(fileName);
    }

    public bool RemoveFileName(string fileName)
    {
        return _uniqueFileNames.Remove(fileName);
    }

    public IEnumerable<string> GetFileNames()
    {
        return _uniqueFileNames;
    }

    public HashSet<string> FindAlreadyArchived(IEnumerable<string> archiveFileNames)
    {
        HashSet<string> common = new HashSet<string>(_uniqueFileNames, StringComparer.OrdinalIgnoreCase);
        common.IntersectWith(archiveFileNames);
        return common;
    }

    public ImageFile? PeekNextImage()
    {
        return _processingQueue.TryPeek(out ImageFile? image) ? image : null;
    }

    public ImageFile ProcessNextImage()
    {
        if (_processingQueue.Count == 0)
        {
            throw new InvalidOperationException("Черга зображень порожня.");
        }

        return _processingQueue.Dequeue();
    }

    public void ApplyFilter(ImageFile image, string filterName)
    {
        _filterHistory.Push($"{image.FileName}: {filterName}");
    }

    public string? PeekLastFilter()
    {
        return _filterHistory.TryPeek(out string? filter) ? filter : null;
    }

    public string? UndoLastFilter()
    {
        return _filterHistory.TryPop(out string? filter) ? filter : null;
    }

    public IEnumerable<string> GetFilterHistory()
    {
        return _filterHistory;
    }
}

public static class Program
{
    public static void Main()
    {
        ImageProcessingSystem system = new ImageProcessingSystem();

        Console.WriteLine("=== 1. HashSet<string>: унікальні назви файлів ===");
        ImageFile[] uploadedImages =
        [
            new ImageFile("sunset.jpg", 2048),
            new ImageFile("portrait.png", 1536),
            new ImageFile("Sunset.JPG", 2048),
            new ImageFile("logo.svg", 64),
            new ImageFile("portrait.png", 1536),
            new ImageFile("mountains.jpg", 3072)
        ];

        foreach (ImageFile image in uploadedImages)
        {
            bool added = system.RegisterImage(image);
            string status = added ? "додано" : "відхилено (дублікат)";
            Console.WriteLine($"  Add(\"{image.FileName}\") -> {status}");
        }

        Console.WriteLine($"Унікальних файлів: {system.UniqueFileCount} з {uploadedImages.Length} завантажених");
        Console.WriteLine($"Список: {string.Join(", ", system.GetFileNames())}");
        Console.WriteLine($"Contains(\"logo.svg\") -> {system.IsRegistered("logo.svg")}");
        Console.WriteLine($"Contains(\"cat.gif\") -> {system.IsRegistered("cat.gif")}");

        string[] archive = ["mountains.jpg", "beach.jpg", "SUNSET.jpg"];
        HashSet<string> alreadyArchived = system.FindAlreadyArchived(archive);
        Console.WriteLine($"IntersectWith з архівом: вже є в архіві -> {string.Join(", ", alreadyArchived)}");

        Console.WriteLine();
        Console.WriteLine("=== 2. Queue<ImageFile>: черга зображень на обробку (FIFO) ===");
        Console.WriteLine($"Зображень у черзі: {system.QueueCount}");
        Console.WriteLine($"Peek() -> наступне зображення: {system.PeekNextImage()}");

        string[] filters = ["Яскравість +20%", "Чорно-білий", "Різкість"];
        int step = 1;

        while (system.QueueCount > 0)
        {
            ImageFile image = system.ProcessNextImage();
            string filter = filters[(step - 1) % filters.Length];
            system.ApplyFilter(image, filter);
            Console.WriteLine($"  {step}. Dequeue() -> оброблено {image}, фільтр \"{filter}\". Залишилось у черзі: {system.QueueCount}");
            step++;
        }

        Console.WriteLine($"Peek() для порожньої черги -> {system.PeekNextImage()?.ToString() ?? "черга порожня"}");

        Console.WriteLine();
        Console.WriteLine("=== 3. Stack<string>: історія фільтрів (Undo, LIFO) ===");
        PrintFilterHistory(system);
        Console.WriteLine($"Peek() -> останній фільтр: {system.PeekLastFilter()}");
        Console.WriteLine($"Undo (Pop) -> скасовано: {system.UndoLastFilter()}");
        Console.WriteLine($"Undo (Pop) -> скасовано: {system.UndoLastFilter()}");
        PrintFilterHistory(system);

        Console.WriteLine();
        Console.WriteLine("=== 4. Повторне завантаження файлу (HashSet.Remove) ===");
        Console.WriteLine($"Remove(\"logo.svg\") -> {system.RemoveFileName("logo.svg")}");
        bool reAdded = system.RegisterImage(new ImageFile("logo.svg", 72));
        Console.WriteLine($"Повторний Add(\"logo.svg\") -> {(reAdded ? "додано" : "відхилено")}, у черзі: {system.QueueCount}");

        Console.WriteLine();
        Console.WriteLine("=== 5. Порівняння швидкості Contains(): List<T> та HashSet<T> ===");
        CompareContainsPerformance();
    }

    private static void PrintFilterHistory(ImageProcessingSystem system)
    {
        Console.WriteLine($"Історія фільтрів ({system.FilterHistoryCount}), від останнього до першого:");
        foreach (string filter in system.GetFilterHistory())
        {
            Console.WriteLine($"  - {filter}");
        }
    }

    private static void CompareContainsPerformance()
    {
        const int itemCount = 100_000;
        const int searchCount = 1_000;

        List<string> list = new List<string>(itemCount);
        HashSet<string> set = new HashSet<string>(itemCount);

        for (int i = 0; i < itemCount; i++)
        {
            string fileName = $"image_{i}.jpg";
            list.Add(fileName);
            set.Add(fileName);
        }

        string target = $"image_{itemCount - 1}.jpg";
        bool found = false;

        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < searchCount; i++)
        {
            found = list.Contains(target);
        }
        stopwatch.Stop();
        double listTime = stopwatch.Elapsed.TotalMilliseconds;

        stopwatch.Restart();
        for (int i = 0; i < searchCount; i++)
        {
            found = set.Contains(target);
        }
        stopwatch.Stop();
        double setTime = stopwatch.Elapsed.TotalMilliseconds;

        Console.WriteLine($"Пошук \"{target}\" {searchCount} разів серед {itemCount} назв (знайдено: {found}):");
        Console.WriteLine($"  List<string>.Contains:    {listTime:F2} мс (O(n))");
        Console.WriteLine($"  HashSet<string>.Contains: {setTime:F2} мс (O(1))");
    }
}

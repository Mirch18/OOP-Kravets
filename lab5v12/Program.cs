namespace Lab5v12;

public class MediaFile
{
    public string FileName { get; }

    public MediaFile(string fileName)
    {
        FileName = fileName;
    }

    public virtual string Open()
    {
        return $"Відкрито медіа-файл \"{FileName}\".";
    }
}

public class AudioFile : MediaFile
{
    public int BitRate { get; }

    public AudioFile(string fileName, int bitRate)
        : base(fileName)
    {
        BitRate = bitRate;
    }

    public override string Open()
    {
        return $"Відкрито аудіо-файл \"{FileName}\". Бітрейт: {BitRate} кбіт/с.";
    }
}

public class VideoFile : MediaFile
{
    public string Resolution { get; }

    public VideoFile(string fileName, string resolution)
        : base(fileName)
    {
        Resolution = resolution;
    }

    public override string Open()
    {
        return $"Відкрито відео-файл \"{FileName}\". Роздільна здатність: {Resolution}.";
    }
}

public class ImageFile : MediaFile
{
    public string Dimensions { get; }

    public ImageFile(string fileName, string dimensions)
        : base(fileName)
    {
        Dimensions = dimensions;
    }

    public override string Open()
    {
        return $"Відкрито зображення \"{FileName}\". Розміри: {Dimensions}.";
    }
}

public static class Program
{
    public static void Main()
    {
        List<MediaFile> mediaFiles =
        [
            new AudioFile("podcast.mp3", 320),
            new VideoFile("lecture.mp4", "1920x1080"),
            new ImageFile("diagram.png", "1600x900")
        ];

        List<string> openedFiles = [];

        Console.WriteLine("Демонстрація поліморфних викликів:");
        foreach (MediaFile mediaFile in mediaFiles)
        {
            string result = mediaFile.Open();
            Console.WriteLine(result);
            openedFiles.Add(mediaFile.FileName);
        }

        Console.WriteLine();
        Console.WriteLine("Агрегація: список усіх відкритих медіа-файлів:");
        foreach (string fileName in openedFiles)
        {
            Console.WriteLine($"- {fileName}");
        }

        Console.WriteLine($"Усього відкрито файлів: {openedFiles.Count}");
    }
}

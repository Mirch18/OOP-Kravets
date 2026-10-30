namespace Lab7v12;

public class Car : IEntity
{
    public int Id { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }

    public int Year { get; set; }

    public Car(int id, string brand, string model, int year)
    {
        Id = id;
        Brand = brand;
        Model = model;
        Year = year;
    }

    public override string ToString()
    {
        return $"[Id = {Id}] {Brand} {Model}, {Year} р.";
    }
}

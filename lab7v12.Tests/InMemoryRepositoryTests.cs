namespace Lab7v12.Tests;

public class InMemoryRepositoryTests
{
    private static InMemoryRepository<Car> CreateRepositoryWithCars()
    {
        InMemoryRepository<Car> repository = new InMemoryRepository<Car>();
        repository.Add(new Car(1, "Toyota", "Camry", 2020));
        repository.Add(new Car(2, "BMW", "X5", 2018));
        return repository;
    }

    [Fact]
    public void Add_NewCar_CarIsStored()
    {
        InMemoryRepository<Car> repository = new InMemoryRepository<Car>();
        Car car = new Car(1, "Toyota", "Camry", 2020);

        repository.Add(car);

        Assert.Single(repository.GetAll());
        Assert.Same(car, repository.GetById(1));
    }

    [Fact]
    public void Add_CarWithExistingId_ThrowsInvalidOperationException()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        Assert.Throws<InvalidOperationException>(() => repository.Add(new Car(1, "Audi", "A4", 2019)));
        Assert.Equal(2, repository.GetAll().Count());
    }

    [Fact]
    public void Add_Null_ThrowsArgumentNullException()
    {
        InMemoryRepository<Car> repository = new InMemoryRepository<Car>();

        Assert.Throws<ArgumentNullException>(() => repository.Add(null!));
    }

    [Fact]
    public void GetById_ExistingId_ReturnsCar()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        Car car = repository.GetById(2);

        Assert.Equal("BMW", car.Brand);
        Assert.Equal("X5", car.Model);
        Assert.Equal(2018, car.Year);
    }

    [Fact]
    public void GetById_NonExistingId_ThrowsKeyNotFoundException()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        Assert.Throws<KeyNotFoundException>(() => repository.GetById(99));
    }

    [Fact]
    public void GetAll_RepositoryWithCars_ReturnsAllCars()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        List<Car> cars = repository.GetAll().ToList();

        Assert.Equal(2, cars.Count);
        Assert.Contains(cars, car => car.Id == 1);
        Assert.Contains(cars, car => car.Id == 2);
    }

    [Fact]
    public void GetAll_EmptyRepository_ReturnsEmptyCollection()
    {
        InMemoryRepository<Car> repository = new InMemoryRepository<Car>();

        Assert.Empty(repository.GetAll());
    }

    [Fact]
    public void Update_ExistingCar_UpdatesData()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        repository.Update(new Car(1, "Toyota", "Corolla", 2021));

        Car updatedCar = repository.GetById(1);
        Assert.Equal("Corolla", updatedCar.Model);
        Assert.Equal(2021, updatedCar.Year);
        Assert.Equal(2, repository.GetAll().Count());
    }

    [Fact]
    public void Update_NonExistingCar_ThrowsKeyNotFoundException()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        Assert.Throws<KeyNotFoundException>(() => repository.Update(new Car(99, "Audi", "A4", 2019)));
    }

    [Fact]
    public void Delete_ExistingId_RemovesCar()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        repository.Delete(1);

        Assert.Single(repository.GetAll());
        Assert.Throws<KeyNotFoundException>(() => repository.GetById(1));
    }

    [Fact]
    public void Delete_NonExistingId_ThrowsKeyNotFoundException()
    {
        InMemoryRepository<Car> repository = CreateRepositoryWithCars();

        Assert.Throws<KeyNotFoundException>(() => repository.Delete(99));
        Assert.Equal(2, repository.GetAll().Count());
    }
}

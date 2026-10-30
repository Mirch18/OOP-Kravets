namespace Lab7v12;

public class InMemoryRepository<T> : IRepository<T> where T : IEntity
{
    private readonly List<T> _items = [];

    public T GetById(int id)
    {
        int index = FindIndex(id);
        if (index == -1)
        {
            throw new KeyNotFoundException($"Елемент з Id = {id} не знайдено.");
        }

        return _items[index];
    }

    public IEnumerable<T> GetAll()
    {
        return new List<T>(_items);
    }

    public void Add(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (FindIndex(item.Id) != -1)
        {
            throw new InvalidOperationException($"Елемент з Id = {item.Id} вже існує.");
        }

        _items.Add(item);
    }

    public void Update(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        int index = FindIndex(item.Id);
        if (index == -1)
        {
            throw new KeyNotFoundException($"Неможливо оновити: елемент з Id = {item.Id} не знайдено.");
        }

        _items[index] = item;
    }

    public void Delete(int id)
    {
        int index = FindIndex(id);
        if (index == -1)
        {
            throw new KeyNotFoundException($"Неможливо видалити: елемент з Id = {id} не знайдено.");
        }

        _items.RemoveAt(index);
    }

    private int FindIndex(int id)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i].Id == id)
            {
                return i;
            }
        }

        return -1;
    }
}

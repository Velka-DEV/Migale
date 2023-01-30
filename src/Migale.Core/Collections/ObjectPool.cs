using Migale.Core.Crawlers;

namespace Migale.Core.Collections;

public class ObjectPool<T>
{
    public int Count => _pool.Count;
    
    private int _index = 0;
    private readonly List<T> _pool;
    private readonly object _locker = new object();
    
    public ObjectPool () : this(0) { }

    internal ObjectPool(int initialCapacity)
    {
        _pool = new List<T>(initialCapacity);
    }
    
    internal ObjectPool(IEnumerable<T> items)
    {
        _pool = new List<T>(items);
    }

    public T? NextItem(bool delete = false)
    {
        lock (_locker)
        {
            if (!_pool.Any()) return default;
            
            var item = _pool[_index];
            
            if (delete) _pool.RemoveAt(_index);
            else _index++;
            
            if (_index >= _pool.Count) _index = 0;
            
            return item;
        }
    }

    public T? RandomItem(bool delete = false)
    {
        if (!_pool.Any()) return default;
        
        lock (_locker)
        {
            var index = Random.Shared.Next(_pool.Count);

            var item = _pool[index];
            
            if (delete) _pool.RemoveAt(index);
            
            return item;
        }
    }

    public bool IsEmpty() => !_pool.Any();

    public void Clear() => _pool.Clear();
    
    public void Add(T item) => _pool.Add(item);
    
    public void AddRange(IEnumerable<T> items) => _pool.AddRange(items);
    
    public void Remove(T item) => _pool.Remove(item);
}
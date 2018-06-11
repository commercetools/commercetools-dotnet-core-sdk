using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace commercetools.Common
{
    public class LimitedPool<T> : IDisposable where T : class
    {
        readonly Func<T> _valueFactory;
        readonly Action<T> _valueDisposeAction;
        readonly ConcurrentStack<LimitedPoolItem<T>> _pool;
        bool _disposed;
        public LimitedPool(Func<T> valueFactory, Action<T> valueDisposeAction)
        {
            _valueFactory = valueFactory;
            _valueDisposeAction = valueDisposeAction;
            _pool = new ConcurrentStack<LimitedPoolItem<T>>();
        }
        public int IdleCount => _pool.Count;
        public LimitedPoolItem<T> Get(TimeSpan limitedPoolItemLifeSpan)
        {
            LimitedPoolItem<T> item;
            // try to get live cached item
            while (!_disposed && _pool.TryPop(out item))
            {
                if (!item.Expired && item.Lifetime == limitedPoolItemLifeSpan) {
                    return item;
                }
                if (item.Expired)
                {
                    // dispose expired item
                    item.Dispose();
                    // try to collect other items as well
                    CollectAllExpiredItems();
                }
            }
            // since no cached items available we create a new one
            return new LimitedPoolItem<T>(_valueFactory(), disposedItem =>
            {
                if (_disposed || disposedItem.Expired)
                {
                    // item has been expired, dispose it
                    if (Interlocked.CompareExchange(ref disposedItem.DisposeFlag, 1, 0) == 0)
                    {
                        _valueDisposeAction(disposedItem.Value);
                    }
                }
                else
                {
                    // item is still fresh enough, return it to the pool
                    if (!_disposed)
                    {
                        _pool.Push(disposedItem);
                    }
                }
            }, limitedPoolItemLifeSpan);
        }
        void CollectAllExpiredItems()
        {
            const int maximumBufferSize = 1000;
            int length = Math.Min(_pool.Count, maximumBufferSize);
            if (length <= 0) {
                return;
            }

            LimitedPoolItem<T>[] items = new LimitedPoolItem<T>[length];
            int poppedItems = _pool.TryPopRange(items);
            for (int i = 0; i < poppedItems; i++)
            {
                LimitedPoolItem<T> item = items[i];
                // if item is expired it will be disposed, otherwise returned back to the pool
                try
                {
                    item.Dispose();
                }
                catch
                {
                    // return rest items back to the pool and rethrow
                    int nextIndex = i + 1;
                    if (nextIndex < poppedItems) {
                        _pool.PushRange(items, nextIndex, poppedItems - nextIndex);
                    }
                    throw;
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposed = true;
                LimitedPoolItem<T>[] items = _pool.ToArray();
                foreach (LimitedPoolItem<T> item in items)
                {
                    _valueDisposeAction(item.Value);
                }
            }
        }
    }
}

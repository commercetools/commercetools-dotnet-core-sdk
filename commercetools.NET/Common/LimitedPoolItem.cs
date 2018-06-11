using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace commercetools.Common
{
    public class LimitedPoolItem<T> : IDisposable
    {
        readonly Action<LimitedPoolItem<T>> _disposeAction;
        readonly TimeSpan _lifetime;
        bool _expired;
        internal int DisposeFlag;
        public T Value { get; }
        internal bool Expired
        {
            get
            {
                if (_expired)
                {
                    return true;
                }
                _expired = _stopwatch.Elapsed > _lifetime;
                return _expired;
            }
        }
        internal TimeSpan Lifetime
        {
            get
            {
                return _lifetime;
            }
        }
        readonly Stopwatch _stopwatch;
        internal LimitedPoolItem(T value, Action<LimitedPoolItem<T>> disposeAction, TimeSpan lifetime)
        {
            _disposeAction = disposeAction;
            _lifetime = lifetime;
            Value = value;
            _stopwatch = Stopwatch.StartNew();
        }
        public void Expire()
        {
            _expired = true;
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
                _disposeAction(this);
            }
        }
    }
}

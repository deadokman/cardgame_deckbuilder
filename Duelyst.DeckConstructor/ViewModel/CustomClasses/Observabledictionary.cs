using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Duelyst.DeckConstructor.ViewModel.CustomClasses
{
    public class ObservableDictionary<TKey, TObject> : INotifyCollectionChanged, IEnumerable<KeyValuePair<TKey, TObject>>
    {
        public ObservableDictionary()
        {
            _dict = new Dictionary<TKey, TObject>();
        }
        
        public void Add(TKey key, TObject obj)
        {
            _dict.Add(key, obj);
            RaiseCollectionChanged(NotifyCollectionChangedAction.Add, obj);
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        public void Remove(TKey key)
        {
            if (_dict.ContainsKey(key))
            {
                var obj = _dict[key];
                RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, obj);
            }
        }

        private Dictionary<TKey, TObject> _dict = new Dictionary<TKey, TObject>();

        private void RaiseCollectionChanged(NotifyCollectionChangedAction act, TObject obj)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(act, obj));
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<KeyValuePair<TKey, TObject>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }

        public TObject this[TKey key]
        {
            get
            {
                return _dict[key];
            }
            set
            {
                _dict[key] = value;
                RaiseCollectionChanged(NotifyCollectionChangedAction.Add, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

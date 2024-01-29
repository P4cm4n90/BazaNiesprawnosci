using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaNiesprawnosci
{
    public class PropertyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {

        private IDictionary<TKey, TValue> _Dictionary;
        protected IDictionary<TKey, TValue> Dictionary
        {
            get { return _Dictionary; }
        }

        #region Constructors
        public PropertyDictionary()
        {
            _Dictionary = new Dictionary<TKey, TValue>();
        }
        public PropertyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _Dictionary = new Dictionary<TKey, TValue>(dictionary);
        }
        public PropertyDictionary(IEqualityComparer<TKey> comparer)
        {
            _Dictionary = new Dictionary<TKey, TValue>(comparer);
        }
        public PropertyDictionary(int capacity)
        {
            _Dictionary = new Dictionary<TKey, TValue>(capacity);
        }
        public PropertyDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _Dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }
        public PropertyDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }
        #endregion

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            Insert(key, value, true);
        }

        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return Dictionary.Keys; }
        }

        public bool Remove(TKey key)
        {
            if (!lockProperties)
            {
                if (key == null) throw new ArgumentNullException("key");

                Dictionary.TryGetValue(key, out TValue value);
                var removed = Dictionary.Remove(key);
                if (removed)
                    OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));
                return removed;
            }
            else
                return false;
        }


        public bool TryGetValue(TKey key, out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }


        public ICollection<TValue> Values
        {
            get { return Dictionary.Values; }
        }


        public TValue this[TKey key]
        {
            get
            {
                return Dictionary[key];
            }
            set
            {
                Insert(key, value, false);
            }
        }


        #endregion


        #region ICollection<KeyValuePair<TKey,TValue>> Members


        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Insert(item.Key, item.Value, true);
        }


        public void Clear()
        {
            if (!lockProperties)
            {
                if (Dictionary.Count > 0)
                {
                    Dictionary.Clear();
                    OnCollectionChanged();
                }
            }
        }


        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Dictionary.Contains(item);
        }

        private bool lockProperties = false;

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Dictionary.CopyTo(array, arrayIndex);
        }


        public int Count
        {
            get { return Dictionary.Count; }
        }


        public bool IsReadOnly
        {
            get { return Dictionary.IsReadOnly; }
        }

        public bool LockProperties { get => lockProperties; set => lockProperties = value; }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }


        #endregion


        #region IEnumerable<KeyValuePair<TKey,TValue>> Members


        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }


        #endregion


        #region IEnumerable Members


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Dictionary).GetEnumerator();
        }


        #endregion


        #region INotifyCollectionChanged Members


        public event NotifyCollectionChangedEventHandler CollectionChanged;


        #endregion


        #region INotifyPropertyChanged Members


        public event PropertyChangedEventHandler PropertyChanged;


        #endregion


        public void AddRange(IDictionary<TKey, TValue> items)
        {
            if (!lockProperties)
            {
                if (items == null) throw new ArgumentNullException("items");


                if (items.Count > 0)
                {
                    if (Dictionary.Count > 0)
                    {
                        if (items.Keys.Any((k) => Dictionary.ContainsKey(k)))
                            throw new ArgumentException("An item with the same key has already been added.");
                        else
                            foreach (var item in items) Dictionary.Add(item);
                    }
                    else
                        _Dictionary = new Dictionary<TKey, TValue>(items);


                    OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
                }
            }
        }


        private void Insert(TKey key, TValue value, bool add)
        {
            if (key == null) throw new ArgumentNullException("Key cannot be null");


            if (Dictionary.TryGetValue(key, out TValue item))
            {
                if (add) throw new ArgumentException("An item with the same key has already been added.");
                if (Equals(item, value)) return;
                Dictionary[key] = value;

                OnPropertyChanged(key as string);
                OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, item));
            }
            else
            {
                if (!lockProperties)
                {
                    Dictionary[key] = value;

                    OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }


        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }


        private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }


        private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newItems));
        }

    }
}

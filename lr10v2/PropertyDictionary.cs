using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;



namespace lr10v2
{
    public interface IOrderedDictionary<TKey, TValue> : IOrderedDictionary, IDictionary<TKey, TValue>
    {




        void Insert(int index, TKey key, TValue value);


        new TValue this[int index]
        {
            get;
            set;
        }
    }
    public class PropertyDictionary<TValue> : IOrderedDictionary<string, TValue>
    {

        PropertyList<TValue> list;


        public PropertyDictionary() => list = new PropertyList<TValue>(this);
        public TValue this[int index]
        {
            get => list[index].Value;
            set => list[index].Value = value;
        }

        object IOrderedDictionary.this[int index]
        {
            get => this[index];
            set => this[index] = (TValue)value;
        }


        public TValue this[string key]
        {
            get => list.First(prop => prop.Name == key).Value;
            set
            {
                if (list.TryFirst(prop => prop.Name == key, out var result))
                {
                    result.Value = value;
                }
                else
                {
                    list.Add(key, value);
                }
            }
        }
        public object this[object key]
        {
            get => this[key as string];
            set => this[key as string] = (TValue)value;
        }




        public bool IsFixedSize => false;

        public bool IsReadOnly => false;

        ICollection<string> IDictionary<string, TValue>.Keys => (from prop in list select prop.Name) as ICollection<string>;

        ICollection<TValue> IDictionary<string, TValue>.Values => (from prop in list select prop.Value) as ICollection<TValue>;

        public ICollection Keys => (this as IDictionary<string, TValue>).Keys as ICollection;

        public ICollection Values => (this as IDictionary<string, TValue>).Values as ICollection;
        public int Count => list.Count;

        public bool IsSynchronized => false;

        public object SyncRoot => null;

        public bool ContainsKey(string key) => list.Any(prop => prop.Name == key);


        public bool Contains(object key) => ContainsKey(key as string);




        public bool Contains(KeyValuePair<string, TValue> item) => list.Contains(item);




        public void Add(string key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("такой ключ уже есть");
            }
            else list.Add(key, value);
        }

        public void Add(object key, object value) => Add(key as string, (TValue)value);



        public void Add(KeyValuePair<string, TValue> item)
        {
            if (ContainsKey(item.Key))
            {
                throw new ArgumentException("такой ключ уже есть");
            }
            else list.Add(item);
        }

        public void Clear() => list.Clear();




        public void CopyTo(Array array, int index) => (list as ICollection).CopyTo(array, index);



        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            foreach (var el in list)
            {
                array[arrayIndex] = el;
                arrayIndex++;
            }


        }



        public void Insert(int index, string key, TValue value) => list.Insert(index, new KeyValuePair<string, TValue>(key, value));

        public void Insert(int index, object key, object value) => list.Insert(index, new KeyValuePair<string, TValue>(key as string, (TValue)value));

        public bool Remove(string key) => list.Remove(key);

        public void Remove(object key) => list.Remove(key as string);


        public bool Remove(KeyValuePair<string, TValue> item) => list.Remove(item);

        public void RemoveAt(int index) => list.RemoveAt(index);

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out TValue value)
        {

            if (list.TryFirst(prop => prop.Name == key, out var result))
            {
                value = result.Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        IEnumerator<KeyValuePair<string, TValue>> IEnumerable<KeyValuePair<string, TValue>>.GetEnumerator()
        {
            int index = 0;
            while (index < list.Count)
            {
                yield return list[index];
                index++;
            }
        }


        IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator() => new DictionaryEnumerator<string, TValue>(this.AsEnumerable().GetEnumerator());


        IDictionaryEnumerator IDictionary.GetEnumerator() => new DictionaryEnumerator<string, TValue>(this.AsEnumerable().GetEnumerator());


        public override string ToString() => '{' + string.Join(", ", list) + '}';
       

    }

}

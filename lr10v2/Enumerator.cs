using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lr10v2
{

    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator
    {
        public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator) => Enumerator = enumerator;

        public IEnumerator<KeyValuePair<TKey, TValue>> Enumerator;

        public DictionaryEntry Entry => new DictionaryEntry(Enumerator.Current.Key, Enumerator.Current.Value);

        public object Key => Enumerator.Current.Key;

        public object Value => Enumerator.Current.Value;

        public object Current => Entry;

        public bool MoveNext() => Enumerator.MoveNext();

        public void Reset() => Enumerator.Reset();
    }
}

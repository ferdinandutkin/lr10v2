using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
 
namespace lr10v2
{

    class ProxyProperty<TValue>
    {
        object property;

        object owner;

        public ProxyProperty(KeyValuePair<string, TValue> property) => this.property = property;

        public bool IsClassProperty { get; private set; } = false;

        public static implicit operator KeyValuePair<string, TValue>(ProxyProperty<TValue> proxyProperty) => new KeyValuePair<string, TValue>(proxyProperty.Name, proxyProperty.Value);

        public static implicit operator ProxyProperty<TValue>(KeyValuePair<string, TValue> pair) => new ProxyProperty<TValue>(pair);


        public void Deconstruct(out string name, out TValue value) => (name, value) = (Name, Value);
     
        public ProxyProperty(PropertyInfo property, object owner)
        {
            if (!CanUse(property))
                throw new ArgumentException("Свойство должно быть доступно для чтения и записи");
            this.property = property;
            this.IsClassProperty = true;
            this.owner = owner;
        }


        public override string ToString() => $"{Name} = {Value}";
        

        static public bool CanUse(PropertyInfo property) => property.GetIndexParameters().Length == 0 && property.CanRead && property.CanWrite && property.PropertyType == typeof(TValue);
        public string Name
        {
            get =>
                property switch
                {
                    PropertyInfo classProperty => classProperty.Name,
                    KeyValuePair<string, TValue> pair => pair.Key,
                    _ => ""
                };
            set
            {
                switch (property)
                {
                    case PropertyInfo _: throw new ArgumentException("Попытка переименовать свойство класса");
                    case KeyValuePair<string, TValue> pair: property = new KeyValuePair<string, TValue>(value, pair.Value); break;
                }
            }

        }
 

        public TValue Value
        {
            get =>
                 property switch
                 {
                     PropertyInfo classProperty => (TValue)classProperty.GetValue(owner),
                     KeyValuePair<string, TValue> pair => pair.Value,
                     _ => default
                 };

            set
            {
                switch (property)
                {
                    case PropertyInfo property: property.SetValue(owner, value); break;
                    case KeyValuePair<string, TValue> pair: property = new KeyValuePair<string, TValue>(pair.Key, value); break;
                }
            }
        }


        public override bool Equals(object obj)
            => (obj is ProxyProperty<TValue> prop) ? (prop.Name == Name && prop.Value.Equals(Value)) : false;
      
        ~ProxyProperty() => Value = default;
        

    }




    class PropertyList<TValue> : IEnumerable<ProxyProperty<TValue>>, IList<ProxyProperty<TValue>>, ICollection<ProxyProperty<TValue>>
    {
        List<ProxyProperty<TValue>> properties;
        public PropertyList(object owner) =>
              properties = (from prop in owner.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance) where ProxyProperty<TValue>.CanUse(prop) select new ProxyProperty<TValue>(prop, owner)).ToList();
  

        public ProxyProperty<TValue> this[int index] { get => ((IList<ProxyProperty<TValue>>)properties)[index]; set => ((IList<ProxyProperty<TValue>>)properties)[index] = value; }

        public int Count => ((ICollection<ProxyProperty<TValue>>)properties).Count;

        public bool IsReadOnly => ((ICollection<ProxyProperty<TValue>>)properties).IsReadOnly;

        public void Add(ProxyProperty<TValue> item) => ((ICollection<ProxyProperty<TValue>>)properties).Add(item);

        public void Add(string key, TValue value) => Add(new KeyValuePair<string, TValue>(key, value));

        public void Clear()
        {
            properties.RemoveAll(prop => !prop.IsClassProperty);
            for (int i = 0; i < properties.Count; i++)
            {
                properties[i].Value = default;
            }
        }

        public bool Contains(ProxyProperty<TValue> item) => ((ICollection<ProxyProperty<TValue>>)properties).Contains(item);

        public void CopyTo(ProxyProperty<TValue>[] array, int arrayIndex) => ((ICollection<ProxyProperty<TValue>>)properties).CopyTo(array, arrayIndex);

        public IEnumerator<ProxyProperty<TValue>> GetEnumerator() => ((IEnumerable<ProxyProperty<TValue>>)properties).GetEnumerator();

        public int IndexOf(ProxyProperty<TValue> item) => ((IList<ProxyProperty<TValue>>)properties).IndexOf(item);

        public void Insert(int index, ProxyProperty<TValue> item)
        => ((IList<ProxyProperty<TValue>>)properties).Insert(index, item);

        public bool Remove(string propName)
        {
            if (properties.TryFirst(prop => prop.Name == propName, out var result))
            {
                if (result.IsClassProperty)
                {

                 
                    result.Value = default;
                    return true;

                }
                else
                {
             
                  
                    return ((ICollection<ProxyProperty<TValue>>)properties).Remove(result);
                }

            }
            else return false;
        }


        public bool Remove(ProxyProperty<TValue> item)
        {
            if (properties.TryFirst(prop => prop.Equals(item), out var result))
            {
                if (result.IsClassProperty)
                {
                    result.Value = default;
                    return true;

                }
                else
                {
                    return ((ICollection<ProxyProperty<TValue>>)properties).Remove(item);
                }

            }
            else return false;
        }

        public void RemoveAt(int index)
        {
            if (properties[index].IsClassProperty)
            {
                properties[index].Value = default;
            }
            else
            {
                ((IList<ProxyProperty<TValue>>)properties).RemoveAt(index);
            }
           
        }

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)properties).GetEnumerator();

    }
}

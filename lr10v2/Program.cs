using System;

using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace lr10v2
{



    class Program
    {






        public class Product : PropertyDictionary<object>
        {
            public object Name { get; set; }
            public object Price { get; set; } = 5;
            public object Weight { get; set; } = 1000;

            public object ShelfLife { get; set; } = 14;
        }


        public class Product<TValue> : PropertyDictionary<TValue>
        {
            public string Name { get; set; }
            public int Price { get; set; } = 5;
            public int Weight { get; set; } = 1000;

            public int ShelfLife { get; set; } = 14;
        }


        class Transformer : IComparable<Transformer>
        {
            public Transformer(string mark, int powerLevel) => (Mark, PowerLevel) = (mark, powerLevel);

            public DateTime CreationDate { get; } = DateTime.Now;

            public readonly string Mark;
            public int PowerLevel { get; set; }
            public int CompareTo(Transformer other) => PowerLevel.CompareTo(other.PowerLevel);

            public override string ToString() => $"Трансформер \"{Mark}\", Уровень мощности: {PowerLevel}, Дата создания: {CreationDate.ToString("d")}";

        }

        static void Main(string[] args)
        {
            var Milk = new Product<int>() { Name = "Молоко обыкновенное" };

            Milk.ShelfLife = 13;

            Milk["ShelfLife"] = 12;

            Milk[2] = 11;

            Milk["Count"] = 8;

            Milk.Add("Quality", 1);


            Console.WriteLine(Milk);


            Console.WriteLine();

            Milk.Remove("Count");

            Milk.Remove("Price");
            foreach (var (property, value) in Milk)
            {
                Console.WriteLine($"{property} : {value}");
            }
            for (int i = 0; i < Milk.Count; i++)
            {
                Console.WriteLine($"Свойство {i + 1} : {Milk[i]}");
            }

            Milk.Insert(1, "Молочность", 100);

            Console.WriteLine(Milk[1]);

            var products = new ConcurrentBag<Product<int>>() { new Product<int>() { Name = "Масло" }, new Product<int>() { Name = "Батон" } };

            var tasks = new List<Task>();
            foreach (var name in new[] { "Хлеб", "Вода", "Кефир", "Помидоры", "Яблоки" })
            {
                tasks.Add(Task.Run(() => products.Add(new Product<int>() { Name = name })));
            }


            Task.WaitAll(tasks.ToArray());

            foreach (var product in products)
            {
                Console.WriteLine(product.Name);

            }



            //2
            var hashSet = new HashSet<long>() { 1335, 1245, 109, 475, 137 };

            foreach (var el in hashSet)
            {
                Console.WriteLine(el);
            }


            hashSet.RemoveWhere(el => el <= 475); //удалено 3 последовательных

            hashSet.Add(10);


            var linkedList = new LinkedList<long>();


            foreach (var el in hashSet)
            {
                linkedList.AddLast(el);
            }

            Console.WriteLine(linkedList.Find(1335).Value);
            //f


            var hashSetT = new HashSet<Transformer>() { new Transformer("Тестовая модель", 4),
                new Transformer("Тестовая модель", 3), new Transformer("Тестовая модель", 2),
                new Transformer("Тестовая модель", 2), new Transformer("Тестовая модель", 1),
                new Transformer("Тестовая модель", 9)
            };

            foreach (var el in hashSetT)
            {
                Console.WriteLine(el);
            }


            hashSetT.RemoveWhere(el => el.PowerLevel <= 3);

            var mark5 = new Transformer("Модель 5", 8);

            hashSetT.Add(mark5);



            var linkedListT = new LinkedList<Transformer>();

            foreach (var el in hashSetT)
            {
                linkedListT.AddLast(el);
            }

            Console.WriteLine(linkedListT.Find(mark5).Value);


            var observableTransformers = new ObservableCollection<Transformer>() { new Transformer("Тестовая модель", 4) };
            observableTransformers.CollectionChanged += ChangeHandler;

            observableTransformers.Add(new Transformer("Тестовая модель 2", 7));
            observableTransformers[0] = new Transformer("Новая модель", 13);
            observableTransformers.RemoveAt(0);
            observableTransformers.Clear();

            void ChangeHandler(object obj, NotifyCollectionChangedEventArgs args)
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Console.WriteLine($"Добавлен {args.NewItems[0] as Transformer}");
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        Console.WriteLine($"Удален элемент {args.OldItems[0] as Transformer}");
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        Console.WriteLine("Коллекция сброшена");
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        Console.WriteLine($"Элемент {args.OldItems[0] as Transformer} заменен на {args.NewItems[0] as Transformer}");
                        break;
                    default: break;
                }
            }














        }
    }
}
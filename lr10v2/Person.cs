using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace lr10v2
{
    

        static class StringExtensions
        {
            public static string ToNameCase(this string source) => source.ToLower().Remove(0, 1)
                        .Insert(0, source.Substring(0, 1).ToUpper());

        }



        public class Person
        {
            private string surname;
            private string name;
            private string patronymic;


            public string Surname
            {
                get => surname;
                set => surname = value.ToNameCase();
            }

            public string Name
            {
                get => name;
                set => name = value.ToNameCase();
            }

            public string Patronymic
            {
                get => patronymic;
                set => patronymic = value.ToNameCase();
            }

            public string FullName => $"{Surname} {Name} {Patronymic}";

            public ushort Age =>
                  (ushort)((DateTime.Now.Year - Birthday.Year - 1) + (((DateTime.Now.Month > Birthday.Month)
                || ((DateTime.Now.Month == Birthday.Month) && (DateTime.Now.Day >= Birthday.Day))) ? 1 : 0));

            public Date Birthday { get; protected set; }


            public Person(string surname, string name, string patronymic, Date birthday)
            {
                Surname = surname;
                Name = name;
                Patronymic = patronymic;
                Birthday = birthday;
            }

            //в этом языке не любят copy ctorы поэтому пускай будет protected)))
            protected Person(Person p) : this(p.Surname, p.Name, p.Patronymic, p.Birthday)
            {
            }

            public Person(string personaldata)
            {
                string[] pd = personaldata.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (pd.Length != 4)
                {
                    Debug.WriteLine("парсинг личных данных сломался");
                }
                else
                {
                    Surname = pd[0];
                    Name = pd[1];
                    Patronymic = pd[2];
                    Birthday = pd[3];

                }

            }


            public override string ToString() => FullName + $" {Birthday}";

            public override bool Equals(object obj)
            {
                if (obj is Person p)
                {
                    return (p.Name == Name) && (p.Patronymic == Patronymic) && (p.Surname == Surname) && p.Birthday.Equals(Birthday);
                }
                else return false;
            }

            public override int GetHashCode() => HashCode.Combine(Surname, Name, Patronymic, Birthday);
        }










        public partial class Student : Person
        {
            public readonly int id;



            private string phoneNumber;




            public override string ToString() => base.ToString() + $" {Faculty} {Course} {Group}";

            //  Адрес,  Телефон, Факультет, Курс, Группа.


            public string Address
            {
                get; set;
            }

            public string PhoneNumber
            {
                get => phoneNumber;
                set
                {
                    var toSet = value.Trim();
                    if (toSet.Length != 13)
                    {
                        Debug.WriteLine("номер должен быть 13 символов");
                    }
                    else
                    {
                        phoneNumber = toSet;
                    }

                }
            }

            public string Faculty
            {
                get; set;
            }
            public int Course
            {
                get; set;
            }

            public int Group
            {
                get; set;
            }

            public override bool Equals(object obj)
            {

                if (obj is Student s)
                {
                    return base.Equals(s) && (s.Address == Address) && (s.PhoneNumber == PhoneNumber) && (s.Faculty == Faculty) && (s.Course == Course) && (s.Group == Group);
                }
                else return false;
            }

            public override int GetHashCode()
            {
                HashCode hash = new HashCode();
                hash.Add(base.GetHashCode());
                hash.Add(Address);
                hash.Add(PhoneNumber);
                hash.Add(Faculty);
                hash.Add(Course);
                hash.Add(Group);
                return hash.ToHashCode();
            }

            private Student(Person p, string address, string phonenumber, string faculty, int course = 1, int group = 1) : base(p)
            {
                Address = address;
                PhoneNumber = phonenumber;
                Faculty = faculty;
                Course = course;
                Group = group;

                id = Math.Abs(GetHashCode());
            }
            //аргументы по умолчанию есть! приватный конструктор есть!
            public Student(string personaldata, string address, string phonenumber, string faculty, int course, int group) :
                this(new Person(personaldata), address, phonenumber, faculty, course, group)
            {
            }

            public Student() : this(new Person("Неизвестно Неизвестно Неизвестно 01.01.1970"), "Неизвестно", "Неизвестно", "Неизвестно")
            {
            }



        }
    }

 

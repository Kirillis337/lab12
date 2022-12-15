using System;
using System.Collections.Generic;

namespace lab12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------------------------Adapter---------------------------------");
            // Рыцарь
            Knight knight = new Knight();
            // Конь
            Horse hrs = new Horse();
            // Рыцарь едет на коне по полю
            knight.Travel(hrs);
            // Впереди высокие горы - нужен дракон!
            Dragon drag = new Dragon();
            // используем адаптер
            ITransport dragonTransport = new DragonToTransportAdapter(drag);
            // Облетаем высокие горы на драконе
            knight.Travel(dragonTransport);

            Console.WriteLine("--------------------------Мост---------------------------------");
            // создаем комика, который шутит только хорошие шутки
            Сomedian comedian = new TicktokСomedian(new GoodJokes());
            comedian.DoWork();
            comedian.EarnMoney();
            // взяли в камеди клаб
            comedian.Language = new BadJokes();
            comedian.DoWork();
            comedian.EarnMoney();
            Console.WriteLine("--------------------------Приспособленец (Flyweight)---------------------------------");
            double row = 1;
            double column = 1;

            ArmyFactory unitFactory = new ArmyFactory();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Unit panelHouse = unitFactory.GetUnit("Warrior");
                    if (panelHouse != null)
                        panelHouse.Build(row, column);
                    column += 1;
                    if (column > 5)
                        column = 1;
                }
                row += 1;
            }

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Unit brickHouse = unitFactory.GetUnit   ("Archer");
                    if (brickHouse != null)
                        brickHouse.Build(row, column);
                    column += 1;
                    if (column > 5)
                        column = 1;
                }
                row += 1;
            }
            Console.WriteLine("--------------------------Прокси---------------------------------");
            CoolKnight coolknight = new Proxy_Knight();
            coolknight.cleararmor();
            coolknight.Defend();
            Console.Read();

        }
    }
    //--------------------Adapter----------------------------
    interface ITransport
    {
        void Drive();
    }
    // класс машины
    class Horse : ITransport
    {
        public void Drive()
        {
            Console.WriteLine("Конь скачет по полю");
        }
    }
    class Knight
    {
        public void Travel(ITransport transport)
        {
            transport.Drive();
        }
    }
    // интерфейс животного
    interface IAnimal
    {
        void Move();
    }
    // класс верблюда
    class Dragon : IAnimal
    {
        public void Move()
        {
            Console.WriteLine("Дракон облетает высокие горы");
        }
    }
    // Адаптер от Dragon к ITransport
    class DragonToTransportAdapter : ITransport
    {
        Dragon dragon;
        public DragonToTransportAdapter(Dragon d)
        {
            dragon = d;
        }

        public void Drive()
        {
            dragon.Move();
        }
    }
    //------------------------Мост---------------------------------
    interface ILanguage
    {
        void Create();
        void Joke();
    }

    class GoodJokes : ILanguage
    {
        public void Create()
        {
            Console.WriteLine("Придумать смешную шутку");
        }

        public void Joke()
        {
            Console.WriteLine("Смешно пошутить");
        }
    }

    class BadJokes : ILanguage
    {
        public void Create()
        {
            Console.WriteLine("Придумать шутку про говно");
        }

        public void Joke()
        {
            Console.WriteLine("Тупо пошутить");
            Console.WriteLine("Сделать вид что всем смешно");
        }
    }

    abstract class Сomedian
    {
        protected ILanguage language;
        public ILanguage Language
        {
            set { language = value; }
        }
        public Сomedian(ILanguage lang)
        {
            language = lang;
        }
        public virtual void DoWork()
        {
            language.Create();
            language.Joke();
        }
        public abstract void EarnMoney();
    }

    class TicktokСomedian : Сomedian
    {
        public TicktokСomedian(ILanguage lang) : base(lang)
        {
        }
        public override void EarnMoney()
        {
            Console.WriteLine("Получаем оплату за шутки");
        }
    }
    class ComedyClubComedian : Сomedian
    {
        public ComedyClubComedian(ILanguage lang)
            : base(lang)
        {
        }
        public override void EarnMoney()
        {
            Console.WriteLine("Получаем в конце месяца зарплату");
        }
    }
    // Приспособленец (Flyweight)
    abstract class Unit
    {
        protected int hp; // количество жизней
        protected int dmg; // количество урона

        public abstract void Build(double row, double column);
    }

    class Warrior : Unit
    {
        public Warrior()
        {
            hp = 10;
            dmg = 3;
        }

        public override void Build(double row, double column)
        {
            Console.WriteLine("Создан Воин. координаты: {0} ряд, {1} место в ряду", row, column);
        }
    }
    class Archer : Unit
    {
        public Archer()
        {
            hp = 5;
            dmg = 5;
        }

        public override void Build(double row, double column)
        {
            Console.WriteLine("Создан Лучник. координаты: {0} ряд, {1} место в ряду", row, column);
        }
    }

    class ArmyFactory
    {
        Dictionary<string, Unit> units = new Dictionary<string, Unit>();
        public ArmyFactory()
        {
            units.Add("Warrior", new Warrior());
            units.Add("Archer", new Archer());
        }

        public Unit GetUnit(string key)
        {
            if (units.ContainsKey(key))
                return units[key];
            else
                return null;
        }
    }
    //-------------------------Прокси---------------------------------------
    abstract class CoolKnight
    {
        public abstract void cleararmor();
        public abstract void Defend();
    }

    class Real_Knight : CoolKnight
    {
        public override void cleararmor()
        {
            Console.WriteLine(GetType().Name + ": Почистить доспех? ладно.");
        }
        public override void Defend()
        {
            Console.WriteLine(GetType().Name + ": ЗАЩИТА!");
        }
    }

    class Proxy_Knight : CoolKnight
    {
        Real_Knight real_knight;
        public override void cleararmor()
        {
            Console.WriteLine(GetType().Name + ": Почистить доспех? Будет сделано!");
        }
        public override void Defend()
        {
            Console.WriteLine(GetType().Name + ": Защищаться? я слишком слабый, сейчас позову рыцаря");
            real_knight = new Real_Knight();
            real_knight.Defend();
        }
    }
}

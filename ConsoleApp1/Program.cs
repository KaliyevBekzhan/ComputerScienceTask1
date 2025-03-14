using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Progrma
    {
        private static Queue<string> orders = new Queue<string>();
        private static object locker = new object();
        private static Random random = new Random();
        private static string[] menu = { "Пицца", "Бургер", "Суши", "Салат", "Стейк" };
        private const int maxOrders = 5;

        static void Main()
        {
            Thread waiterThread = new Thread(WaiterWork);
            Thread chefThread = new Thread(ChefWork);

            waiterThread.Start();
            chefThread.Start();

            waiterThread.Join();
            chefThread.Join();
        }

        static void WaiterWork()
        {
            while (true)
            {
                lock (locker)
                {
                    if (orders.Count < maxOrders)
                    {
                        string order = menu[random.Next(menu.Length)];
                        orders.Enqueue(order);
                        Console.WriteLine($"Официант: Добавлен заказ на {order}. Текущее количество заказов: {orders.Count}");
                    }
                    else
                    {
                        Console.WriteLine("Официант: Очередь заказов переполнена, жду");
                    }
                }
                Thread.Sleep(2000);
            }
        }

        static void ChefWork()
        {
            while (true)
            {
                lock (locker)
                {
                    if (orders.Count > 0)
                    {
                        string order = orders.Dequeue();
                        Console.WriteLine($"Повар: Приступаю к приготовлению {order}. Осталось заказов: {orders.Count}");
                    }
                    else
                    {
                        Console.WriteLine("Повар: Нет заказов, жду");
                    }
                }
                Thread.Sleep(3000);
            }
        }
    }

}

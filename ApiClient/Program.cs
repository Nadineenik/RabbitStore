using System;
using System.Threading.Tasks;
using laba2.Models; 

class Program
{
    static readonly ApiService apiService = new ApiService();

    static async Task Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Магазин кроликов - Консольный клиент");
            Console.WriteLine("1. Просмотреть всех кроликов");
            Console.WriteLine("2. Добавить кролика");
            Console.WriteLine("3. Изменить кролика");
            Console.WriteLine("4. Удалить кролика");
            Console.WriteLine("5. Поиск кроликов");
            Console.WriteLine("6. Выйти");
            Console.Write("Выберите действие: ");

            string choice = Console.ReadLine() ?? "";

            switch (choice)
            {
                case "1":
                    await ShowRabbits();
                    break;
                case "2":
                    await AddRabbit();
                    break;
                case "3":
                    await UpdateRabbit();
                    break;
                case "4":
                    await DeleteRabbit();
                    break;
                case "5":
                    await SearchRabbits();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    // 🔍 Метод поиска кроликов
    static async Task SearchRabbits()
    {
        Console.Write("\nВведите поисковый запрос (имя или описание): ");
        string query = Console.ReadLine() ?? "";

        var results = await apiService.SearchRabbits(query);

        Console.WriteLine("\n Результаты поиска:");
        if (results.Count == 0)
        {
            Console.WriteLine(" Ничего не найдено.");
            return;
        }

        foreach (var rabbit in results)
        {
            Console.WriteLine($" {rabbit.Id} - {rabbit.Name} ({rabbit.Price}руб)");
            Console.WriteLine($"    {rabbit.Description}");
            Console.WriteLine($"    Изображение: {rabbit.ImageUrl}");
            Console.WriteLine("---------------------------------");
        }
    }

    static async Task ShowRabbits()
    {
        var rabbits = await apiService.GetRabbits();
        Console.WriteLine("\n Список кроликов:");
        if (rabbits.Count == 0)
        {
            Console.WriteLine(" Кроликов пока нет.");
            return;
        }

        foreach (var rabbit in rabbits)
        {
            Console.WriteLine($" {rabbit.Id} - {rabbit.Name} ({rabbit.Price}руб)");
        }
    }

    static async Task AddRabbit()
    {
        Console.Write("\nВведите имя кролика: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Введите описание: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Введите цену: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine(" Ошибка: введите корректную цену!");
            return;
        }

        var rabbit = new Rabbit
        {
            Name = name,
            Description = description,
            Price = price,
            ImageUrl = "/images/default-rabbit.jpg"
        };

        await apiService.AddRabbit(rabbit);
        Console.WriteLine("V Кролик добавлен!");
    }

    static async Task UpdateRabbit()
    {
        Console.Write("\nВведите ID кролика для обновления: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) 
        {
            Console.WriteLine(" Ошибка: неверный ID!");
            return;
        }

        Console.Write("Введите новое имя кролика: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Введите новое описание: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Введите новую цену: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
        {
            Console.WriteLine(" Ошибка: введите корректную цену!");
            return;
        }

        var rabbit = new Rabbit
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            ImageUrl = "/images/default-rabbit.jpg"
        };

        await apiService.UpdateRabbit(rabbit);
        Console.WriteLine("V Кролик обновлён!");
    }

    static async Task DeleteRabbit()
    {
        Console.Write("\nВведите ID кролика для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) 
        {
            Console.WriteLine(" Ошибка: неверный ID!");
            return;
        }

        await apiService.DeleteRabbit(id);
        Console.WriteLine("V Кролик удалён!");
    }

}

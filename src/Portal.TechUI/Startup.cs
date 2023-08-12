using Microsoft.Extensions.Configuration;
using Portal.Common.Models;
using Portal.Common.Models.Enums;
using Portal.Services.InventoryServices;
using Portal.Services.MenuService;
using Portal.Services.OauthService;
using Portal.Services.PackageService;
using Portal.Services.UserService;
using Portal.Services.ZoneService;

namespace Portal.TechUI
{
    public class Startup
    {
        private readonly UserService _userService;
        private readonly OauthService _oauthService;
        private readonly ZoneService _zoneService;
        private readonly MenuService _menuService;
        private readonly PackageService _packageService;
        private readonly InventoryService _inventoryService;
        
        private readonly IConfiguration _config;

        private User? _currentUser;

        public Startup(IConfiguration config,
                       UserService userService,
                       OauthService oauthService,
                       ZoneService zoneService,
                       MenuService menuService,
                       PackageService packageService,
                       InventoryService inventoryService) 
        {
            _userService = userService;
            _oauthService = oauthService;
            _zoneService = zoneService;
            _menuService = menuService;
            _packageService = packageService;
            _inventoryService = inventoryService;

            _config = config;
            _currentUser = null;
        }

        public async Task RunAsync() 
        {
            int choice = -1;
            while (choice != 0)
            {
                if (_currentUser is null)
                {
                    Console.WriteLine($"\n Права доступа: Гость - неизвестный пользователь");
                    Console.WriteLine("1 - Зарегистрироваться");
                    Console.WriteLine("2 - Войти в аккаунт");
                    Console.WriteLine("3 - Просмотреть все зоны");
                    Console.WriteLine("4 - Просмотреть меню");
                    Console.WriteLine("5 - Просмотреть все пакеты");
                    Console.WriteLine("6 - Просмотреть расписание работы");
                    Console.WriteLine("0 - выход");
                    Console.Write("Выбор: ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                await Registration();
                                break;
                            case 2:
                                await LogIn();
                                break;
                            case 3:
                                await PrintZones();
                                break;
                            case 4:
                                await PrintMenu();
                                break;
                            case 5:
                                await PrintPackages();
                                break;
                            case 6:
                                PrintWorkTime();
                                break;
                            case 0:
                                Console.WriteLine("Выход");
                                break;
                            default:
                                Console.WriteLine($"Нет пункта под номером - {choice}");
                                break;
                        }
                    }

                }
                else if (_currentUser.Role == Role.User)
                {
                    Console.WriteLine($"\n Права доступа: Пользователь - {_currentUser.LastName} {_currentUser.FirstName}");
                    Console.WriteLine("1 - Просмотреть все зоны");
                    Console.WriteLine("2 - Просмотреть меню");
                    Console.WriteLine("3 - Просмотреть все пакеты");
                    Console.WriteLine("4 - Просмотреть расписание работы");
                    Console.WriteLine("5 - Просмотреть все свои брони");
                    Console.WriteLine("6 - Зайти в акаунт");
                    Console.WriteLine("7 - Выйти из аккаунта");
                    Console.WriteLine("0 - выход");
                    Console.Write("Выбор: ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                await PrintZones();
                                break;
                            case 2:
                                await PrintMenu();
                                break;
                            case 3:
                                await PrintPackages();
                                break;
                            case 4:
                                PrintWorkTime();
                                break;
                            case 0:
                                Console.WriteLine("Выход");
                                break;
                            default:
                                Console.WriteLine($"Нет пункта под номером - {choice}");
                                break;
                        }
                    }
                }
                else if (_currentUser.Role == Role.Administrator)
                {
                    Console.WriteLine($"\n Права доступа: Администратор - {_currentUser.LastName} {_currentUser.FirstName}");
                    Console.WriteLine("1 - Просмотреть все зоны");
                    Console.WriteLine("2 - Просмотреть меню");
                    Console.WriteLine("3 - Просмотреть все пакеты");
                    Console.WriteLine("4 - Просмотреть расписание работы");
                    Console.WriteLine("5 - Просмотреть всех пользователей");
                    Console.WriteLine("6 - Просмотреть все брони");
                    Console.WriteLine("7 - Панель администратора");
                    Console.WriteLine("8 - Зайти в профиль пользователя");
                    Console.WriteLine("9 - Выйти из аккаунта");
                    Console.WriteLine("0 - выход");
                    Console.Write("Выбор: ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                await PrintZones();
                                break;
                            case 2:
                                await PrintMenu();
                                break;
                            case 3:
                                await PrintPackages();
                                break;
                            case 4:
                                PrintWorkTime();
                                break;
                            case 7:
                                await AdminPanel();
                                break;
                            case 0:
                                Console.WriteLine("Выход");
                                break;
                            default:
                                Console.WriteLine($"Нет пункта под номером - {choice}");
                                break;
                        }
                    }
                }
            }
        }

        private async Task AdminPanel()
        {
            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("\n-- Админ панель --");
                Console.WriteLine("1 - Создать зону");
                Console.WriteLine("2 - Создать блюдо");
                Console.WriteLine("3 - Создать пакет");
                Console.WriteLine("4 - Создать бронь");
                Console.WriteLine("0 - Назад");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            await CreateZone();
                            break;
                        case 0:
                            Console.WriteLine("Выход");
                            break;
                        default:
                            Console.WriteLine($"Нет пункта под номером - {choice}");
                            break;
                    }
                }
            }
        }

        private async Task CreateZone()
        {
            Console.WriteLine("--------------------- Создание зоны ----------------------------");
            
            Console.Write("Введите название: ");
            var name = Console.ReadLine();
            if (name is null || name.Length < 3)
            {
                Console.WriteLine("Некорректно.");
                return;
            }
            
            Console.Write("Введите адресс: ");
            var address = Console.ReadLine();
            if (address is null || address.Length < 3)
            {
                Console.WriteLine("Некорректно.");
                return;
            }
            
            Console.Write("Введите размер в кв. м.: ");
            if (!double.TryParse(Console.ReadLine(), out var size) || size <= 0)
            {
                Console.WriteLine("Некорректно.");
                return;
            }
            
            Console.Write("Введите макс. количество людей: ");
            if (!int.TryParse(Console.ReadLine(), out var limit) || limit <= 0)
            {
                Console.WriteLine("Некорректно.");
                return;
            }
            
            Console.Write("Введите цену за час аренды: ");
            if (!double.TryParse(Console.ReadLine(), out var price) || price <= 0)
            {
                Console.WriteLine("Некорректно.");
                return;
            }

            var zoneId = await _zoneService.AddZoneAsync(name, address, size, limit, price);

            Console.WriteLine("Зона успешно создана");
            await PrintZone(zoneId);
        }
        
        
        private async Task PrintPackages()
        {
            var packages = await _packageService.GetPackagesAsync();

            if (packages.Count == 0)
            {
                Console.WriteLine("Пакетов нет.");
                return;
            }

            for (var i = 0; i < packages.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\tНазвание: {packages[i].Name}");
                Console.WriteLine($"  \tОписание: {packages[i].Description}");
                Console.WriteLine($"  \tЦена: {packages[i].Price} за {packages[i].RentalTime} час(ов)");
            }
            
            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("№ - Посмотреть пакет, к которой хотите перейти");
                Console.WriteLine("0 - Назад");
                
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice) && 0 < choice && choice <= packages.Count)
                {
                    await PrintPackage(packages[choice - 1].Id);
                }
            }
        }

        private async Task PrintPackage(Guid packageId)
        {
            var package = await _packageService.GetPackageById(packageId);
            
            Console.WriteLine($"Название: {package.Name}");
            Console.WriteLine($"Тип пакета: {package.Type}");
            Console.WriteLine($"Время проведения: {package.Description}");
            Console.WriteLine($"Цена: {package.Price}");
            Console.WriteLine($"Описание: {package.Description}");
            
            Console.WriteLine("Меню пакета: ");
            foreach (var dish in package.Dishes)
            {
                Console.WriteLine($"\t {dish.Name}");
            }
            
            Console.WriteLine("Пакеты применены к зонам/комнатам/зонам: ");
            foreach (var zone in package.Zones)
            {
                Console.WriteLine($"\t {zone.Name}");
            }
        }

        private void PrintWorkTime()
        {
            Console.WriteLine("Работаем с пн - вс с 10:00 до 22:00");
        }

        private async Task PrintMenu()
        {
            var menu = await _menuService.GetAllDishesAsync();
            
            if (menu.Count == 0)
            {
                Console.WriteLine("Меню нет.");
                return;
            }
            
            foreach (var dish in menu)
            {
                Console.WriteLine($"Название: {dish.Name}");
                Console.WriteLine($"Описание: {dish.Description}");
                Console.WriteLine($"Тип: {dish.Type}");
                Console.WriteLine($"Цена: {dish.Price} р.");
            }
        }

        private async Task PrintZones()
        {
            var zones = await _zoneService.GetAllZonesAsync();

            if (zones.Count == 0)
            {
                Console.WriteLine("Зон/Комнат/Залов нет.");
                return;
            }
            
            for (var i = 0; i < zones.Count; i++)
            {
                Console.WriteLine($"{i + 1}.\t Название: <<{zones[i].Name}>>");
                Console.WriteLine($"  \t Адрес: {zones[i].Address}");
            }
            
            
            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("№ - Посмотреть зону, к которой хотите перейти");
                Console.WriteLine("0 - Назад");
                
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice) && 0 < choice && choice <= zones.Count)
                {
                    await PrintZone(zones[choice - 1].Id);
                }
            }
            
        }

        private async Task PrintZone(Guid zoneId)
        {
            var zone = await _zoneService.GetZoneByIdAsync(zoneId);

            Console.WriteLine($"Название: {zone.Name}");
            Console.WriteLine($"Адрес: {zone.Address}");
            Console.WriteLine($"Размер: {zone.Size} кв. м.");
            Console.WriteLine($"Цена за час аренда: {zone.Price} р.");
            Console.WriteLine($"Максимальное количество людей: {zone.Limit}");
            Console.WriteLine($"Рейтинг: {zone.Rating}");
            Console.WriteLine("Инвентарь: ");
            foreach (var inv in zone.Inventories)
            {
                Console.Write($"{inv.Name}, ");
            }
            Console.WriteLine("Пакеты: ");
            foreach (var package in zone.Packages)
            {
                Console.WriteLine($"{package.Name}");
                Console.WriteLine($"\t{package.Description}");
            }

            if (_currentUser?.Role == Role.Administrator)
            {
                int choice = -1;
                while (choice != 0)
                {
                    Console.WriteLine("\n-- Админ панель --");
                    Console.WriteLine("1 - Изменить данные");
                    Console.WriteLine("2 - Добавить инвентарь");
                    Console.WriteLine("3 - Добавить пакет");
                    Console.WriteLine("0 - Назад");
                    Console.Write("Выбор: ");

                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                                await EditZone(zoneId);
                                break;
                            case 2:
                                await AddInventoryInZone(zoneId);
                                break;
                            case 3:
                                await AddPackageInZone(zoneId);
                                break;
                            case 0:
                                Console.WriteLine("Выход");
                                break;
                            default:
                                Console.WriteLine($"Нет пункта под номером - {choice}");
                                break;
                        }
                    }
                }
            }
        }

        private async Task AddPackageInZone(Guid zoneId)
        {
            var packages = await _packageService.GetPackagesAsync();

            if (packages.Count == 0)
            {
                Console.WriteLine("Пакетов нет.");
                return;
            }

            var zone = await _zoneService.GetZoneByIdAsync(zoneId);

            for (var i = 0; i < packages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {packages[i].Name}");
            }
            
            Console.Write("Выбор: ");
            if (!int.TryParse(Console.ReadLine(), out var choice) || choice <= 0 || choice > packages.Count)
            {
                Console.WriteLine("Ошибка ввода");
            }

            await _zoneService.AddPackageAsync(zone.Id, packages[choice - 1].Id);
            Console.WriteLine("Пакет успешно добавлен");
        }

        private async Task AddInventoryInZone(Guid zoneId)
        {
            // var inventories = await _inventoryService.GetAllInventoriesAsync();
            //
            // if (inventories.Count == 0)
            // {
            //     Console.WriteLine("Инветаря нет.");
            //     return;
            // }
            //
            // var zone = await _zoneService.GetZoneByIdAsync(zoneId);
            //
            // for (var i = 0; i < packages.Count; i++)
            // {
            //     Console.WriteLine($"{i + 1}. {packages[i].Name}");
            // }
            //
            // Console.Write("Выбор: ");
            // if (!int.TryParse(Console.ReadLine(), out var choice) || choice <= 0 || choice > packages.Count)
            // {
            //     Console.WriteLine("Ошибка ввода");
            // }
            //
            // await _zoneService.AddPackageAsync(zone.Id, packages[choice - 1].Id);
            // Console.WriteLine("Пакет успешно добавлен");
        }

        private async Task EditZone(Guid zoneId)
        {
            var zone = await _zoneService.GetZoneByIdAsync(zoneId);

            var choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("----------- Изменения ------------");
                Console.WriteLine("1 - Изменить название");
                Console.WriteLine("2 - Изменить адрес");
                Console.WriteLine("3 - Изменить размер");
                Console.WriteLine("4 - Изменить макс. количество людей");
                Console.WriteLine("5 - Изменить размер");
                Console.WriteLine("0 - Готово");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                        {
                            Console.Write("Введите новое название:");
                            var name = Console.ReadLine();
                        
                            if (name is null || name.Length < 3)
                            {
                                Console.WriteLine("Ошибка ввода.");
                            }
                            else
                            {
                                zone.Name = name;
                            }
                            
                            break;
                        }
                        case 2:
                        {
                            Console.Write("Введите новое значение площади:");
                            int size;
                            if (!int.TryParse(Console.ReadLine(), out size))
                            {
                                Console.WriteLine("Ошибка ввода.");
                            }
                            else
                            {
                                zone.Size = size;
                            }

                            break;
                        }
                        case 3:
                        {
                            Console.Write("Введите новое цену:");
                            if (!double.TryParse(Console.ReadLine(), out var price))
                            {
                                Console.WriteLine("Ошибка ввода.");
                            }
                            else
                            {
                                zone.ChangePrice(price);
                            }
                            
                            break;
                        }
                        case 0:
                        {
                            await _zoneService.UpdateZoneAsync(zone);
                            Console.WriteLine("Изменение прошло успешно");
                            break;
                        }
                    }
                }
            }
        }

        private async Task LogIn()
        {
            Console.WriteLine("------------ Авторизация --------------");
            Console.Write("Введите логин (email): ");
            var login = Console.ReadLine();
            if (login?.Length < 3)
            {
                Console.WriteLine("Некорректный логин.");
                return;
            }

            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();
            if (password?.Length < 8)
            {
                Console.WriteLine("Некорректный пароль.");
                return;
            }

            try
            {
                _currentUser = await _oauthService.LogIn(login!, password!);
                Console.WriteLine("Авторизация прошла успешна.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task Registration()
        {
            Console.WriteLine("------------- Регистрация --------------------");

            Console.Write("Введите вашу фамилию: ");
            var lastName = Console.ReadLine();
            if (lastName is null || lastName.Length < 3)
            {
                Console.WriteLine("Некорректно.");
                return;
            }

            Console.Write("Введите ваше имя: ");
            var firstName = Console.ReadLine();
            if (firstName is null || firstName.Length < 3)
            {
                Console.WriteLine("Некорректно.");
                return;
            }

            Console.Write("Введите ваше отчество (если есть): ");
            var middleName = Console.ReadLine();
            if (middleName is null || middleName.Length is > 0 and < 3)
            {
                Console.WriteLine("Некорректно.");
                return;
            }

            Console.Write("Введите дату рождения (17.12.2012): ");
            var birthday = DateTime.Parse(Console.ReadLine()!);

            Gender gender;
            Console.WriteLine("Выберите пол: ");
            Console.WriteLine("1. Мужской");
            Console.WriteLine("2. Женский");
            if (int.TryParse(Console.ReadLine(), out var choiceGender))
            {
                gender = choiceGender switch
                {
                    1 => Gender.Male,
                    2 => Gender.Female,
                    _ => Gender.Unknown,
                };
            } 
            else
            {
                Console.WriteLine("Некорректно.");
                return;
            }

            Console.Write("Введите номер телефон: ");
            var phone = Console.ReadLine();
            if (phone is null || phone.Length < 3)
            {
                Console.WriteLine("Некорректный номер.");
                return;
            }

            Console.Write("Введите email: ");
            var email = Console.ReadLine();
            if (email is null || email.Length < 3)
            {
                Console.WriteLine("Некорректный email.");
                return;
            }

            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();
            if (password is null || password.Length < 8)
            {
                Console.WriteLine("Некорректный пароль.");
                return;
            }

            Console.Write("Подтвердите пароль: ");
            var passwordch = Console.ReadLine();
            if (passwordch is null)
            {
                Console.WriteLine("Некорректный пароль.");
                return;
            }
            if (passwordch != password)
            {
                Console.WriteLine("Пароли не совпадают.");
                return;
            }

            try
            {
                var user = new User(Guid.NewGuid(), 
                    lastName, firstName, middleName, 
                    birthday, gender, email, phone);
                await _oauthService.Registrate(user, password);
                _currentUser = user;
                Console.WriteLine("Регистрация прошла успешна.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

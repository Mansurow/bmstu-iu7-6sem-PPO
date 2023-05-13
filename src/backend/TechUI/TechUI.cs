using System.Reflection.Metadata;
using Anticafe.BL.Exceptions;
using Anticafe.BL.Models;
using Anticafe.BL.Services.MenuService;
using Anticafe.BL.Sevices.BookingService;
using Anticafe.BL.Sevices.FeedbackService;
using Anticafe.BL.Sevices.OauthService;
using Anticafe.BL.Sevices.RoomService;
using Anticafe.BL.Sevices.UserService;
using Anticafe.Common.Enums;
using Microsoft.Extensions.Configuration;

namespace Anticafe.TechUI;

public class TechUI
{
    private readonly UserService _userService;
    private readonly RoomService _roomService;
    private readonly BookingService _bookingService;
    private readonly MenuService _menuService;
    private readonly Oauthservice _oauthService;
    private readonly FeedbackService _feedbackService;

    private readonly IConfiguration _configuration;

    private User? currentUser;

    public TechUI(UserService userService, 
                  RoomService roomService, 
                  BookingService bookingService, 
                  MenuService menuService, 
                  Oauthservice oauthService, 
                  FeedbackService feedbackService, 
                  IConfiguration configuration)
    {
        _userService = userService;
        _roomService = roomService;
        _bookingService = bookingService;
        _menuService = menuService;
        _oauthService = oauthService;
        _feedbackService = feedbackService;
        _configuration = configuration;

        _configuration["DbConnection"] = "guest";
        currentUser = null;
    }

    public void Run()
    {
        MainView();
    }

    private void MainView() 
    {
        
        int choice = -1;
        while (choice != 0) 
        {
            if (currentUser is null)
            {
                Console.WriteLine($"\nПрава доступа: {_configuration["DbConnection"]}");
                Console.WriteLine("1 - зарегистрироваться");
                Console.WriteLine("2 - войти в аккаунт");
                Console.WriteLine("3 - просмотр информации о зонах");
                Console.WriteLine("4 - просмотр информации о меню");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            UserRegister();
                            break;
                        case 2:
                            LogIn();
                            break;
                        case 3:
                            GetAllRooms();
                            break;
                        case 4:
                            GetMenu();
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
            else if (currentUser.Role == UserRole.User) 
            {
                Console.WriteLine("Права доступа: Пользователь");
                Console.WriteLine("1 - просмотр информации о зонах");
                Console.WriteLine("2 - просмотр информации о меню");
                Console.WriteLine("3 - выйти из акаунта");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            GetAllRooms();
                            break;
                        case 2:
                            GetMenu();
                            break;
                        case 3:
                            LogOut();
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
            else if (currentUser.Role == UserRole.Admin) 
            {
                
            }
        }
    }

    private void UserRegister()
    {
        Console.WriteLine("Регистрация");
        Console.Write("Введите логин: ");
        string? login = Console.ReadLine();
        if (login?.Length < 3)
        {
            Console.WriteLine("Некорректный логин.");
            return;
        }

        Console.Write("Введите пароль: ");
        string? password = Console.ReadLine();
        if (password?.Length < 8)
        {
            Console.WriteLine("Некорректный пароль.");
            return;
        }

        Console.Write("Введите пароль: ");
        string? passwordch = Console.ReadLine();
        if (passwordch is null || passwordch != password)
        {
            Console.WriteLine("Пароли не совпадают.");
            return;
        }

        try
        {
            currentUser = (_oauthService.Registrate(login!, password!)).Result;
            Console.WriteLine("Регистрация прошла успешна.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void LogIn() 
    {
        Console.WriteLine("Авторизация");
        Console.Write("Введите логин: ");
        string? login = Console.ReadLine();
        if (login?.Length < 3)
        {
            Console.WriteLine("Некорректный логин.");
            return;
        }

        Console.Write("Введите пароль: ");
        string? password = Console.ReadLine();
        if (password?.Length < 8)
        {
            Console.WriteLine("Некорректный пароль.");
            return;
        }

        try
        {
            currentUser = (_oauthService.LogIn(login!, password!)).Result;
            _configuration["DbConnection"] = currentUser.Role.ToString();
            Console.WriteLine("Авторизация прошла успешна.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void LogOut()
    {
        currentUser = null;
        _configuration["DbConnection"] = "guest";
        Console.WriteLine("Выход из аккаунта произошел успешно.");
    }

    private void GetMenu() 
    {
        var menu = (_menuService.GetAllDishesAsync()).Result;
        PrintMenu(menu);
    }

    private void PrintMenu(List<Menu> menu) 
   {
        if (menu.Count == 0)
            Console.WriteLine("Меню отсутствует.");
        else
            for (int i = 0; i < menu.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {menu[i].Name}, {menu[i].Price}");
            }
    }

    private void GetAllRooms() 
    {
        var allRooms = (_roomService.GetAllRoomsAsync()).Result;
        PrintRooms(allRooms);

        int choice = -1;
        while (choice != 0) 
        {
            if (currentUser is null) 
            {
                Console.WriteLine($"\nПрава доступа: Гость");
                Console.WriteLine("Хотите просмотреть подробную информацию про зону?");
                Console.WriteLine("Ввведите номер зоны.");
                Console.WriteLine("0 - Нет");
                Console.Write("Выбор: ");
                if (int.TryParse(Console.ReadLine(), out choice) && choice > 0 && choice <= allRooms.Count)
                {
                    try
                    {
                        var actualRoom = (_roomService.GetRoomByIdAsync(allRooms[choice - 1].Id)).Result;
                        PrintRoom(actualRoom);
                    }
                    catch (RoomNotFoundException)
                    {
                        Console.WriteLine("Такой зоны нет");
                    }
                }
                else
                    Console.WriteLine("Ошибка при выборе!");
            }
            
        }
    }

    private void PrintRooms(List<Room> rooms) 
    {
        if (rooms.Count == 0)
            Console.WriteLine("Нет зон.");
        else
            for (int i = 0; i < rooms.Count; i++) 
            {
                Console.WriteLine($"{i + 1}) {rooms[i].Name}");
            }
    }

    private void PrintRoom(Room room)
    {
        Console.WriteLine($"Название: {room.Name}");
        Console.WriteLine($"Площадь: {room.Size} кв.м.");
        Console.WriteLine($"Price: {room.Price} Р.");
        if (room.Inventories is not null && room.Inventories.Count > 0) 
        {
            PrintInventory(room.Inventories);
        } else
            Console.WriteLine($"Инвентарь: не доступен");
    }

    private void PrintInventory(ICollection<Inventory> inventories) 
    {
        Console.Write("Интыентарь: ");
        foreach (var i in inventories) 
        {
            Console.Write($"{i.Name} ");
        }
    }
}

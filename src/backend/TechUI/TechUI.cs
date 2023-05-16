using System.Security;
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
                Console.WriteLine($"\n Права доступа: Гость");
                Console.WriteLine("1 - зарегистрироваться");
                Console.WriteLine("2 - войти в аккаунт");
                Console.WriteLine("3 - просмотр информации о зонах");
                Console.WriteLine("4 - просмотр информации о конкретной зоне");
                Console.WriteLine("5 - просмотр информации о меню (все блюда)");
                Console.WriteLine("6 - просмотр информации о конкретном блюде");
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
                            PrintRooms();
                            break;
                        case 4:
                            PrintRoom();
                            break;
                        case 5:
                            PrintMenu();
                            break;
                        case 6:
                            PrintDish();
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
                Console.WriteLine("\n Права доступа: Пользователь");
                Console.WriteLine("1 - просмотр информации о зонах");
                Console.WriteLine("2 - просмотр информации о конкретной зоне");
                Console.WriteLine("3 - просмотр информации о меню (все блюда)");
                Console.WriteLine("4 - просмотр информации о конкретном блюде");
                Console.WriteLine("5 - просмотр всех своих броней");
                Console.WriteLine("6 - забронировать конкретную зону");
                Console.WriteLine("7 - отменить бронь");
                Console.WriteLine("8 - оставить отзыв для конкретной зоны");
                Console.WriteLine("9 - выйти из акаунта");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            PrintRooms();
                            break;
                        case 2:
                            PrintRoom();
                            break;
                        case 3:
                            PrintMenu();
                            break;
                        case 4:
                            PrintDish();
                            break;
                        case 5:
                            PrintUserBookings();
                            break;
                        case 6:
                            BookRoom();
                            break;
                        case 7:
                            CancellBooking();
                            break;
                        case 8:
                            AddFeedback();
                            break;
                        case 9:
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
                Console.WriteLine("\n Права доступа: Админ");
                Console.WriteLine("------------Зона---------------");
                Console.WriteLine("1 - добавить информацию о зоне");
                Console.WriteLine("2 - обновить информацию о зоне");
                Console.WriteLine("3 - удалить информацию о зоне");
                Console.WriteLine("4 - просмотр информации о зонах");
                Console.WriteLine("5 - просмотр информации о конкретной зоне");
                Console.WriteLine("------------Меню---------------");
                Console.WriteLine("6 - добавить информацию о блюде");
                Console.WriteLine("7 - обновить информацию о блюде");
                Console.WriteLine("8 - удалить информацию о блюде");
                Console.WriteLine("9 - просмотр информации о меню (все блюда)");
                Console.WriteLine("10 - просмотр информации о конкретном блюде");
                Console.WriteLine("------------Бронь---------------");
                Console.WriteLine("11 - просмотр всех броней пользователей");
                Console.WriteLine("------------Отзыв---------------");
                Console.WriteLine("12 - просмотреть отзывы для конкретной зоны");
                Console.WriteLine("13 - удалить отзыв");
                Console.WriteLine("------------Пользователь---------------");
                Console.WriteLine("14 - Вывести всех пользователей");
                Console.WriteLine("15 - Назначить пользователя админом");
                Console.WriteLine("------------Другое---------------");
                Console.WriteLine("16 - выйти из акаунта");
                Console.WriteLine("0 - выход");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddRoom();
                            break;
                        case 2:
                            UpdateRoom();
                            break;
                        case 3:
                            DeleteRoom();
                            break;
                        case 4:
                            PrintRooms();
                            break;
                        case 5:
                            PrintRoom();
                            break;
                        case 6:
                            AddDish();
                            break;
                        case 7:
                            UpdateDish();
                            break;
                        case 8:
                            DeleteDish();
                            break;
                        case 9:
                            PrintMenu();
                            break;
                        case 10:
                            PrintDish();
                            break;
                        case 11:
                            PrintAllBookings();
                            break;
                        case 12:
                            PrintRoomFeedbacks();
                            break;
                        case 13:
                            DeleteFeedback();
                            break;
                        case 14:
                            PrintUsers();
                            break;
                        case 15:
                            AssignAdmin();
                            break;
                        case 16:
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
        }
    }

    private void AssignAdmin() 
    {
        Console.Write("Ввведите id пользователя: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try
        {
             _userService.ChangeUserPermissionsAsync(id);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
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
            currentUser = _oauthService.Registrate(login!, password!).Result;
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
            currentUser = _oauthService.LogIn(login!, password!).Result;
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

    private void BookRoom() 
    {
        Console.Write("Ввведите id комнаты: ");
        int roomId;
        if (!int.TryParse(Console.ReadLine(), out roomId))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        Console.Write("Ввведите количество людей: ");
        int amount;
        if (!int.TryParse(Console.ReadLine(), out amount))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        Console.WriteLine("Ввведите время для брони (в формате 2023-04-20 12:00):");
        Console.Write("Начало: ");
        string sTime = Console.ReadLine()!;
        Console.Write("Конец: ");
        string eTime = Console.ReadLine()!;

        try
        {
            _bookingService.CreateBookingAsync(currentUser!.Id, roomId, amount, sTime, eTime);
            Console.WriteLine("Зона забронирована.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void CancellBooking() 
    {
        Console.Write("Ввведите id брони: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }
        try
        {
            _bookingService.DeleteBookingAsync(id);
            Console.WriteLine("Бронь успешна отменена.");
        } catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void AddRoom() 
    {
        Console.Write("Введите название:");
        string name = Console.ReadLine()!;
        Console.Write("Ввведите площадь зоны:");
        int size;
        if (!int.TryParse(Console.ReadLine(), out size))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }
        Console.Write("Ввведите стоимость брони в час:");
        double price;
        if (!double.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }
        try
        {
            _roomService.AddRoomAsync(name, size, price);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void UpdateRoom() 
    {
        Console.Write("Ввведите id зоны:");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try
        {
            var room = _roomService.GetRoomByIdAsync(id).Result;

            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("Изменить информацию о зоне:");
                Console.WriteLine($"1 - Название, текущее название - {room.Name}");
                Console.WriteLine($"2 - Площадь в кв.м., текущее значение - {room.Size} кв.м.");
                Console.WriteLine($"3 - Цену в рублях, текущее цена - {room.Price} р.");
                Console.WriteLine($"0 - Готово");
                Console.Write("Выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice == 1)
                    {
                        Console.Write("Введите новое название:");
                        string name = Console.ReadLine()!;
                        room.Name = name;
                    }
                    else if (choice == 2)
                    {
                        Console.Write("Введите новое значение площади:");
                        int size;
                        if (!int.TryParse(Console.ReadLine(), out size))
                        {
                            Console.WriteLine("Ошибка ввода.");
                            return;
                        }

                        room.Size = size;
                    }
                    else if (choice == 3)
                    {
                        Console.Write("Введите новое цену:");
                        double price;
                        if (!double.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Ошибка ввода.");
                            return;
                        }

                        room.ChangePrice(price);
                    }
                }


            }

            await _roomService.UpdateRoomAsync(room);
            Console.WriteLine("Данные успешно обновлены.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void DeleteRoom() 
    {
        Console.Write("Ввведите id зоны:");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try 
        {
           _roomService.DeleteRoomAsync(id);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private void AddDish() 
    {
        Console.Write("Введите название: ");
        string name = Console.ReadLine()!;
        Console.Write("Выберите тип: ");
        DishType type;
        if (!DishType.TryParse(Console.ReadLine(), out type))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }
        Console.Write("Ввведите стоимость брони в час:");
        double price;
        if (!double.TryParse(Console.ReadLine(), out price))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }
        Console.Write("Введите описание: ");
        string description = Console.ReadLine()!;

        try
        {
            _menuService.AddDishAsync(name, type, price, description);
            Console.WriteLine("Блюдо внесено в меню.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void UpdateDish() 
    {
        Console.Write("Ввведите id блюда: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try
        {
            var dish = _menuService.GetDishByIdAsync(id).Result;

            int choice = -1;
            while (choice != 0)
            {
                Console.WriteLine("Изменить информацию о зоне:");
                Console.WriteLine($"1 - Название, текущее название - {dish.Name}");
                Console.WriteLine($"2 - Тип блюда - {dish.Type} кв.м.");
                Console.WriteLine($"3 - Цену в рублях, текущее цена - {dish.Price} р.");
                Console.WriteLine($"4 - Описание.");
                Console.WriteLine($"0 - Готово");
                Console.Write("Выбор: ");
                
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    if (choice == 1)
                    {
                        Console.Write("Введите новое название:");
                        string name = Console.ReadLine()!;
                        dish.Name = name;
                    }
                    else if (choice == 2)
                    {
                        Console.Write("Введите тип блюда:");
                        DishType type;
                        if (!DishType.TryParse(Console.ReadLine(), out type))
                        {
                            Console.WriteLine("Ошибка ввода.");
                            return;
                        }

                        dish.Type = type;
                    }
                    else if (choice == 3)
                    {
                        Console.Write("Введите новое цену:");
                        double price;
                        if (!double.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Ошибка ввода.");
                            return;
                        }

                        dish.Price = price;
                    }
                }


            }

            _menuService.UpdateDishAsync(dish);
            Console.WriteLine("Данные успешно обновлены.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private void DeleteDish() 
    {
        Console.Write("Ввведите id блюда: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try 
        {
            _menuService.DeleteDishAsync(id);
        } catch (Exception ex) 
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void AddFeedback() 
    {
        Console.Write("Ввведите id зоны: ");
        int roomId;
        if (!int.TryParse(Console.ReadLine(), out roomId))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        Console.Write("Ввведите оценку (от 0-5): ");
        int rating;
        if (!int.TryParse(Console.ReadLine(), out rating) && rating >= 0 && rating <= 5)
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        Console.Write("Введите комментарий: ");
        string? msg = Console.ReadLine();

        try
        {
            _feedbackService.AddFeedbackAsync(currentUser!.Id, roomId, rating, msg);
            Console.WriteLine("Отзыв оставел");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void DeleteFeedback() 
    {
        Console.Write("Ввведите id отзыва: ");
        int id;
        if (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try 
        {
             _feedbackService.DeleteFeedbackAsync(id);
            Console.WriteLine("Отзыв удален.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void PrintRoomFeedbacks() 
    {
        Console.Write("Ввведите id комнаты: ");
        int roomId;
        if (!int.TryParse(Console.ReadLine(), out roomId))
        {
            Console.WriteLine("Ошибка ввода.");
            return;
        }

        try
        {
            var feedbacks = _feedbackService.GetAllFeedbackByRoomAsync(roomId).Result;
            foreach(var f in feedbacks)
            {
                Console.WriteLine($"Id: {f.Id}, Rating:{f.Mark}, Date: {f.Date}");
                Console.WriteLine($"Комментарий: {f.Message}");
            }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void PrintUserBookings() 
    {
        try 
        {
            var bookings = _bookingService.GetBookingByUserAsync(currentUser!.Id).Result;
            if (bookings.Count == 0)
                Console.WriteLine("У вас нет броней");
            else
                foreach(var b in bookings) 
                {
                    var room = _roomService.GetRoomByIdAsync(b.RoomId).Result;
                    Console.WriteLine($"Id: {b.Id}, Зона: {room.Name}, Время брони: {b.StartTime} - {b.EndTime}, Состоние: {b.Status}");
                }
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void PrintAllBookings()
    {
        try
        {
            var bookings = _bookingService.GetAllBookingAsync().Result;
            foreach (var b in bookings)
            {
                var room = _roomService.GetRoomByIdAsync(b.RoomId).Result;
                Console.WriteLine($"Id: {b.Id}, UserId: {b.UserId}, Зона: {room.Name}, Время брони: {b.StartTime} - {b.EndTime}, Состоние: {b.Status}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void PrintUsers() 
    {
        var users = _userService.GetAllUsersAsync().Result;

        foreach (var u in users) 
        {
            Console.WriteLine($"Id: {u.Id}, ФИО: {u.Surname} {u.Name} {u.FirstName}, Пол: {u.Gender}, Email: {u.Email}, Дата рождения: {u.Birthday}, Телефон: {u.Phone}, Права доступа: ${u.Role}");
        }
    }

    private void PrintMenu() 
    {
        var menu = _menuService.GetAllDishesAsync().Result;
        if (menu.Count == 0)
            Console.WriteLine("Меню отсутствует.");
        else
            for (int i = 0; i < menu.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {menu[i].Name}, {menu[i].Price}");
            }
    }

    private void PrintDish() 
    {
        try
        {
            Console.Write("Ввведите id блюда: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка ввода.");
                return;
            }

            var dish = _menuService.GetDishByIdAsync(id).Result;
            Console.WriteLine($"Id: {dish.Id}");
            Console.WriteLine($"Название: {dish.Name}");
            Console.WriteLine($"Цена: {dish.Price} кв.м.");
            Console.WriteLine($"Описание: {dish.Description} Р.");
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void PrintRooms() 
    {
        var rooms = _roomService.GetAllRoomsAsync().Result;
        if (rooms.Count == 0)
            Console.WriteLine("Нет зон.");
        else
            foreach (var r in rooms)
            {
                Console.WriteLine($"Id:{r.Id} Name: {r.Name}");
            }
    }

    private void PrintRoom()
    {
        try
        {
            Console.WriteLine("Перейти к конкретной зоны?");
            Console.Write("Ввведите id зоны: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Ошибка ввода.");
                return;
            }

            var room = _roomService.GetRoomByIdAsync(id).Result;
            Console.WriteLine($"Название: {room.Name}");
            Console.WriteLine($"Площадь: {room.Size} кв.м.");
            Console.WriteLine($"Price: {room.Price} Р.");
            if (room.Inventories is not null && room.Inventories.Count > 0) 
            {
                PrintInventory(room.Inventories);
            } else
                Console.WriteLine($"Инвентарь: не доступен");
            }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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

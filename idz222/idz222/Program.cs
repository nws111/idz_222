using idz222.Sv;
using System;

namespace idz222
{
    class Program
    {
        static void Main()
        {
            var accessService = new AccessControlService();
            var requestService = new AccessRequestService();
            var reportService = new ReportService();

            Console.WriteLine("Система контроля доступа в режимное помещение");
            Console.WriteLine("--------------------------------------------");

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Проверить доступ сотрудника");
                Console.WriteLine("2. Зарегистрировать вход");
                Console.WriteLine("3. Зарегистрировать выход");
                Console.WriteLine("4. Подать заявку на доступ");
                Console.WriteLine("5. Одобрение и отклонение заявок");
                Console.WriteLine("6. Сформировать отчет");
                Console.WriteLine("0. Выход");

                Console.Write("Выберите действие: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CheckAccess(accessService);
                        break;
                    case "2":
                        RegisterEntry(accessService);
                        break;
                    case "3":
                        RegisterExit(accessService);
                        break;
                    case "4":
                        CreateRequest(requestService);
                        break;
                    case "5": 
                        ProcessRequest(requestService);
                        break;
                    case "6":
                        GenerateReport(reportService);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void CheckAccess(AccessControlService service)
        {
            Console.Write("Введите ID сотрудника: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                bool hasAccess = service.CheckAccess(id);
                Console.WriteLine(hasAccess ? "Доступ разрешен" : "Доступ запрещен");
            }
            else
            {
                Console.WriteLine("Некорректный ID!");
            }
        }

        static void RegisterEntry(AccessControlService accessService)
        {
            Console.Write("Введите ID сотрудника: ");

            if (!int.TryParse(Console.ReadLine(), out int employeeId))
            {
                Console.WriteLine("Ошибка: Некорректный формат ID");
                return;
            }

            var (success, message) = accessService.RegisterEntry(employeeId);

            if (success)
            {
                Console.WriteLine($"Успех: {message}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка: {message}");
            }
            
        }

        static void RegisterExit(AccessControlService service)
        {
            Console.Write("Введите ID сотрудника: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                service.RegisterExit(id);
                Console.WriteLine("Выход зарегистрирован");
            }
            else
            {
                Console.WriteLine("Некорректный ID!");
            }
        }

        static void CreateRequest(AccessRequestService service)
        {
            Console.Write("Введите ID сотрудника: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                Console.Write("Тип заявки (Grant/Revoke): ");
                string type = Console.ReadLine();
                if (type == "Grant" || type == "Revoke")
                {
                    service.CreateRequest(id, type);
                    Console.WriteLine("Заявка создана");
                }
                else
                {
                    Console.WriteLine("Некорректный тип заявки!");
                }
            }
            else
            {
                Console.WriteLine("Некорректный ID!");
            }
        }

        static void GenerateReport(ReportService service)
        {
            Console.WriteLine("\n" + service.GenerateAccessReport());
        }
        static void ProcessRequest(AccessRequestService service)
        {
            Console.WriteLine("\n=== Обработка заявок ===");

            Console.Write("\nВведите ID заявки: ");
            if (!int.TryParse(Console.ReadLine(), out int requestId))
            {
                Console.WriteLine("Некорректный ID заявки!");
                return;
            }

            Console.WriteLine("\n1. Одобрить");
            Console.WriteLine("2. Отклонить");
            Console.Write("Выберите действие: ");

            var action = Console.ReadLine();
            switch (action)
            {
                case "1":
                    service.ApproveRequest(requestId);
                    Console.WriteLine("Заявка одобрена");
                    break;
                case "2":
                    service.RejectRequest(requestId);
                    Console.WriteLine("Заявка отклонена");
                    break;
                default:
                    Console.WriteLine("Неверный выбор действия!");
                    break;
            }
        }
    }
}
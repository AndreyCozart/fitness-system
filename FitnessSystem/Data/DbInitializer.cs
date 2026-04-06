using FitnessSystem.Models;

namespace FitnessSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Проверяем, есть ли уже данные
            if (context.Clients.Any() && context.MembershipTypes.Any() && context.GroupClasses.Any())
            {
                return; // Данные уже есть, ничего не делаем
            }

            // 1. Добавляем типы абонементов (только если их нет)
            if (!context.MembershipTypes.Any())
            {
                var membershipTypes = new MembershipType[]
                {
                    new MembershipType { TypeName = "Разовое посещение", VisitsCount = 1, DurationDays = 1, Price = 500, Description = "Однократное посещение тренажерного зала", IsActive = true },
                    new MembershipType { TypeName = "Абонемент на 4 посещения", VisitsCount = 4, DurationDays = 30, Price = 1800, Description = "4 посещения в течение месяца", IsActive = true },
                    new MembershipType { TypeName = "Абонемент на 8 посещений", VisitsCount = 8, DurationDays = 30, Price = 3200, Description = "8 посещений в течение месяца", IsActive = true },
                    new MembershipType { TypeName = "Безлимитный на месяц", VisitsCount = null, DurationDays = 30, Price = 4000, Description = "Неограниченное количество посещений в течение месяца", IsActive = true },
                    new MembershipType { TypeName = "Утренний (до 16:00)", VisitsCount = null, DurationDays = 30, Price = 3000, Description = "Безлимитный абонемент на посещения до 16:00", IsActive = true },
                    new MembershipType { TypeName = "Вечерний (после 16:00)", VisitsCount = null, DurationDays = 30, Price = 3500, Description = "Безлимитный абонемент на посещения после 16:00", IsActive = true },
                    new MembershipType { TypeName = "Персональная тренировка", VisitsCount = 1, DurationDays = 1, Price = 1500, Description = "Индивидуальная тренировка с тренером", IsActive = true },
                    new MembershipType { TypeName = "Безлимитный на 3 месяца", VisitsCount = null, DurationDays = 90, Price = 10000, Description = "Неограниченное количество посещений в течение 3 месяцев", IsActive = true },
                    new MembershipType { TypeName = "Безлимитный на год", VisitsCount = null, DurationDays = 365, Price = 35000, Description = "Неограниченное количество посещений в течение года", IsActive = true }
                };

                foreach (var type in membershipTypes)
                {
                    context.MembershipTypes.Add(type);
                }
                context.SaveChanges();
            }

            // 2. Добавляем тестовых клиентов (только если их нет)
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client {
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович",
                        BirthDate = new DateTime(1990, 1, 1),
                        Gender = "M",
                        Phone = "+7 (999) 123-45-67",
                        Email = "ivanov@mail.ru",
                        RegistrationDate = DateTime.Now.AddDays(-30),
                        Notes = "Постоянный клиент"
                    },
                    new Client {
                        LastName = "Петрова",
                        FirstName = "Анна",
                        MiddleName = "Сергеевна",
                        BirthDate = new DateTime(1995, 5, 15),
                        Gender = "F",
                        Phone = "+7 (999) 765-43-21",
                        Email = "petrova@mail.ru",
                        RegistrationDate = DateTime.Now.AddDays(-15),
                        Notes = "Ходит на йогу"
                    },
                    new Client {
                        LastName = "Сидоров",
                        FirstName = "Петр",
                        MiddleName = "Алексеевич",
                        BirthDate = new DateTime(1988, 10, 20),
                        Gender = "M",
                        Phone = "+7 (999) 555-12-34",
                        Email = "sidorov@mail.ru",
                        RegistrationDate = DateTime.Now.AddDays(-45),
                        Notes = "Тренажерный зал"
                    },
                    new Client {
                        LastName = "Смирнова",
                        FirstName = "Елена",
                        MiddleName = "Дмитриевна",
                        BirthDate = new DateTime(1992, 3, 8),
                        Gender = "F",
                        Phone = "+7 (999) 222-33-44",
                        Email = "smirnova@mail.ru",
                        RegistrationDate = DateTime.Now.AddDays(-7),
                        Notes = "Новый клиент"
                    },
                    new Client {
                        LastName = "Козлов",
                        FirstName = "Андрей",
                        MiddleName = "Николаевич",
                        BirthDate = new DateTime(1985, 7, 12),
                        Gender = "M",
                        Phone = "+7 (999) 888-99-00",
                        Email = "kozlov@mail.ru",
                        RegistrationDate = DateTime.Now.AddDays(-60),
                        Notes = "Любитель групповых занятий"
                    }
                };

                foreach (var client in clients)
                {
                    context.Clients.Add(client);
                }
                context.SaveChanges();
            }

            // 3. Добавляем групповые занятия (только если их нет)
            if (!context.GroupClasses.Any())
            {
                var groupClasses = new GroupClass[]
                {
                    new GroupClass {
                        Name = "Йога",
                        Instructor = "Елена Смирнова",
                        DayOfWeek = "Понедельник",
                        StartTime = new TimeSpan(10, 0, 0),
                        EndTime = new TimeSpan(11, 0, 0),
                        MaxParticipants = 15,
                        Room = "Зал йоги",
                        Description = "Хатха-йога для начинающих. Развитие гибкости и снятие стресса.",
                        Price = 400,
                        IsActive = true
                    },
                    new GroupClass {
                        Name = "Пилатес",
                        Instructor = "Анна Иванова",
                        DayOfWeek = "Вторник",
                        StartTime = new TimeSpan(18, 30, 0),
                        EndTime = new TimeSpan(19, 30, 0),
                        MaxParticipants = 12,
                        Room = "Малый зал",
                        Description = "Укрепление мышц кора, улучшение осанки и координации.",
                        Price = 450,
                        IsActive = true
                    },
                    new GroupClass {
                        Name = "Аэробика",
                        Instructor = "Мария Петрова",
                        DayOfWeek = "Среда",
                        StartTime = new TimeSpan(19, 0, 0),
                        EndTime = new TimeSpan(20, 0, 0),
                        MaxParticipants = 20,
                        Room = "Основной зал",
                        Description = "Интенсивная кардио-тренировка под музыку. Сжигание калорий.",
                        Price = 350,
                        IsActive = true
                    },
                    new GroupClass {
                        Name = "Танцы",
                        Instructor = "Дмитрий Сидоров",
                        DayOfWeek = "Четверг",
                        StartTime = new TimeSpan(20, 0, 0),
                        EndTime = new TimeSpan(21, 0, 0),
                        MaxParticipants = 16,
                        Room = "Основной зал",
                        Description = "Zumba и латина. Зажигательные танцы для хорошего настроения.",
                        Price = 400,
                        IsActive = true
                    },
                    new GroupClass {
                        Name = "Йога",
                        Instructor = "Елена Смирнова",
                        DayOfWeek = "Суббота",
                        StartTime = new TimeSpan(11, 0, 0),
                        EndTime = new TimeSpan(12, 0, 0),
                        MaxParticipants = 15,
                        Room = "Зал йоги",
                        Description = "Йога выходного дня для восстановления сил.",
                        Price = 400,
                        IsActive = true
                    },
                    new GroupClass {
                        Name = "Стретчинг",
                        Instructor = "Анна Иванова",
                        DayOfWeek = "Пятница",
                        StartTime = new TimeSpan(18, 0, 0),
                        EndTime = new TimeSpan(19, 0, 0),
                        MaxParticipants = 15,
                        Room = "Малый зал",
                        Description = "Растяжка и развитие гибкости всего тела.",
                        Price = 350,
                        IsActive = true
                    }
                };

                foreach (var gc in groupClasses)
                {
                    context.GroupClasses.Add(gc);
                }
                context.SaveChanges();
            }

            // 4. Добавляем абонементы для клиентов (только если их нет)
            if (!context.Memberships.Any() && context.Clients.Any() && context.MembershipTypes.Any())
            {
                var clients = context.Clients.ToList();
                var membershipTypes = context.MembershipTypes.ToList();

                var memberships = new Membership[]
                {
                    new Membership {
                        ClientId = clients[0].Id, // Иванов
                        TypeId = membershipTypes.First(t => t.TypeName == "Безлимитный на месяц").Id,
                        StartDate = DateTime.Today.AddDays(-10),
                        EndDate = DateTime.Today.AddDays(20),
                        VisitsRemaining = -1,
                        Status = "active",
                        PurchaseDate = DateTime.Now.AddDays(-10)
                    },
                    new Membership {
                        ClientId = clients[0].Id, // Иванов
                        TypeId = membershipTypes.First(t => t.TypeName == "Абонемент на 4 посещения").Id,
                        StartDate = DateTime.Today.AddDays(-5),
                        EndDate = DateTime.Today.AddDays(25),
                        VisitsRemaining = 2,
                        Status = "active",
                        PurchaseDate = DateTime.Now.AddDays(-5)
                    },
                    new Membership {
                        ClientId = clients[1].Id, // Петрова
                        TypeId = membershipTypes.First(t => t.TypeName == "Утренний (до 16:00)").Id,
                        StartDate = DateTime.Today.AddDays(-20),
                        EndDate = DateTime.Today.AddDays(10),
                        VisitsRemaining = -1,
                        Status = "active",
                        PurchaseDate = DateTime.Now.AddDays(-20)
                    },
                    new Membership {
                        ClientId = clients[2].Id, // Сидоров
                        TypeId = membershipTypes.First(t => t.TypeName == "Разовое посещение").Id,
                        StartDate = DateTime.Today.AddDays(-1),
                        EndDate = DateTime.Today,
                        VisitsRemaining = 0,
                        Status = "expired",
                        PurchaseDate = DateTime.Now.AddDays(-1)
                    },
                    new Membership {
                        ClientId = clients[3].Id, // Смирнова
                        TypeId = membershipTypes.First(t => t.TypeName == "Персональная тренировка").Id,
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddDays(1),
                        VisitsRemaining = 1,
                        Status = "active",
                        PurchaseDate = DateTime.Now
                    }
                };

                foreach (var m in memberships)
                {
                    context.Memberships.Add(m);
                }
                context.SaveChanges();
            }

            // 5. Добавляем посещения (только если их нет)
            if (!context.Visits.Any() && context.Clients.Any() && context.Memberships.Any())
            {
                var clients = context.Clients.ToList();
                var memberships = context.Memberships.ToList();

                var visits = new Visit[]
                {
                    new Visit {
                        ClientId = clients[0].Id,
                        MembershipId = memberships[0].Id,
                        CheckInTime = DateTime.Today.AddHours(10),
                        CheckOutTime = DateTime.Today.AddHours(11).AddMinutes(30)
                    },
                    new Visit {
                        ClientId = clients[1].Id,
                        MembershipId = memberships[2].Id,
                        CheckInTime = DateTime.Today.AddHours(15),
                        CheckOutTime = DateTime.Today.AddHours(16).AddMinutes(15)
                    },
                    new Visit {
                        ClientId = clients[0].Id,
                        MembershipId = memberships[1].Id,
                        CheckInTime = DateTime.Today.AddHours(18),
                        CheckOutTime = null // Сейчас в зале
                    },
                    new Visit {
                        ClientId = clients[2].Id,
                        MembershipId = memberships[3].Id,
                        CheckInTime = DateTime.Today.AddDays(-1).AddHours(19),
                        CheckOutTime = DateTime.Today.AddDays(-1).AddHours(20).AddMinutes(30)
                    },
                    new Visit {
                        ClientId = clients[3].Id,
                        MembershipId = memberships[4].Id,
                        CheckInTime = DateTime.Today.AddHours(9),
                        CheckOutTime = DateTime.Today.AddHours(10)
                    }
                };

                foreach (var v in visits)
                {
                    context.Visits.Add(v);
                }
                context.SaveChanges();
            }

            // 6. Добавляем записи на групповые занятия (только если их нет)
            if (!context.GroupClassBookings.Any() && context.Clients.Any() && context.GroupClasses.Any())
            {
                var clients = context.Clients.ToList();
                var groupClasses = context.GroupClasses.ToList();

                var bookings = new GroupClassBooking[]
                {
                    new GroupClassBooking {
                        GroupClassId = groupClasses.First(g => g.Name == "Йога" && g.DayOfWeek == "Понедельник").Id,
                        ClientId = clients[1].Id, // Петрова
                        ClassDate = DateTime.Today.AddDays(1),
                        BookingDate = DateTime.Now.AddDays(-2),
                        IsAttended = false
                    },
                    new GroupClassBooking {
                        GroupClassId = groupClasses.First(g => g.Name == "Пилатес").Id,
                        ClientId = clients[1].Id, // Петрова
                        ClassDate = DateTime.Today.AddDays(2),
                        BookingDate = DateTime.Now.AddDays(-1),
                        IsAttended = false
                    },
                    new GroupClassBooking {
                        GroupClassId = groupClasses.First(g => g.Name == "Аэробика").Id,
                        ClientId = clients[4].Id, // Козлов
                        ClassDate = DateTime.Today.AddDays(3),
                        BookingDate = DateTime.Now.AddDays(-3),
                        IsAttended = false
                    }
                };

                foreach (var b in bookings)
                {
                    context.GroupClassBookings.Add(b);
                }
                context.SaveChanges();
            }
        }
    }
}
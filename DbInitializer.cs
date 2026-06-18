using FitnessSystem.Models;

namespace FitnessSystem.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Clients.Any() && context.MembershipTypes.Any() && context.GroupClasses.Any())
                return;

            // 1. Типы абонементов (не меняем)
            if (!context.MembershipTypes.Any())
            {
                var membershipTypes = new MembershipType[]
                {
                    new MembershipType { TypeName = "Разовое посещение",        VisitsCount = 1,    DurationDays = 1,   Price = 500,   Description = "Однократное посещение тренажерного зала",                 IsActive = true },
                    new MembershipType { TypeName = "Абонемент на 4 посещения", VisitsCount = 4,    DurationDays = 30,  Price = 1800,  Description = "4 посещения в течение месяца",                            IsActive = true },
                    new MembershipType { TypeName = "Абонемент на 8 посещений", VisitsCount = 8,    DurationDays = 30,  Price = 3200,  Description = "8 посещений в течение месяца",                            IsActive = true },
                    new MembershipType { TypeName = "Безлимитный на месяц",     VisitsCount = null, DurationDays = 30,  Price = 4000,  Description = "Неограниченное количество посещений в течение месяца",    IsActive = true },
                    new MembershipType { TypeName = "Утренний (до 16:00)",      VisitsCount = null, DurationDays = 30,  Price = 3000,  Description = "Безлимитный абонемент на посещения до 16:00",             IsActive = true },
                    new MembershipType { TypeName = "Вечерний (после 16:00)",   VisitsCount = null, DurationDays = 30,  Price = 3500,  Description = "Безлимитный абонемент на посещения после 16:00",          IsActive = true },
                    new MembershipType { TypeName = "Персональная тренировка",  VisitsCount = 1,    DurationDays = 1,   Price = 1500,  Description = "Индивидуальная тренировка с тренером",                    IsActive = true },
                    new MembershipType { TypeName = "Безлимитный на 3 месяца",  VisitsCount = null, DurationDays = 90,  Price = 10000, Description = "Неограниченное количество посещений в течение 3 месяцев", IsActive = true },
                    new MembershipType { TypeName = "Безлимитный на год",       VisitsCount = null, DurationDays = 365, Price = 35000, Description = "Неограниченное количество посещений в течение года",      IsActive = true }
                };
                foreach (var t in membershipTypes) context.MembershipTypes.Add(t);
                context.SaveChanges();
            }

            // 2. Клиенты — 30 человек
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { LastName="Иванов",     FirstName="Иван",      MiddleName="Иванович",    BirthDate=new DateTime(1990,1,1),   Gender="M", Phone="+7 (962) 100-01-01", Email="ivanov@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-180), Notes="Постоянный клиент" },
                    new Client { LastName="Петрова",    FirstName="Анна",      MiddleName="Сергеевна",   BirthDate=new DateTime(1995,5,15),  Gender="F", Phone="+7 (962) 100-02-02", Email="petrova@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-120), Notes="Ходит на йогу" },
                    new Client { LastName="Сидоров",    FirstName="Петр",      MiddleName="Алексеевич",  BirthDate=new DateTime(1988,10,20), Gender="M", Phone="+7 (962) 100-03-03", Email="sidorov@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-150), Notes="Тренажерный зал" },
                    new Client { LastName="Смирнова",   FirstName="Елена",     MiddleName="Дмитриевна",  BirthDate=new DateTime(1992,3,8),   Gender="F", Phone="+7 (962) 100-04-04", Email="smirnova@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-60),  Notes="" },
                    new Client { LastName="Козлов",     FirstName="Андрей",    MiddleName="Николаевич",  BirthDate=new DateTime(1985,7,12),  Gender="M", Phone="+7 (962) 100-05-05", Email="kozlov@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-200), Notes="Групповые занятия" },
                    new Client { LastName="Морозова",   FirstName="Ольга",     MiddleName="Павловна",    BirthDate=new DateTime(1993,11,3),  Gender="F", Phone="+7 (962) 100-06-06", Email="morozova@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-90),  Notes="Аэробика" },
                    new Client { LastName="Новиков",    FirstName="Сергей",    MiddleName="Аркадьевич",  BirthDate=new DateTime(1987,4,22),  Gender="M", Phone="+7 (962) 100-07-07", Email="novikov@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-75),  Notes="" },
                    new Client { LastName="Соколова",   FirstName="Мария",     MiddleName="Игоревна",    BirthDate=new DateTime(1999,8,18),  Gender="F", Phone="+7 (962) 100-08-08", Email="sokolova@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-45),  Notes="Студентка" },
                    new Client { LastName="Волков",     FirstName="Денис",     MiddleName="Юрьевич",     BirthDate=new DateTime(1991,2,14),  Gender="M", Phone="+7 (962) 100-09-09", Email="volkov@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-110), Notes="Силовые" },
                    new Client { LastName="Лебедева",   FirstName="Виктория",  MiddleName="Олеговна",    BirthDate=new DateTime(1996,6,25),  Gender="F", Phone="+7 (962) 100-10-10", Email="lebedeva@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-30),  Notes="" },
                    new Client { LastName="Зайцев",     FirstName="Алексей",   MiddleName="Вячеславович", BirthDate=new DateTime(1983,9,30), Gender="M", Phone="+7 (962) 100-11-11", Email="zaitsev@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-220), Notes="VIP-клиент" },
                    new Client { LastName="Попова",     FirstName="Татьяна",   MiddleName="Евгеньевна",  BirthDate=new DateTime(1994,12,5),  Gender="F", Phone="+7 (962) 100-12-12", Email="popova@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-55),  Notes="Пилатес" },
                    new Client { LastName="Семёнов",    FirstName="Михаил",    MiddleName="Романович",   BirthDate=new DateTime(1989,3,17),  Gender="M", Phone="+7 (962) 100-13-13", Email="semenov@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-95),  Notes="" },
                    new Client { LastName="Андреева",   FirstName="Наталья",   MiddleName="Борисовна",   BirthDate=new DateTime(1997,7,9),   Gender="F", Phone="+7 (962) 100-14-14", Email="andreeva@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-40),  Notes="Танцы" },
                    new Client { LastName="Кузнецов",   FirstName="Роман",     MiddleName="Дмитриевич",  BirthDate=new DateTime(1986,1,28),  Gender="M", Phone="+7 (962) 100-15-15", Email="kuznetsov@mail.ru",  RegistrationDate=DateTime.UtcNow.AddDays(-160), Notes="Кардио" },
                    new Client { LastName="Фёдорова",   FirstName="Светлана",  MiddleName="Анатольевна", BirthDate=new DateTime(1998,4,11),  Gender="F", Phone="+7 (962) 100-16-16", Email="fedorova@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-25),  Notes="Новый клиент" },
                    new Client { LastName="Егоров",     FirstName="Владимир",  MiddleName="Степанович",  BirthDate=new DateTime(1984,10,6),  Gender="M", Phone="+7 (962) 100-17-17", Email="egorov@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-130), Notes="" },
                    new Client { LastName="Никитина",   FirstName="Дарья",     MiddleName="Леонидовна",  BirthDate=new DateTime(2000,2,20),  Gender="F", Phone="+7 (962) 100-18-18", Email="nikitina@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-20),  Notes="Студентка" },
                    new Client { LastName="Орлов",      FirstName="Константин",MiddleName="Федорович",   BirthDate=new DateTime(1982,8,14),  Gender="M", Phone="+7 (962) 100-19-19", Email="orlov@mail.ru",      RegistrationDate=DateTime.UtcNow.AddDays(-250), Notes="Старожил клуба" },
                    new Client { LastName="Титова",     FirstName="Инна",      MiddleName="Максимовна",  BirthDate=new DateTime(1993,5,30),  Gender="F", Phone="+7 (962) 100-20-20", Email="titova@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-70),  Notes="Йога и пилатес" },
                    new Client { LastName="Павлов",     FirstName="Игорь",     MiddleName="Валерьевич",  BirthDate=new DateTime(1980,11,22), Gender="M", Phone="+7 (962) 100-21-21", Email="pavlov@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-300), Notes="Утренние тренировки" },
                    new Client { LastName="Захарова",   FirstName="Юлия",      MiddleName="Николаевна",  BirthDate=new DateTime(1996,3,8),   Gender="F", Phone="+7 (962) 100-22-22", Email="zakharova@mail.ru",  RegistrationDate=DateTime.UtcNow.AddDays(-50),  Notes="" },
                    new Client { LastName="Герасимов",  FirstName="Артём",     MiddleName="Олегович",    BirthDate=new DateTime(2001,6,15),  Gender="M", Phone="+7 (962) 100-23-23", Email="gerasimov@mail.ru",  RegistrationDate=DateTime.UtcNow.AddDays(-15),  Notes="Студент" },
                    new Client { LastName="Степанова",  FirstName="Кристина",  MiddleName="Сергеевна",   BirthDate=new DateTime(1994,9,1),   Gender="F", Phone="+7 (962) 100-24-24", Email="stepanova@mail.ru",  RegistrationDate=DateTime.UtcNow.AddDays(-85),  Notes="Аэробика" },
                    new Client { LastName="Романов",    FirstName="Николай",   MiddleName="Иванович",    BirthDate=new DateTime(1979,12,25), Gender="M", Phone="+7 (962) 100-25-25", Email="romanov@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-190), Notes="" },
                    new Client { LastName="Макарова",   FirstName="Полина",    MiddleName="Андреевна",   BirthDate=new DateTime(1997,4,4),   Gender="F", Phone="+7 (962) 100-26-26", Email="makarova@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-35),  Notes="Стретчинг" },
                    new Client { LastName="Тарасов",    FirstName="Евгений",   MiddleName="Петрович",    BirthDate=new DateTime(1991,7,19),  Gender="M", Phone="+7 (962) 100-27-27", Email="tarasov@mail.ru",    RegistrationDate=DateTime.UtcNow.AddDays(-105), Notes="Силовые" },
                    new Client { LastName="Белова",     FirstName="Анастасия", MiddleName="Викторовна",  BirthDate=new DateTime(1995,10,10), Gender="F", Phone="+7 (962) 100-28-28", Email="belova@mail.ru",     RegistrationDate=DateTime.UtcNow.AddDays(-65),  Notes="Танцы и аэробика" },
                    new Client { LastName="Гусев",      FirstName="Александр", MiddleName="Михайлович",  BirthDate=new DateTime(1988,1,31),  Gender="M", Phone="+7 (962) 100-29-29", Email="gusev@mail.ru",      RegistrationDate=DateTime.UtcNow.AddDays(-140), Notes="Кардио + силовые" },
                    new Client { LastName="Калинина",   FirstName="Вероника",  MiddleName="Руслановна",  BirthDate=new DateTime(2000,8,27),  Gender="F", Phone="+7 (962) 100-30-30", Email="kalinina@mail.ru",   RegistrationDate=DateTime.UtcNow.AddDays(-10),  Notes="Новый клиент" },
                };
                foreach (var c in clients) context.Clients.Add(c);
                context.SaveChanges();
            }

            // 3. Групповые занятия
            if (!context.GroupClasses.Any())
            {
                var groupClasses = new GroupClass[]
                {
                    new GroupClass { Name="Йога",      Instructor="Елена Смирнова",  DayOfWeek="Понедельник", StartTime=new TimeSpan(10,0,0),  EndTime=new TimeSpan(11,0,0),  MaxParticipants=15, Room="Зал йоги",    Description="Хатха-йога для начинающих.", Price=400, IsActive=true },
                    new GroupClass { Name="Пилатес",   Instructor="Анна Иванова",    DayOfWeek="Вторник",     StartTime=new TimeSpan(18,30,0), EndTime=new TimeSpan(19,30,0), MaxParticipants=12, Room="Малый зал",   Description="Укрепление мышц кора.",       Price=450, IsActive=true },
                    new GroupClass { Name="Аэробика",  Instructor="Мария Петрова",   DayOfWeek="Среда",       StartTime=new TimeSpan(19,0,0),  EndTime=new TimeSpan(20,0,0),  MaxParticipants=20, Room="Основной зал",Description="Кардио-тренировка под музыку.",Price=350, IsActive=true },
                    new GroupClass { Name="Танцы",     Instructor="Дмитрий Сидоров", DayOfWeek="Четверг",     StartTime=new TimeSpan(20,0,0),  EndTime=new TimeSpan(21,0,0),  MaxParticipants=16, Room="Основной зал",Description="Zumba и латина.",             Price=400, IsActive=true },
                    new GroupClass { Name="Йога",      Instructor="Елена Смирнова",  DayOfWeek="Суббота",     StartTime=new TimeSpan(11,0,0),  EndTime=new TimeSpan(12,0,0),  MaxParticipants=15, Room="Зал йоги",    Description="Йога выходного дня.",         Price=400, IsActive=true },
                    new GroupClass { Name="Стретчинг", Instructor="Анна Иванова",    DayOfWeek="Пятница",     StartTime=new TimeSpan(18,0,0),  EndTime=new TimeSpan(19,0,0),  MaxParticipants=15, Room="Малый зал",   Description="Растяжка и гибкость.",        Price=350, IsActive=true },
                };
                foreach (var g in groupClasses) context.GroupClasses.Add(g);
                context.SaveChanges();
            }

            // 4. Абонементы — 25 штук
            if (!context.Memberships.Any() && context.Clients.Any() && context.MembershipTypes.Any())
            {
                var cl  = context.Clients.ToList();
                var mts = context.MembershipTypes.ToList();
                MembershipType MT(string name) => mts.First(t => t.TypeName == name);

                var memberships = new Membership[]
                {
                    new Membership { ClientId=cl[0].Id,  TypeId=MT("Безлимитный на месяц").Id,      StartDate=DateTime.UtcNow.Date.AddDays(-15), EndDate=DateTime.UtcNow.Date.AddDays(15),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-15) },
                    new Membership { ClientId=cl[1].Id,  TypeId=MT("Утренний (до 16:00)").Id,        StartDate=DateTime.UtcNow.Date.AddDays(-20), EndDate=DateTime.UtcNow.Date.AddDays(10),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-20) },
                    new Membership { ClientId=cl[2].Id,  TypeId=MT("Вечерний (после 16:00)").Id,     StartDate=DateTime.UtcNow.Date.AddDays(-5),  EndDate=DateTime.UtcNow.Date.AddDays(25),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-5)  },
                    new Membership { ClientId=cl[3].Id,  TypeId=MT("Абонемент на 8 посещений").Id,   StartDate=DateTime.UtcNow.Date.AddDays(-10), EndDate=DateTime.UtcNow.Date.AddDays(20),  VisitsRemaining=5,  Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-10) },
                    new Membership { ClientId=cl[4].Id,  TypeId=MT("Безлимитный на 3 месяца").Id,    StartDate=DateTime.UtcNow.Date.AddDays(-30), EndDate=DateTime.UtcNow.Date.AddDays(60),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-30) },
                    new Membership { ClientId=cl[5].Id,  TypeId=MT("Утренний (до 16:00)").Id,        StartDate=DateTime.UtcNow.Date.AddDays(-8),  EndDate=DateTime.UtcNow.Date.AddDays(22),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-8)  },
                    new Membership { ClientId=cl[6].Id,  TypeId=MT("Безлимитный на месяц").Id,       StartDate=DateTime.UtcNow.Date.AddDays(-12), EndDate=DateTime.UtcNow.Date.AddDays(18),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-12) },
                    new Membership { ClientId=cl[7].Id,  TypeId=MT("Абонемент на 4 посещения").Id,   StartDate=DateTime.UtcNow.Date.AddDays(-3),  EndDate=DateTime.UtcNow.Date.AddDays(27),  VisitsRemaining=3,  Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-3)  },
                    new Membership { ClientId=cl[8].Id,  TypeId=MT("Вечерний (после 16:00)").Id,     StartDate=DateTime.UtcNow.Date.AddDays(-18), EndDate=DateTime.UtcNow.Date.AddDays(12),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-18) },
                    new Membership { ClientId=cl[9].Id,  TypeId=MT("Безлимитный на месяц").Id,       StartDate=DateTime.UtcNow.Date.AddDays(-2),  EndDate=DateTime.UtcNow.Date.AddDays(28),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-2)  },
                    new Membership { ClientId=cl[10].Id, TypeId=MT("Безлимитный на год").Id,          StartDate=DateTime.UtcNow.Date.AddDays(-90), EndDate=DateTime.UtcNow.Date.AddDays(275), VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-90) },
                    new Membership { ClientId=cl[11].Id, TypeId=MT("Абонемент на 8 посещений").Id,   StartDate=DateTime.UtcNow.Date.AddDays(-7),  EndDate=DateTime.UtcNow.Date.AddDays(23),  VisitsRemaining=6,  Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-7)  },
                    new Membership { ClientId=cl[12].Id, TypeId=MT("Вечерний (после 16:00)").Id,     StartDate=DateTime.UtcNow.Date.AddDays(-22), EndDate=DateTime.UtcNow.Date.AddDays(8),   VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-22) },
                    new Membership { ClientId=cl[13].Id, TypeId=MT("Утренний (до 16:00)").Id,        StartDate=DateTime.UtcNow.Date.AddDays(-4),  EndDate=DateTime.UtcNow.Date.AddDays(26),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-4)  },
                    new Membership { ClientId=cl[14].Id, TypeId=MT("Безлимитный на 3 месяца").Id,    StartDate=DateTime.UtcNow.Date.AddDays(-45), EndDate=DateTime.UtcNow.Date.AddDays(45),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-45) },
                    new Membership { ClientId=cl[15].Id, TypeId=MT("Абонемент на 4 посещения").Id,   StartDate=DateTime.UtcNow.Date.AddDays(-1),  EndDate=DateTime.UtcNow.Date.AddDays(29),  VisitsRemaining=4,  Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-1)  },
                    new Membership { ClientId=cl[16].Id, TypeId=MT("Безлимитный на месяц").Id,       StartDate=DateTime.UtcNow.Date.AddDays(-25), EndDate=DateTime.UtcNow.Date.AddDays(5),   VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-25) },
                    new Membership { ClientId=cl[17].Id, TypeId=MT("Персональная тренировка").Id,    StartDate=DateTime.UtcNow.Date,              EndDate=DateTime.UtcNow.Date.AddDays(1),   VisitsRemaining=1,  Status="active",  PurchaseDate=DateTime.UtcNow              },
                    new Membership { ClientId=cl[18].Id, TypeId=MT("Безлимитный на год").Id,          StartDate=DateTime.UtcNow.Date.AddDays(-60), EndDate=DateTime.UtcNow.Date.AddDays(305), VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-60) },
                    new Membership { ClientId=cl[19].Id, TypeId=MT("Утренний (до 16:00)").Id,        StartDate=DateTime.UtcNow.Date.AddDays(-14), EndDate=DateTime.UtcNow.Date.AddDays(16),  VisitsRemaining=-1, Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-14) },
                    new Membership { ClientId=cl[20].Id, TypeId=MT("Разовое посещение").Id,           StartDate=DateTime.UtcNow.Date.AddDays(-5),  EndDate=DateTime.UtcNow.Date.AddDays(-4),  VisitsRemaining=0,  Status="expired", PurchaseDate=DateTime.UtcNow.AddDays(-5)  },
                    new Membership { ClientId=cl[21].Id, TypeId=MT("Абонемент на 4 посещения").Id,   StartDate=DateTime.UtcNow.Date.AddDays(-35), EndDate=DateTime.UtcNow.Date.AddDays(-5),  VisitsRemaining=0,  Status="expired", PurchaseDate=DateTime.UtcNow.AddDays(-35) },
                    new Membership { ClientId=cl[22].Id, TypeId=MT("Безлимитный на месяц").Id,       StartDate=DateTime.UtcNow.Date.AddDays(-50), EndDate=DateTime.UtcNow.Date.AddDays(-20), VisitsRemaining=-1, Status="expired", PurchaseDate=DateTime.UtcNow.AddDays(-50) },
                    new Membership { ClientId=cl[23].Id, TypeId=MT("Вечерний (после 16:00)").Id,     StartDate=DateTime.UtcNow.Date.AddDays(-10), EndDate=DateTime.UtcNow.Date.AddDays(30),  VisitsRemaining=-1, Status="frozen",  PurchaseDate=DateTime.UtcNow.AddDays(-10), FreezeDays=10 },
                    new Membership { ClientId=cl[24].Id, TypeId=MT("Абонемент на 8 посещений").Id,   StartDate=DateTime.UtcNow.Date.AddDays(-6),  EndDate=DateTime.UtcNow.Date.AddDays(24),  VisitsRemaining=7,  Status="active",  PurchaseDate=DateTime.UtcNow.AddDays(-6)  },
                };
                foreach (var m in memberships) context.Memberships.Add(m);
                context.SaveChanges();
            }

            // 5. Посещения — 50+ записей
            if (!context.Visits.Any() && context.Clients.Any() && context.Memberships.Any())
            {
                var cl   = context.Clients.ToList();
                var memb = context.Memberships.ToList();
                var visits = new List<Visit>();

                Membership? GetMem(int idx) =>
                    memb.FirstOrDefault(m => m.ClientId == cl[idx].Id &&
                                             (m.Status == "active" || m.Status == "expired"));

                void V(int cIdx, DateTime ci, DateTime? co)
                {
                    var mem = GetMem(cIdx);
                    if (mem == null) return;
                    visits.Add(new Visit { ClientId=cl[cIdx].Id, MembershipId=mem.Id, CheckInTime=ci, CheckOutTime=co });
                }

                // Сегодня — 10 посещений
                V(0,  DateTime.UtcNow.Date.AddHours(8).AddMinutes(10),  DateTime.UtcNow.Date.AddHours(9).AddMinutes(40));
                V(1,  DateTime.UtcNow.Date.AddHours(9),                  DateTime.UtcNow.Date.AddHours(10).AddMinutes(15));
                V(2,  DateTime.UtcNow.Date.AddHours(17).AddMinutes(30),  null);
                V(3,  DateTime.UtcNow.Date.AddHours(10).AddMinutes(45), DateTime.UtcNow.Date.AddHours(12));
                V(4,  DateTime.UtcNow.Date.AddHours(11),                 DateTime.UtcNow.Date.AddHours(12).AddMinutes(30));
                V(5,  DateTime.UtcNow.Date.AddHours(8).AddMinutes(30),  DateTime.UtcNow.Date.AddHours(10));
                V(6,  DateTime.UtcNow.Date.AddHours(19),                 null);
                V(7,  DateTime.UtcNow.Date.AddHours(15).AddMinutes(15), DateTime.UtcNow.Date.AddHours(16).AddMinutes(45));
                V(8,  DateTime.UtcNow.Date.AddHours(18),                 null);
                V(9,  DateTime.UtcNow.Date.AddHours(12).AddMinutes(30), DateTime.UtcNow.Date.AddHours(14));

                // Вчера — 8
                V(10, DateTime.UtcNow.Date.AddDays(-1).AddHours(8),    DateTime.UtcNow.Date.AddDays(-1).AddHours(9).AddMinutes(30));
                V(11, DateTime.UtcNow.Date.AddDays(-1).AddHours(10),   DateTime.UtcNow.Date.AddDays(-1).AddHours(11).AddMinutes(20));
                V(0,  DateTime.UtcNow.Date.AddDays(-1).AddHours(19),   DateTime.UtcNow.Date.AddDays(-1).AddHours(20).AddMinutes(30));
                V(12, DateTime.UtcNow.Date.AddDays(-1).AddHours(18),   DateTime.UtcNow.Date.AddDays(-1).AddHours(19).AddMinutes(45));
                V(13, DateTime.UtcNow.Date.AddDays(-1).AddHours(9),    DateTime.UtcNow.Date.AddDays(-1).AddHours(10).AddMinutes(30));
                V(14, DateTime.UtcNow.Date.AddDays(-1).AddHours(20),   DateTime.UtcNow.Date.AddDays(-1).AddHours(21).AddMinutes(15));
                V(4,  DateTime.UtcNow.Date.AddDays(-1).AddHours(11),   DateTime.UtcNow.Date.AddDays(-1).AddHours(12).AddMinutes(30));
                V(16, DateTime.UtcNow.Date.AddDays(-1).AddHours(8).AddMinutes(45), DateTime.UtcNow.Date.AddDays(-1).AddHours(10));

                // 2 дня назад
                V(1,  DateTime.UtcNow.Date.AddDays(-2).AddHours(10),   DateTime.UtcNow.Date.AddDays(-2).AddHours(11));
                V(5,  DateTime.UtcNow.Date.AddDays(-2).AddHours(8).AddMinutes(30), DateTime.UtcNow.Date.AddDays(-2).AddHours(10));
                V(3,  DateTime.UtcNow.Date.AddDays(-2).AddHours(14),   DateTime.UtcNow.Date.AddDays(-2).AddHours(15).AddMinutes(30));
                V(18, DateTime.UtcNow.Date.AddDays(-2).AddHours(7).AddMinutes(30), DateTime.UtcNow.Date.AddDays(-2).AddHours(9));
                V(2,  DateTime.UtcNow.Date.AddDays(-2).AddHours(20),   DateTime.UtcNow.Date.AddDays(-2).AddHours(21).AddMinutes(30));
                V(19, DateTime.UtcNow.Date.AddDays(-2).AddHours(9),    DateTime.UtcNow.Date.AddDays(-2).AddHours(10).AddMinutes(15));

                // 3-7 дней назад
                for (int d = 3; d <= 7; d++)
                {
                    V(0,       DateTime.UtcNow.Date.AddDays(-d).AddHours(8).AddMinutes(15),  DateTime.UtcNow.Date.AddDays(-d).AddHours(9).AddMinutes(45));
                    V(10,      DateTime.UtcNow.Date.AddDays(-d).AddHours(8),                  DateTime.UtcNow.Date.AddDays(-d).AddHours(9).AddMinutes(30));
                    V(14,      DateTime.UtcNow.Date.AddDays(-d).AddHours(18),                 DateTime.UtcNow.Date.AddDays(-d).AddHours(19).AddMinutes(30));
                    V(8,       DateTime.UtcNow.Date.AddDays(-d).AddHours(20),                 DateTime.UtcNow.Date.AddDays(-d).AddHours(21));
                    V(6,       DateTime.UtcNow.Date.AddDays(-d).AddHours(19),                 DateTime.UtcNow.Date.AddDays(-d).AddHours(20).AddMinutes(30));
                    V(d % 15,  DateTime.UtcNow.Date.AddDays(-d).AddHours(12),                 DateTime.UtcNow.Date.AddDays(-d).AddHours(13).AddMinutes(30));
                }

                // 8-20 дней назад
                for (int d = 8; d <= 20; d++)
                {
                    V(d % 20,       DateTime.UtcNow.Date.AddDays(-d).AddHours(9 + d % 4), DateTime.UtcNow.Date.AddDays(-d).AddHours(10 + d % 4).AddMinutes(30));
                    V((d+5) % 20,   DateTime.UtcNow.Date.AddDays(-d).AddHours(18),        DateTime.UtcNow.Date.AddDays(-d).AddHours(19).AddMinutes(45));
                }

                foreach (var v in visits) context.Visits.Add(v);
                context.SaveChanges();
            }

            // 6. Бронирования
            if (!context.GroupClassBookings.Any() && context.Clients.Any() && context.GroupClasses.Any())
            {
                var cl = context.Clients.ToList();
                var gc = context.GroupClasses.ToList();
                GroupClass GC(string name, string day) => gc.First(g => g.Name == name && g.DayOfWeek == day);

                var bookings = new GroupClassBooking[]
                {
                    new GroupClassBooking { GroupClassId=GC("Йога","Понедельник").Id,  ClientId=cl[1].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(1), BookingDate=DateTime.UtcNow.AddDays(-2), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Йога","Понедельник").Id,  ClientId=cl[3].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(1), BookingDate=DateTime.UtcNow.AddDays(-1), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Йога","Понедельник").Id,  ClientId=cl[5].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(1), BookingDate=DateTime.UtcNow.AddDays(-1), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Пилатес","Вторник").Id,   ClientId=cl[1].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(2), BookingDate=DateTime.UtcNow.AddDays(-1), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Пилатес","Вторник").Id,   ClientId=cl[11].Id, ClassDate=DateTime.UtcNow.Date.AddDays(2), BookingDate=DateTime.UtcNow.AddDays(-3), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Аэробика","Среда").Id,    ClientId=cl[4].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(3), BookingDate=DateTime.UtcNow.AddDays(-3), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Аэробика","Среда").Id,    ClientId=cl[5].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(3), BookingDate=DateTime.UtcNow.AddDays(-2), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Аэробика","Среда").Id,    ClientId=cl[13].Id, ClassDate=DateTime.UtcNow.Date.AddDays(3), BookingDate=DateTime.UtcNow.AddDays(-1), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Танцы","Четверг").Id,     ClientId=cl[13].Id, ClassDate=DateTime.UtcNow.Date.AddDays(4), BookingDate=DateTime.UtcNow.AddDays(-2), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Танцы","Четверг").Id,     ClientId=cl[27].Id, ClassDate=DateTime.UtcNow.Date.AddDays(4), BookingDate=DateTime.UtcNow.AddDays(-1), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Стретчинг","Пятница").Id, ClientId=cl[25].Id, ClassDate=DateTime.UtcNow.Date.AddDays(5), BookingDate=DateTime.UtcNow.AddDays(-4), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Стретчинг","Пятница").Id, ClientId=cl[1].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(5), BookingDate=DateTime.UtcNow.AddDays(-1), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Йога","Суббота").Id,      ClientId=cl[19].Id, ClassDate=DateTime.UtcNow.Date.AddDays(6), BookingDate=DateTime.UtcNow.AddDays(-5), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Йога","Суббота").Id,      ClientId=cl[3].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(6), BookingDate=DateTime.UtcNow.AddDays(-2), IsAttended=false },
                    new GroupClassBooking { GroupClassId=GC("Йога","Понедельник").Id,  ClientId=cl[1].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(-6), BookingDate=DateTime.UtcNow.AddDays(-8), IsAttended=true },
                    new GroupClassBooking { GroupClassId=GC("Аэробика","Среда").Id,    ClientId=cl[4].Id,  ClassDate=DateTime.UtcNow.Date.AddDays(-4), BookingDate=DateTime.UtcNow.AddDays(-6), IsAttended=true },
                    new GroupClassBooking { GroupClassId=GC("Пилатес","Вторник").Id,   ClientId=cl[11].Id, ClassDate=DateTime.UtcNow.Date.AddDays(-5), BookingDate=DateTime.UtcNow.AddDays(-7), IsAttended=true },
                };
                foreach (var b in bookings) context.GroupClassBookings.Add(b);
                context.SaveChanges();
            }
        }
    }
}

Web-приложение по учёту и мониторингу выполнения заказов фирмы по производству мебели поможет автоматизировать работу компании/компаний, занимающихся производством и продажей мебели. Данное приложение позволит усовершенствовать процесс обслуживания клиентов благодаря ведению всестороннего
учёта.

Основные функции web-приложения:
- авторизация и регистрация пользователей;
- учет и регистрация заказов мебели;
- поиск товаров;
- фильтрация товаров;
- отслеживание заказов;
- возможность оставить отзыв; 
- уведомления для клиентов;
- получение статистикиза период времени;
- управление запасами склада.

Роли web-приложения: гость, клиент, специалист по приёму заказов, менеджер по управлению запасами склада, администратор.

В процессе разработки была использована база данных MS SQL для хранения информации о мебели и остальных сущностях web-приложения, платформа ASP.NET Core для разработки web-приложения с использованием объектно-ориентированного языка программирования C# в среде разработки Visual Studio. Для доступа данных была использована технология Entity Framework Core, которая позволяет создавать запросы к базе данных на языке LINQ, фреймворк Bootstrap для создания приятного пользовательского интерфейса. Для визуализации 3D-моделей используется библиотека JavaScript Three.js, содержащая набор готовых классов для создания и отображения интерактивной 3D-графики. Использовался паттерн проектирования MVC для разделения логики приложения на отдельные компоненты, фреймворк ASP.NET Core Identity для организации аутентификации и авторизации пользователей, обеспечивающий безопасность доступа к функциональности приложения.

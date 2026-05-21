# База данных отелей – клиент-серверное приложение

Курсовая работа, 2-й курс, программная инженерия.

![Скриншот](https://github.com/user-attachments/assets/bdae45a1-5faf-48f2-9a9a-5a2aa6735789)

## Пояснительная записка

[Скачать PDF](https://github.com/user-attachments/files/28122495/_._.401_2.pdf)

## Запуск
1. Создайте базу данных из скрипта:
```sql
CREATE TABLE HotelNames (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Cities (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Hotels (
    Id INT PRIMARY KEY IDENTITY,
    HotelNameId INT FOREIGN KEY REFERENCES HotelNames(Id),
    CityId INT FOREIGN KEY REFERENCES Cities(Id),
    Capacity INT NOT NULL,
    Price DECIMAL(10,2) NOT NULL
);
```
2. Укажите строку подключения в appsettings.json.  
3.Выполните Update-Database.  
4.Запустите проект (F5).  


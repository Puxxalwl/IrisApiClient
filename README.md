# IrisApiClient — асинхронный клиент для работы с API Iris | Чат-менеджер'а в Telegram
### (Документация и библиотека не полностью готовы)

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![.NET 8](https://img.shields.io/badge/.NET-8-512BD4.svg?logo=dotnet&logoColor=white)

---

## Оглавление
1. [Установка](#установка)  
2. [Конфигурация](#конфигурация)  
3. [Быстрый старт](#быстрый-старт)  
4. [Методы клиента](#методы-клиента)  
   - [GetBalance](#getbalance)  
   - [GiveSweets](#givesweets)  
   - [OpenBag](#openbag)  
   - [AllowAll](#allowall)  
   - [AllowUser](#allowuser)  
5. [Обработка ошибок](#обработка-ошибок)  
6. [Контакты](#контакты)  

---

## Установка

Установка пакета через NuGet:

```bash
dotnet add package IrisApi
```

---

## Конфигурация

Создайте файл **IrisSettings.json** в корне проекта:

```JSON
{
  "IrisApi": {
    "IrisUrl": "https://iris-tg.ru/api",
    "botId": "ID_бота",
    "IrisToken": "Iris-Token (получается у @irisism)"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```


---

### Быстрый старт

```Csharp
using IrisClient.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var client = IrisApiClient.Create();

        var balance = await client.GetBalance();
        Console.WriteLine($"Голд: {balance.gold}, Ириски: {balance.sweets}, Донат: {balance.donate_score}");
    }
}
```

---

## Методы клиента

### GetBalance

Получение текущего баланса.

```Csharp
Balance balance = await client.GetBalance();
Console.WriteLine($"Голд: {balance.gold}, Ириски: {balance.sweets}");
```

---

### GiveSweets

Передача ирисок другому пользователю.

```csharp
var result = await client.GiveSweets(
    userId: 6984952764,
    sweets: 10,
    withoutDonateScore: true,
    Comment: "За такую прекрасную либу"
);

if (result != null)
    Console.WriteLine("Ириски отправлены");
```

---

### OpenBag

Открытие или закрытие мешка для переводов.

```csharp
await client.OpenBag(true);  // открыть
await client.OpenBag(false); // закрыть
```


---

### AllowAll

Разрешить или запретить переводы для всех.

```csharp
await client.AllowAll(true);  // разрешить
await client.AllowAll(false); // запретить
```


---

### AllowUser

Разрешить или запретить переводы для конкретного пользователя.
```csharp
await client.AllowUser(true, 123456789);  // разрешить
await client.AllowUser(false, 123456789); // запретить
```

---

### Обработка ошибок

Все запросы используют функцию `getWithRetryAsync`, которое сразу выдает ошибки

---

### Контакты

Я есть лишь в **Telegram** и только с username: @puxalwl (ID: 6984952764)

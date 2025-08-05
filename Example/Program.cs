using System;
using System.Threading.Tasks;
using IrisClient.Config;
using IrisClient.Services;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var client = IrisApiClient.Create();

            var balance = await client.GetBalance();

            Console.WriteLine($"Ирис-голд: {balance.gold}, Ириски: {balance.sweets}, Очки-Доната: {balance.donate_score}");


            var giveSweets = await client.GiveSweets(userId: 123456789, sweets: 10.1, withoutDonateScore: true, Comment: "Подарочек"); // Перевести ириски
            if (giveSweets.result == "ok")
            {
                Console.WriteLine($"Перевод ирисок инфо:\nКол-во: {giveSweets.amount}, кому: {giveSweets}");
            }
            else
            {
                Console.WriteLine($"Error {giveSweets.error_code}: {giveSweets.description}");
            }

            var AllowAll = await client.AllAllow(Status: true); // Указывать статус нужно чтобы разрешить/запретить всём переводы вам
            Console.WriteLine($"Статус: {AllowAll.response}");


            var AllowUser = await client.AllowUser(Status: true, userId: 123456789); // Аналогичго AllowAll только для определенного пользователя
            Console.WriteLine($"Статус: {AllowUser.response}");
            

            var OpenBag = await client.OpenBag(Status: true); // Открыть мешок
            Console.WriteLine($"Статус: {OpenBag.result}");
        }
        catch (Exception err)
        {
            Console.WriteLine($"Ошибка: {err.Message}");
        }
    }
}
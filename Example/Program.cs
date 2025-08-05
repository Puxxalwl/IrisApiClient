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
        }
        catch (Exception err)
        {
            Console.WriteLine($"Ошибка: {err.Message}");
        }
    }
}
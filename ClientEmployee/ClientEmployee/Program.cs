
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientEmployee
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const string PREFIX_API = "https://localhost:5001/api/";

            var service = new EmployeeService(PREFIX_API);

            await service.GetAllAsync();
            await service.GetOneAsync(2);
            await service.GetEmailAsync(3);

            //await service.CreateAsync("Pau", "Cuminal", "pcuminal@institutmontilivi.cat", 4000);
            await service.GetAllAsync();

            Console.Write("QUIN VOLS ELIMINAR? ");
            int codiToDelete = Convert.ToInt32(Console.ReadLine());
            await service.DeleteAsync(codiToDelete);

            Console.WriteLine("ANEM A MODIFICAR EL SOU ALEATORIAMENT DEL EMPLEAT QUE VULGUIS");
            await service.GetAllAsync();

            Console.Write("QUIN SOU VOLS MODIFICAR? ");
            int codiToUpdate = Convert.ToInt32(Console.ReadLine());
            var e = await service.GetOneAsync(codiToUpdate);

            if (e != null)
            {
                var r = new Random();
                e.Salary = r.Next(5000, 60000); // sou aleatori
                await service.UpdateAsync(e);
                Console.WriteLine("ACTUALITZAT");
            }
            else
            {
                Console.WriteLine($"CODI {codiToUpdate} INEXISTENT");
            }
        }
    }
}


using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json;
using System.Threading.Tasks;
using ClientEmployee.Model;

namespace ClientEmployee
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

			// Si voleu per fer proves podeu fer-vos un menu amb un switch i un bucle do-while, 

			// Però al entregar la pràctica volem un client d'aquest estil que mostre totes les funcionalitats de les preguntes realitzades a la pràctica 
			// per poder corregir millor els resultats esperats.
            // Poseu un Console.WriteLine abans de cada funcionalitat per identificar-la millor.

			const string PREFIX_API = "https://localhost:5001/api/";

            var service = new EmployeeService(PREFIX_API);

			Console.WriteLine("==== INICI ====");
			Console.WriteLine("···· Mostrem tots els empleats ····");
            List<Employee> employees = await service.GetAllAsync();
            foreach (Employee emp in employees)
                Console.WriteLine(emp);

			Console.WriteLine("\n\n···· Mostrem un empleat concret ····");
			Employee empAux = await service.GetOneAsync(2);
            Console.WriteLine(empAux);

            Console.WriteLine("\n\n···· Mostrem el email d'un empleat concret ····");
            string email = await service.GetEmailAsync(3);
            Console.WriteLine(email);

			//await service.CreateAsync("Pau", "Cuminal", "pcuminal@institutmontilivi.cat", 4000);
			Console.WriteLine("\n\n···· Creem un nou empleat ····");
			Console.Write("Nom: "); empAux.FirstName = Console.ReadLine();
			Console.Write("Cognom: "); empAux.LastName = Console.ReadLine();
			Console.Write("Email: "); empAux.Email = Console.ReadLine();
			Console.Write("Sou: "); empAux.Salary = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine(await service.CreateAsync(empAux.FirstName, empAux.LastName, empAux.Email, empAux.Salary)?"Empleat CREAT Correctament":"Error, Empleat NO CREAT");

			Console.WriteLine("\n\n···· Mostrem tots els empleats (novament) ····");
			List<Employee> employeesAfterCreate = await service.GetAllAsync();
            foreach (Employee emp in employeesAfterCreate)
                Console.WriteLine(emp);

			Console.WriteLine("\n\n···· Quin Empleat vols eliminar? ····");
			int codiToDelete = Convert.ToInt32(Console.ReadLine());
            await service.DeleteAsync(codiToDelete);

            Console.WriteLine("\n\n···· Mostrem tots els empleats (novament) ····");
			foreach (Employee emp in employeesAfterCreate)
				Console.WriteLine(emp);

			Console.WriteLine("\n\n···· Quin Empleat vols modificar? ····");
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

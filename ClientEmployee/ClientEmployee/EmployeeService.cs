using ClientEmployee.Model;
using System.Text;
using System.Text.Json;

namespace ClientEmployee
{
	public class EmployeeService
	{
		private readonly string prefixAPI;
		private readonly HttpClient client;

		public EmployeeService(string prefixApi)
		{
			prefixAPI = prefixApi;
			client = new HttpClient();
		}

		public async Task<List<Employee>> GetAllAsync()
		{
			// Cridem a l'endpoint GET api/employees que ens retorna la llista de tots els empleats
			var response = await client.GetAsync($"{prefixAPI}employees");
			if (response.IsSuccessStatusCode)
			{
				// Aquest json és una llista d'empleats, per això el deserialitzem a List<Employee>
				var json = await response.Content.ReadAsStringAsync();
				List<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(json);

				//foreach (var e in employees)
				//    Console.WriteLine(e);
				return employees;
			}
			return null;
		}

		public async Task<Employee> GetOneAsync(int id)
		{
			// Cridem a l'endpoint GET api/employees/{id} que ens retorna les dades de l'empleat amb id = {id}
			// retorna un objecte Employee en format JSON
			var response = await client.GetAsync($"{prefixAPI}employees/{id}");
			if (!response.IsSuccessStatusCode) return null;
			// Aquest json és un objecte Employee, per això el deserialitzem a Employee
			var json = await response.Content.ReadAsStringAsync();
			Employee e = JsonSerializer.Deserialize<Employee>(json);
			//Console.WriteLine(e);
			return e;
		}

		public async Task<string> GetEmailAsync(int id)
		{
			// Cridem a l'endpoint GET api/employees/{id}/email que ens retorna l'email de l'empleat amb id = {id}
			var response = await client.GetAsync($"{prefixAPI}employees/{id}/email");
			if (!response.IsSuccessStatusCode) return null;
			// auqest ja no es un objecte JSON, sinó un string, per això no el deserialitzem, sinó que el llegim directament com a string
			string email = await response.Content.ReadAsStringAsync();
			//Console.WriteLine(json);
			return email;
		}



		public async Task<bool> CreateAsync(string firstName, string lastName, string email, decimal salary)
		{
			bool inserit = false;
			try
			{

				Employee employee = new Employee
				{
					FirstName = firstName,
					LastName = lastName,
					Email = email,
					Salary = salary
				};

				// Per enviar les dades al servidor, primer hem de convertir l'objecte Employee a format JSON, per això el serialitzem
				string json = JsonSerializer.Serialize(employee);
				// per enviar el JSON al servidor, el convertim a un StringContent, que és el format que espera el mètode PostAsync
				var content = new StringContent(json, Encoding.UTF8, "application/json");

				await client.PostAsync($"{prefixAPI}employees", content);

				Console.WriteLine($"Empleat {employee.FirstName} {employee.LastName} inserit correctament.");
				inserit = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Han hagut problemes al inserir l'empleat: " + ex.Message);
			}
			return inserit;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			bool eliminat = false;
			// si no volem fer un try catch podem comprovar l'estat de la resposta del servidor, i si no és success, mostrar un missatge d'error

			// Comprovem que l'empleat existeix abans de intentar eliminar-lo
			var responseExist = await client.GetAsync($"{prefixAPI}employees/{id}");
			if (responseExist.IsSuccessStatusCode)
			{
				var responseDelete = await client.DeleteAsync($"{prefixAPI}employees/{id}");
				if (responseDelete.IsSuccessStatusCode)
				{
					Console.WriteLine($"Empleat amb id {id} eliminat correctament.");
					eliminat = true;
				}
				else
					Console.WriteLine($"No s'ha pogut eliminar l'empleat amb id {id}. Status code: {responseDelete.StatusCode}");
			}
			else
				Console.WriteLine($"L'empleat amb id {id} no existeix.");


			return eliminat;
		}

		public async Task<bool> UpdateAsync(Employee e)
		{
			// Podem fer una forma simple de saber si ha modificat l'empleat.
			
			var json = JsonSerializer.Serialize(e);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await client.PutAsync($"{prefixAPI}employees/{e.EmployeeId}", content);

			return response.IsSuccessStatusCode;
		}
	}
}

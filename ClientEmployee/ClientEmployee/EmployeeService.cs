using System.Text;
using System.Text.Json;
using ClientEmployee.Model;

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

        public async Task GetAllAsync()
        {
            var response = await client.GetAsync($"{prefixAPI}employees");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var employees = JsonSerializer.Deserialize<List<Employee>>(json);

                foreach (var e in employees)
                    Console.WriteLine(e);
            }
        }

        public async Task<Employee> GetOneAsync(int id)
        {
            var response = await client.GetAsync($"{prefixAPI}employees/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            var e = JsonSerializer.Deserialize<Employee>(json);
            Console.WriteLine(e);
            return e;
        }

        public async Task GetEmailAsync(int id)
        {
            var response = await client.GetAsync($"{prefixAPI}employees/{id}/email");
            if (!response.IsSuccessStatusCode) return;

            var json = await response.Content.ReadAsStringAsync();
            Console.WriteLine(json);
        }

        public async Task CreateAsync(string firstName, string lastName, string email, decimal salary)
        {
            var employee = new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Salary = salary
            };

            var json = JsonSerializer.Serialize(employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync($"{prefixAPI}employees", content);
        }

        public async Task DeleteAsync(int id)
        {
            var response = await client.DeleteAsync($"{prefixAPI}employees/{id}");
            Console.WriteLine(response.StatusCode);
        }

        public async Task UpdateAsync(Employee e)
        {
            var json = JsonSerializer.Serialize(e);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PutAsync($"{prefixAPI}employees/{e.EmployeeId}", content);
        }
    }
}

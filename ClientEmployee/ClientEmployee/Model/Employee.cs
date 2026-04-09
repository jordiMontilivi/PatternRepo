using System.Text.Json.Serialization;

namespace ClientEmployee.Model
{
    public class Employee
    {
        [JsonPropertyName("employeeId")]
        public int EmployeeId { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("salary")]
        public decimal Salary { get; set; }

        public override string ToString()
        {
            return $"{EmployeeId} - {FirstName} - {Email}";
        }
    }
}

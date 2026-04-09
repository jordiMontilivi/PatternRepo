using EmployeeAPI.Model;
using Microsoft.AspNetCore.Connections;
using MySql.Data.MySqlClient;
using EmployeeAPI.Interfaces;

namespace EmployeeAPI.Data
{
	public class EmployeeRepository: IEmployeeRepository
	{
		private readonly DBConnectionFactory connectionFactory;

		public EmployeeRepository(DBConnectionFactory connectionFactory)
		{
			this.connectionFactory = connectionFactory;
		}

		public List<Employee> GetAll()
		{
			var list = new List<Employee>();

			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var cmd = new MySqlCommand("SELECT * FROM Employees", conn);
			using var reader = cmd.ExecuteReader();

			while (reader.Read())
			{
				list.Add(new Employee
				{
					EmployeeId = reader.GetInt32("EmployeeId"),
					FirstName = reader.GetString("FirstName"),
					LastName = reader.GetString("LastName"),
					Email = reader.GetString("Email"),
					Salary = reader.GetDecimal("Salary")
				});
			}

			return list;
		}

		public Employee GetById(int id)
		{
			Employee e = null;
			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var cmd = new MySqlCommand("SELECT * FROM Employees WHERE EmployeeId=@id", conn);
			cmd.Parameters.AddWithValue("@id", id);

			using var reader = cmd.ExecuteReader();

			if (reader.Read())
			{
				e = new Employee
				{
					EmployeeId = reader.GetInt32("EmployeeId"),
					FirstName = reader.GetString("FirstName"),
					LastName = reader.GetString("LastName"),
					Email = reader.GetString("Email"),
					Salary = reader.GetDecimal("Salary")
				};
			}

			return e;
		}

		public void Add(Employee e)
		{
			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var cmd = new MySqlCommand(
				@"INSERT INTO Employees (FirstName,LastName,Email,Salary)
                  VALUES (@fn,@ln,@em,@sal)", conn);

			cmd.Parameters.AddWithValue("@fn", e.FirstName);
			cmd.Parameters.AddWithValue("@ln", e.LastName);
			cmd.Parameters.AddWithValue("@em", e.Email);
			cmd.Parameters.AddWithValue("@sal", e.Salary);

			cmd.ExecuteNonQuery();
		}

		public void Update(Employee e)
		{
			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var cmd = new MySqlCommand(
				@"UPDATE Employees SET FirstName=@fn,LastName=@ln,Email=@em,Salary=@sal
                  WHERE EmployeeId=@id", conn);

			cmd.Parameters.AddWithValue("@id", e.EmployeeId);
			cmd.Parameters.AddWithValue("@fn", e.FirstName);
			cmd.Parameters.AddWithValue("@ln", e.LastName);
			cmd.Parameters.AddWithValue("@em", e.Email);
			cmd.Parameters.AddWithValue("@sal", e.Salary);

			cmd.ExecuteNonQuery();
		}

		public void Delete(int id)
		{
			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var cmd = new MySqlCommand("DELETE FROM Employees WHERE EmployeeId=@id", conn);
			cmd.Parameters.AddWithValue("@id", id);

			cmd.ExecuteNonQuery();
		}

		//public void DeleteByName(string name)
		//{
		//	using var conn = connectionFactory.CreateConnection();
		//	conn.Open();

		//	var cmd = new MySqlCommand("DELETE FROM Employees WHERE EmployeeId=@name", conn);
		//	cmd.Parameters.AddWithValue("@name", name);

		//	cmd.ExecuteNonQuery();
		//}

		public string GetEmail(int id)
		{
			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var query = "SELECT Email FROM Employees WHERE EmployeeId = @id";

			using var cmd = new MySqlCommand(query, conn);
			cmd.Parameters.AddWithValue("@id", id);

			var result = cmd.ExecuteScalar();

			return result?.ToString();
		}

		public void UpdateEmail(int id, string Email)
		{
			using var conn = connectionFactory.CreateConnection();
			conn.Open();

			var query = "UPDATE Employees SET Email = @Email WHERE EmployeeId = @id";

			using var cmd = new MySqlCommand(query, conn);
			cmd.Parameters.AddWithValue("@id", id);
			cmd.Parameters.AddWithValue("@Email", Email);

			cmd.ExecuteNonQuery();
		}
	}
}

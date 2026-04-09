using EmployeeAPI.Data;
using EmployeeAPI.Interfaces;

namespace EmployeeAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Associcar la cadena de conexió que tenim al appsettings.json al repositori 
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			// DI, dependencies injection, inyección de dependencias
			// Add services to the container.
			// Registrar el DBConnectionFactory com un servei singleton (únic)
			builder.Services.AddSingleton(new DBConnectionFactory(connectionString));
			builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();


			app.UseSwagger();
			app.UseSwaggerUI();
			app.MapControllers();

			app.Run();
		}
	}
}

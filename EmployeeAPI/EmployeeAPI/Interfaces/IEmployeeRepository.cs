using EmployeeAPI.Model;

namespace EmployeeAPI.Interfaces
{
	public interface IEmployeeRepository
	{
		List<Employee> GetAll();
		Employee GetById(int id);
		void Add(Employee employee);
		void Update(Employee employee);
		void Delete(int id);
		//void DeleteByName(string name);
		public string GetEmail(int id);
		public void UpdateEmail(int id, string email);
	}
}

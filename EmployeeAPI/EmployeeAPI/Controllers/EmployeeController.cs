using EmployeeAPI.Interfaces;
using EmployeeAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
	
	[ApiController]
	[Route("api/[controller]")]
	public class EmployeesController : ControllerBase
	{
		private readonly IEmployeeRepository repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
		public IActionResult GetAll()
		{
			var employees = repository.GetAll();
			if (!employees.Any()) return NotFound();
			else return Ok(repository.GetAll());
		}

		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			var emp = repository.GetById(id);
			if (emp == null) return NotFound();
			return Ok(emp);
		}

		[HttpPost]
		public IActionResult Create(Employee e)
		{
			repository.Add(e);
			return Ok();
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Employee e)
		{
			e.EmployeeId = id;
			repository.Update(e);
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			repository.Delete(id);
			return Ok();
		}

		//[HttpDelete("byname/{name}")]
		//public IActionResult DeleteByName(string name)
		//{
		//	repository.DeleteByName(name);
		//	return Ok();
		//}


		// GET email
		[HttpGet("{id}/email")]
		public IActionResult GetEmail(int id)
		{
			var email = repository.GetEmail(id);

			if (email == null)
				return NotFound("Employee no trobat");

			return Ok(email);
		}
		// PUT email
		[HttpPut("{id}/email")]
		public IActionResult UpdateEmail(int id, [FromBody] string email)
		{
			repository.UpdateEmail(id, email);
			return NoContent();
		}
	}
}
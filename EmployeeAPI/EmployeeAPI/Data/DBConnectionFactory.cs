using MySql.Data.MySqlClient;

namespace EmployeeAPI.Data
{
	public class DBConnectionFactory
	{
		private readonly string connectionString;
        // crear un segon atribut per a l'accés al XML (path)
        // private readonly string xmlFilePath;

        public DBConnectionFactory(string connectionString) // crear el constructor que emplene ambdues variables
		{
			this.connectionString = connectionString;
		}

		// podem afegir les dades fitxer.json, encara que per al xml no cal si no voleu ja que el podem crear al mateix projecte

		// En comptes d'obrir la connexió directament, fer algun mètode com férem a primer que et permet canviar l'oritge de la font
		public MySqlConnection CreateConnection()
		{
			return new MySqlConnection(connectionString);
		}
	}
}

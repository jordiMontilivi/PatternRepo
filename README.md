# PatternRepo

## Objectiu del repositori

Aquest repositori conté dos projectes .NET:

- EmployeeAPI: API REST per gestionar empleats (CRUD + operacions d'email).
- ClientEmployee: aplicació de consola que consumeix l'API i permet provar les operacions principals.

La base de dades es desplega amb Docker Compose (MySQL + phpMyAdmin).

## Requisits previs

- Docker Desktop instal.lat i obert.
- .NET SDK compatible amb els projectes.
- Un gestor SQL (opcional): DBeaver, phpMyAdmin o consola MySQL.

## Posada en marxa del projecte

### 1) Obrir Docker Desktop

Comprova que Docker Desktop està en execució abans de continuar.

### 2) Alliberar el port 3306

Abans d'aixecar els serveis, para qualsevol contenidor o servei local que estigui usant el port 3306:

- Contenidors Docker antics.
- MySQL de XAMPP.

Si el port 3306 està ocupat, el contenidor de MySQL no podrà iniciar correctament.

### 3) Aixecar contenidors

Des de l'arrel del repositori, executa:

```bash
docker compose up -d
```

Això aixecarà:

- MySQL (contenidor employee_mysql) al port 3306.
- phpMyAdmin (contenidor employee_phpmyadmin) al port 8080.

### 4) Importar la base de dades (només la primera vegada)

Pots importar l'script [EmployeeDB.sql](EmployeeDB.sql) per consola, DBeaver o phpMyAdmin.

Exemple per consola MySQL:

```bash
docker exec -i employee_mysql mysql -uroot -prootpassword < EmployeeDB.sql
```

Si uses phpMyAdmin:

- URL: http://localhost:8080
- Host: mysql-db
- Usuari: root
- Password: rootpassword
- Importar l'arxiu SQL.

### 5) En acabar de treballar

És recomanable aturar o eliminar contenidors:

Aturar contenidors:

```bash
docker compose stop
```

Eliminar contenidors, imatges i volums:

```bash
docker compose down --rmi all -v
```

## Execució de l'API

Quan la connexió a base de dades ja està preparada, executa el servidor EmployeeAPI i deixa'l en segon pla.

Des de [EmployeeAPI/EmployeeAPI](EmployeeAPI/EmployeeAPI):

```bash
dotnet run
```

Per defecte, l'API queda disponible a:

- http://localhost:5000
- https://localhost:5001

Endpoints d'exemple:

- https://localhost:5001/api/employees
- https://localhost:5001/api/employees/2
- https://localhost:5001/api/employees/3/email

Swagger:

- https://localhost:5001/swagger

## Execució del client de consola

També pots usar ClientEmployee, una consola que permet accedir a l'API i provar operacions CRUD.

Des de [ClientEmployee/ClientEmployee](ClientEmployee/ClientEmployee):

```bash
dotnet run
```

El client està configurat per consumir:

- https://localhost:5001/api/

Per tant, has de tenir EmployeeAPI en execució abans d'arrencar el client.

## Revisió i explicació del codi

### EmployeeAPI

Fitxers clau:

- [EmployeeAPI/EmployeeAPI/Program.cs](EmployeeAPI/EmployeeAPI/Program.cs)
- [EmployeeAPI/EmployeeAPI/Controllers/EmployeeController.cs](EmployeeAPI/EmployeeAPI/Controllers/EmployeeController.cs)
- [EmployeeAPI/EmployeeAPI/Data/EmployeeRepository.cs](EmployeeAPI/EmployeeAPI/Data/EmployeeRepository.cs)
- [EmployeeAPI/EmployeeAPI/Data/DBConnectionFactory.cs](EmployeeAPI/EmployeeAPI/Data/DBConnectionFactory.cs)
- [EmployeeAPI/EmployeeAPI/Interfaces/IEmployeeRepository.cs](EmployeeAPI/EmployeeAPI/Interfaces/IEmployeeRepository.cs)

Com funciona:

1. Program.cs configura DI i registra:
	- DBConnectionFactory com singleton.
	- IEmployeeRepository amb implementació EmployeeRepository com scoped.
2. EmployeesController exposa endpoints REST:
	- GET /api/employees
	- GET /api/employees/{id}
	- POST /api/employees
	- PUT /api/employees/{id}
	- DELETE /api/employees/{id}
	- GET /api/employees/{id}/email
	- PUT /api/employees/{id}/email
3. EmployeeRepository encapsula accés a MySQL amb consultes SQL parametritzades.

Punts forts:

- Separació clara per capes (Controller, Repository, Model).
- Injecció de dependències correcta.
- Ús de paràmetres SQL per evitar injeccions.

Millores recomanades:

- Retornar codis HTTP més específics en POST/PUT/DELETE (per exemple Created, NoContent, NotFound quan pertoqui).
- Afegir validacions de model i control d'errors.
- Evitar doble crida al repositori a GetAll (actualment llegeix dues vegades la mateixa llista).
- Uniformitzar nom del fitxer/controlador (el fitxer es diu EmployeeController.cs però la classe és EmployeesController).

### ClientEmployee

Fitxers clau:

- [ClientEmployee/ClientEmployee/Program.cs](ClientEmployee/ClientEmployee/Program.cs)
- [ClientEmployee/ClientEmployee/EmployeeService.cs](ClientEmployee/ClientEmployee/EmployeeService.cs)
- [ClientEmployee/ClientEmployee/Model/Employee.cs](ClientEmployee/ClientEmployee/Model/Employee.cs)

Com funciona:

1. Program.cs actua com a flux de prova manual:
	- Llista empleats.
	- Consulta un empleat.
	- Consulta email.
	- Crea un empleat llegint dades des de consola.
	- Elimina un empleat.
	- Actualitza el sou d'un empleat de forma aleatòria.
2. EmployeeService centralitza les crides HTTP a l'API.
3. Model/Employee.cs mapeja JSON amb JsonPropertyName.

Punts forts:

- Bona separació entre la lògica de presentació (Program) i de comunicació HTTP (EmployeeService).
- Exemples pràctics de serialització/deserialització JSON.

Millores recomanades:

- Evitar imports duplicats a Program.cs.
- Comprovar codis de resposta també a CreateAsync (actualment pot marcar èxit sense validar status code).
- Gestionar millor errors de connexió/SSL i missatges d'usuari.
- Considerar IHttpClientFactory si el client creix en funcionalitat.

## Resum ràpid d'ús

1. Obrir Docker Desktop.
2. Parar serveis que ocupin el port 3306 (Docker/XAMPP).
3. Executar docker compose up -d.
4. Primera vegada: importar la BBDD.
5. Executar EmployeeAPI i deixar-lo corrent.
6. Provar endpoints per navegador/Swagger o executar ClientEmployee.
7. En acabar: docker compose stop o docker compose down --rmi all -v.
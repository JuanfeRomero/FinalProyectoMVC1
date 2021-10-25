CREATE TABLE [dbo].[RegistroUsuariosCodes]
(
	username VARCHAR(100) NOT NULL,
	lastname VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL,
	age INT NOT NULL,
	DNI INT PRIMARY KEY NOT NULL,
	gender CHAR(1) NOT NULL
);

INSERT INTO RegistroUsuariosCodes(username, lastname, email, age, DNI, gender) values ('Juanfe', 'Romero', 'jromero@codes.com.ar', 26, 38700297, 'M');

SELECT * FROM RegistroUsuariosCodes;
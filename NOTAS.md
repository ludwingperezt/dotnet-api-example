1.  Para listar las plantillas que se pueden usar para crear proyectos de .net:
dotnet new list

2.  Para crear un proyecto de webapi de .Net usando el cli de .net el comando es
dotnet new webapi -o <Nombre del proyecto>

3.  Para crear un archivo .gitignore de .net:
dotnet new gitignore

4.  Para correr un proyecto .net desde la terminal
dotnet run

En la consola aparecen las rutas para hacer peticiones al proyecto, en http y https 
(si es que está disponible).

5. Para ver la documentación swagger del proyecto:
http://localhost:5151/swagger/index.html

====================================================================================
En el archivo WebApiAutores.csproj se cambió la línea

<Nullable>enable</Nullable>
a esta otra:
<Nullable>disable</Nullable>

Para desactivar los tipos de referencia NO Nulos, así no será necesario estar marcando
como Nullable los tipos de referencia en la aplicación que puedan ser nulos, de lo 
contrario podrían aparecer errores.

====================
Por defecto los controllers terminan su nombre con "Controller"

Los decorators en C# se hacen con []

Para instalar entity framework core para SqlServer:
dotnet add package Microsoft.EntityFrameworkCore.SqlServer

(Este es necesario si se usa dotnet cli)
dotnet add package Microsoft.EntityFrameworkCore.Design

==========================

Dónde obtener las imagenes docker de ms sql server: 
https://mcr.microsoft.com/en-us/product/mssql/server/about
https://learn.microsoft.com/en-us/sql/linux/sql-server-linux-docker-container-configure?view=sql-server-ver16&pivots=cs1-bash

==========================

En el archivo appsettings.Development.json se colocan aquellos datos que no se deben poner en el código
como por ejemplo credenciales a base de datos, etc.

==========================

* Instalar primero dotnet-ef
https://learn.microsoft.com/en-us/ef/core/cli/dotnet
https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools#install-a-local-tool

* Para este proyecto se instaló ef como una tool local, así que fue necesario crear un tool-manifest
dotnet new tool-manifest

Y luego instalar:
dotnet tool install dotnet-ef

Este comando crea la migracion inicial (con el nombre Inicial):
dotnet ef migrations add Inicial

Con el siguiente comando se ejecutan todas las migraciones pendientes hacia la db (si la base de datos no existe la crea):
dotnet ef database update

==========================

* Un controlador es una clase que agrupa un conjunto de acciones, por lo regular, relacionadas
  a un recurso.

Los controladores se nombran por lo regular concatenando el nombre del recurso con la palabra Controller
y por lo general van en una carpeta llamada "Controllers", por ejemplo:
- AutoresController


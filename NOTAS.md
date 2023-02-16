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
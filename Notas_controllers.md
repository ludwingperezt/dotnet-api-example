Muchas veces se puede ver esto en un controller:

    [Route("api/[controller]")] // 
    public class AutoresController: ControllerBase  // 
    {
        ...
    }

En este caso lo que sucede es que esa ruta se convierte de api/[controller] a
api/autores es decir que se cambia el texto [controller] por la primera
parte del nombre de la clase del controller.


Un endpoint puede tener varias rutas, por ejemplo:

    [Route("api/autores")] // 
    public class AutoresController: ControllerBase  // 
    {

        [HttpGet]  // 
        [HttpGet("listado")]  // 
        [HttpGet("/listadox")]  // <-- Esta ruta sobreescribe la ruta del controlador
        public async Task<ActionResult<List<Autor>>> Get()
        {
            ...
        }
    }

Así que este endpoint puede ser accedido desde tres rutas simultaneamente:
- /api/autores
- /api/autores/listado
- /listadox
        
A las palabras HttpGet, HttpPost, etc encerrados entre corchetes se les llama atributos.
Ver más: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/


Cuando en un parámetro de URL esperamos un valor de un tipo determinado, podemos usar
algo que se conoce como "Restricción", que es indicar el tipo de dato en el parámetro de
la URL (si lo que se recibe no corresponde al tipo de dato requerido, entonces en lugar
de recibir un error 400 recibiríamos un 404):

    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put(Autor autor, int id)
    {
        ...
    }

En el caso de querer recibir un valor string, NO se debe poner :string porque daría error,
en ese caso NO se usan las restricciones:
    [HttpPut("{nombre}")]
    public async Task<ActionResult> Put(string nombre)
    {
        ...
    }

Se pueden usar varios parámetros en URL, así:
    [HttpGet("{id:int}/{nombre}")]
    public async Task<ActionResult> Get(int id, string nombre)
    {
        ...
    }

Un parámetro de URL puede ser opcional agregando ? luego del nombre
(en este caso si no se envía su valor sería nulo):
    [HttpGet("{id:int}/{nombre?}")]
    public async Task<ActionResult> Get(int id, string nombre)
    {
        ...
    }

Si se quiere un valor por defecto entonces se podría hacer esto
(para este caso si no se envía el valor entonces por defecto la variable
nombre tendría el valor "xyz"):
    [HttpGet("{id:int}/{nombre=xyz}")]
    public async Task<ActionResult> Get(int id, string nombre)
    {
        ...
    }

Model Binding:

Los parámetros que se pueden enviar a un endpoint pueden declararse de 
varias formas.

Por ejemplo, tradicionalmente a través de la ruta:

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
        }

Pero también se puede especificar a nivel de parámetros, de dónde se
quieren obtener los parámetros.  Eso se puede hacer con atributos:
- [FromHeader]  Para especificar que una variable debe venir a través del header
- [FromRoute]  Para especificar que una variable debe venir por una ruta
- [FromBody]  Para especificar que algun valor debe venir por el body de la petición
- [FromQuery]  Para especificar que algun valor debe venir por un query string
- [FromService]  Para especificar que algun valor viene de un servicio
- [FromForm]  Si un valor viene de una fuente con content type application/x-www-url-formencoded (usado comunmente para recibir archivos)


    [HttpPut("{id:int}")]
    public async Task<ActionResult> Put([FromBody] Autor autor, [FromRoute] int id, [FromHeader] string token, [FromQuery] string nombre)
    {
    }


Tipos de datos de retorno:

En este caso no estamos usando programación asíncrona. Si el autor es encontrado no hay
problema, pero de lo contrario debemos retornar una código de error 404 para indicar
que no fue encontrado, pero en este caso eso nos daría un error porque el tipo de dato
de retorno DEBE ser un objeto Autor y la llamada a NotFound() retorna un tipo de objeto
que deriva de ActionResult.

        [HttpGet("{id:int}")]
        public Autor Get(int id)
        {
            var autor = await context.Autores.FirstOrDefault(autorDB => autorDB.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            return autor;
        }

Para evitar esto se recomienda que se retorne un tipo de dato "genérico", que es del tipo
ActionResult o IActionResult.

Al especificar un tipo de dato de retorno ActionResult<T> se logra retornar un objeto de 
tipo Autor (para este ejemplo, pero puede ser de cualquier tipo siempre que se especificique) 
o cualquier objeto que derive de la clase ActionResult, como NotFound(), Ok(), etc.
Se recomienda usar como tipo de retorno ActionResult<T> para especificar el tipo de dato a
retornar.

        [HttpGet("{id:int}")]
        public ActionResult<Autor> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefault(autorDB => autorDB.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            // return 1; // esto no sería permitido
            // return "autor"; // esto no sería permitido

            return autor;
        }

En este otro caso, cuando el tipo de dato de retorno es IActionResult entonces cualquier objeto
que se quiera retornar se debe "envolver" en un objeto de cualquier clase que derive de ActionResult,
por ejemplo en este caso en un objeto retornado por la función Ok()

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var autor = await context.Autores.FirstOrDefault(autorDB => autorDB.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            // return Ok(1); // Esto funcionaría
            // return Ok(1); // Esto funcionaría
            return Ok(autor);
        }
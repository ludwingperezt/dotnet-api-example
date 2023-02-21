La programación asíncrona no debe usarse en todos los casos porque aunque ahora tiempo
en algunas tareas, tiene asociada cierta penalización de rendimiento.

Se debe usar programación asíncrona cuando se hagan operaciones de I/O
por ejemplo: Llamar API, operar con archivos, bases de datos.

Si la operación a realizar llama unicamente a datos en memoria entonces
no se recomienda usar programación asíncrona.

Para usar funciones asíncronas, éstas se deben marcar con la palabra clave
"async" y dentro de ella se puede usar la palabra "await"

public async Task<ActionResult> Put(Autor autor, int id)
{
    var x = await getData();
    ...
}

La programación asincrona requiere que se retorne un Task (que equivale a devolver void)
o Task<T> para devolver un tipo de dato T al finalizar la función.
Una Task representa una promesa.

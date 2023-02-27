# Fitros #

* Usados para correr código en determinados momentos del ciclo de vida del procesamiento
  de una petición HTTP.
* Son útiles para ejecutar lógica en varios controladores sin repetir código.

## Tipos
* Autorización: Determinan si un usuario puede realizar una acción determinada
* De recursos: Se pueden usar para validaciones generales o caché.  Estos filtros
    pueden detener la tubería de filtros, de tal manera que pueden evitar la ejecución
    de otros filtros y de los controladores.
* De acción: Se ejecutan justo antes y después de la ejecución de una acción
* De excpeción: Se ejecutan cuando sucede una excepción no controlada, en la creación
    de un controlador o durante el binding de modelo.
* De resultado: Se ejecutan antes y después de la ejecución de un action result.

## Alcance
* A nivel de acción
* A nivel de controlador
* A nivel global (todo el webapi)
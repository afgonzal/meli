# meli POC by Ale González

La API corre sobre .NET Core 3.2 para permitir portabilidad, esta configurado para correr sobre Docker para facilitar escalamiento.
En el archivo appsettings, está configurado la url de la API de Mercado Libre, y el nombre del archivo que usa para guardar las stats (este también se debe configurar en el nlog.config)

Se puede ver un diagrama de componentes en "Diagramas ML.png", pero se puede reducir a que tiene un endpoint en api/items/{id}, donde id es el identificador de ML para un item, 
por ej MLA885018383 

El resultado es un merge entre la FT y los atributos del item, manteniendo la misma agrupación que en la FT.
Según el tipo de componente, ademas del Value, pueden incluir en campos separados Number y Unit cuando corresponda.
Para los casos de Number_Output el Value contiene el number en letras pero solo en castellano.
{
    "id": "MLU472306587",
    "title": "Cartucho Original Hp 664 Negro + Color 2135 2675 3775",
    "groups": [
        {
            "label": "Características principales",
            "components": [
                {
                    "id": "BRAND",
                    "name": "Marca",
                    "value": "HP"
                },
                {
                    "id": "MODEL",
                    "name": "Modelo",
                    "value": "664"
                }
            ]
        },
        {
            "label": "Otras características",
            "components": [
                {
                    "id": "ITEM_CONDITION",
                    "name": "Condición del ítem",
                    "value": "Nuevo"
                }
            ]
        }
    ]
}

#Cliente API Mercado Libre (MeliClient.cs)
Se uso Streams para mejorar performance de la llamada, y se deserializa resultado a una entidad simplificada para minimizar allocation y tiempos.
Se usó un HttpClientFactory para optimizar manejo de sockets y evitar posibles "socket exhaustion".

#Unit testing. Solo se crearon para FichaTecnicaService dada que es el camino critico de la funcionalidad, y a modo de ejemplificar la cobertura de UT.

#Manejo errores, solo se capturan y logean a nivel de controller.
Se usó prog defensiva donde se consideró vital. 
 
#Comentarios y doc.
Summaries (reusables por Intellisense) a nivel de interfaces o metodos donde fuera relevante

#Dependencias que usa (No MSFT)
NLog para logging.
Newtonsoft.JSON para Serializar/Deserializar
Automapper para mapeo DTOs
NUnit para Unittesting

#Seguridad
No hay configurada ningun tipo de auth, se consume anonimamente, como la api de ML.

#Stats se registra cada llamada a la api de Items. Con el objetivo de simplificar las stats se guardan en un archivo plano (generado por NLog)
Una posible mejora es usar una BD NoSQL para manejar otros volumenes de datos no considerados en esta POC.
Formato de las stats en el log
2020/11/15 23:12:08.383Z|Event Items.GetAttributes-MLU472306587.|Items.GetAttributes|MLU472306587

A modo de ejemplo el endpoint api/stats/{id} devuelve la lista de llamadas a la API para un cierto ItemId.
y api/stats/dateRange?dateFrom=xxx&dateTo=xxx devuelve la lista de llamadas a la API entre dateFrom y dateTo
No se hizo logica de aggregation, u otros que sea algo mas elaborado para consumir las estadisticas

#Performance, Los tiempos requeridos (<10ms) no se logran. El cuello de botella (800ms avg) está en la llamada a la API de ML.
En el logging se hace trazeo de los tiempos de cada request/response.

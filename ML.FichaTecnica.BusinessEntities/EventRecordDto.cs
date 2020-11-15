using System;

namespace ML.FichaTecnica.BusinessEntities
{
    public class EventRecordDto
    {
        /// <summary>
        /// Fecha y hora (y Timezone) de registrado el evento
        /// </summary>
        public DateTimeOffset DateTime { get; set; }
        /// <summary>
        /// Nombre del Evento (usualmente nombre del metodo)
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Alguna descricion extra del evento
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Id, parametro del metodo llamado durante el evento
        /// </summary>
        public string Id { get; set; }
    }
}

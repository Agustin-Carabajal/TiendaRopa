namespace TiendaRopa.Servicio.ServiciosHttp
{
    public static class HttpRespuestaExtensiones
    {
        /// <summary>
        /// Devuelve el mensaje que mandó el servidor (por ejemplo "Stock insuficiente para ...")
        /// y, si no hay uno legible, el mensaje genérico de HttpRespuesta.ObtenerError().
        /// </summary>
        public static async Task<string> ObtenerMensajeAsync<T>(this HttpRespuesta<T> respuesta)
        {
            if (!respuesta.Error) return string.Empty;

            try
            {
                var cuerpo = (await respuesta.HttpResponseMessage.Content.ReadAsStringAsync()).Trim();

                // Los BadRequest/NotFound con texto llegan como texto plano; los errores de
                // validación automática llegan como JSON (ProblemDetails) y no sirven para mostrar.
                if (cuerpo.Length > 0 && !cuerpo.StartsWith('{') && !cuerpo.StartsWith('['))
                {
                    return cuerpo.Trim('"');
                }
            }
            catch
            {
                // Si no se puede leer el cuerpo se usa el mensaje genérico.
            }

            return respuesta.ObtenerError();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace TiendaRopa.Servicio.ServiciosHttp
{
    public class HttpServicio : IHttpServicio
    {
        private readonly HttpClient _httpClient;

        public HttpServicio(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<HttpRespuesta<T>> Get<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerializar<T>(response);
                return new HttpRespuesta<T>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<T>(default, true, response);
            }
        }

        public async Task<HttpRespuesta<TResp>> Post<T, TResp>(string url, object enviar)
        {
            var enviarJson = JsonSerializer.Serialize(enviar);
            var enviarContent = new StringContent(enviarJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, enviarContent);
            if (response.IsSuccessStatusCode)
            {
                var respuesta = await DesSerializar<TResp>(response);
                return new HttpRespuesta<TResp>(respuesta, false, response);
            }
            else
            {
                return new HttpRespuesta<TResp>(default, true, response);
            }
        }

        public async Task<HttpRespuesta<object>> Delete(string url)
        {
            var response = await _httpClient.DeleteAsync(url);

            return new HttpRespuesta<object>(null, !response.IsSuccessStatusCode, response);


        }

        public async Task<T?> DesSerializar<T>(HttpResponseMessage response)
        {
            var respStr = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(respStr, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}

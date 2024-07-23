using System.Net;

namespace Models.DTOs
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; }
        public string Mensaje { get; set; }
        public object Resultado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Aplicacion.ManejadorError
{
     public  class ManejadorExcepcion:Exception
    {
        public HttpStatusCode _statusCode { get; }
        public object _errores { get; }
        public ManejadorExcepcion(HttpStatusCode statusCode, object errores=null )
        {
            _statusCode = statusCode;
            _errores = errores;

        }
    }
}

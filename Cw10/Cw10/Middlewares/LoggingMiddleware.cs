
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cw10.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            string path = "./requestsLog.txt";

            string line = "";

            line += "Nazwa methody: " + httpContext.Request.Method + "\n";
            line += "Path: " + httpContext.Request.Path + "\n";
            line += "Body: " + httpContext.Request.ContentType + "\n";
            line += "Query: " + httpContext.Request.QueryString.Value + "\n";


            Writer(path, line);
            await _next(httpContext);


        }

        private void Writer(string path, string s)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(s);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
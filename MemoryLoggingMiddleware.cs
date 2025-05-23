using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;

namespace UI.CSharpAttributes
{
    public class MemoryLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MemoryLoggingMiddleware> _logger;

        public MemoryLoggingMiddleware(RequestDelegate next, ILogger<MemoryLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var before = GC.GetTotalMemory(false);
            var sw = Stopwatch.StartNew();

            await _next(context);

            sw.Stop();
            var after = GC.GetTotalMemory(false);
            var diff = after - before;

            _logger.LogInformation($"[{context.Request.Method}] {context.Request.Path} | Mem↑: {diff / 1024} KB | Time: {sw.ElapsedMilliseconds} ms");

            var log = new Log { message = $"[{context.Request.Method}] {context.Request.Path}" }.Find<Log>();
            if (log != null)
            {
                log.body = $"[{context.Request.Method}] {context.Request.Path} | Mem↑: {diff / 1024} KB | Time: {sw.ElapsedMilliseconds} ms";
                log.Fecha = DateTime.Now;
                log.Update(log.body);                
            }
            else
            {
                new Log()
                {
                    LogType = LogType.MEMORYINFO.ToString(),
                    message = $"[{context.Request.Method}] {context.Request.Path}",
                    Fecha = DateTime.Now,
                    body = $"[{context.Request.Method}] {context.Request.Path} | Mem↑: {diff / 1024} KB | Time: {sw.ElapsedMilliseconds} ms",
                }.Save();
            }


        }
    }
}
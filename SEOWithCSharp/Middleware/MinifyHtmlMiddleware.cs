using System.Text.RegularExpressions;

namespace SEOWithCSharp.Middleware;

public class MinifyHtmlMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stream = context.Response.Body;
        try
        {
            using var buffer = new MemoryStream();
            
            context.Response.Body = buffer;

            await next(context);

            var isHtml = context.Response.ContentType?.ToLower().Contains("text/html");
            if (!isHtml.GetValueOrDefault())
            {
                return;
            }

            context.Response.Body = stream;

            buffer.Seek(0, SeekOrigin.Begin);

            var content = await new StreamReader(buffer).ReadToEndAsync();
            if (context.Response.StatusCode == 200 && isHtml.GetValueOrDefault())
            {
                content = Regex.Replace(
                        content,
                        @"(?<=\s)\s+(?![^<>]*</pre>)",
                        string.Empty, RegexOptions.Compiled)
                    .Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
            }

            await context.Response.WriteAsync(content);
        }
        finally
        {
            context.Response.Body = stream;
        }
    }
}

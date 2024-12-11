namespace SEOWithCSharp.Middleware;

public static class MinifyHtmlExtensions
{
    public static IApplicationBuilder UseMinifyHtml(
        this IApplicationBuilder builder) =>
        builder.UseMiddleware<MinifyHtmlMiddleware>();
}
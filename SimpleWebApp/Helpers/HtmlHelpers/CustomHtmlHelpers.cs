using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using SimpleWebApp.Models;

namespace SimpleWebApp.Helpers.HtmlHelpers;

public static class CustomHtmlHelpers
{
    public static IHtmlContent NorthwindImageLink(this IHtmlHelper htmlHelper, int id, string other)
    {
        return new HtmlString($"<a href=\"/Image/{id}\">{other}</a>");
    }
    public static IHtmlContent NorthwindImage(this IHtmlHelper htmlHelper, Category cat)
    {
        return new HtmlString($"<a href=\"/Image/{cat.CategoryId}\"><img height=\"100\" width=\"100\" src=\"{cat.GetBase64Image()}\" alt=\"\"></a>");
    }
}
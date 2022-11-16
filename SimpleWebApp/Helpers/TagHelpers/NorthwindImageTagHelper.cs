using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SimpleWebApp.Helpers.TagHelpers;

[HtmlTargetElement("a", Attributes = AttibuteName)]
public class NorthwindImageTagHelper:TagHelper
{
    private const string AttibuteName = "northwind-id";
    private const string ImageAddress = "/Image/";

    [HtmlAttributeName(AttibuteName)]
    public string ImageId { get; set; }
    
    public override void Process(TagHelperContext ctx, TagHelperOutput output)
    {
        var addr = $"{ImageAddress}{ImageId}";
        output.Attributes.SetAttribute("href",addr); 
    }
}
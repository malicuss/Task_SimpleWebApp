using Microsoft.AspNetCore.Mvc;

namespace SimpleWebApp.ViewComponents;

public class BreadCrumbsViewComponent : ViewComponent
{
    private readonly ILogger<BreadCrumbsViewComponent> _logger;

    public BreadCrumbsViewComponent(ILogger<BreadCrumbsViewComponent> logger)
    {
        _logger = logger;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
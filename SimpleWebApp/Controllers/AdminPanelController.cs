using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleWebApp.Controllers;

[Authorize(Policy = "RequireAdministratorRole")]
public class AdminPanelController : Controller
{
    private readonly ILogger<AdminPanelController> _logger;
    private readonly SecurityContext _securityContext;

    public AdminPanelController(ILogger<AdminPanelController> logger, SecurityContext securityContext)
    {
        _logger = logger;
        _securityContext = securityContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var k = _securityContext.Users.ToList();
        return View(k);
    }
}
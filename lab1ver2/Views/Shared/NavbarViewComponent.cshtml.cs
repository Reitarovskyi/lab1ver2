using lab1ver2.Models;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc;

namespace lab1ver2.Views.Shared
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IHtmlLocalizer<SharedResource> _localizer;

        public NavbarViewComponent(IHtmlLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public IViewComponentResult Invoke()
        {
            var navbarItems = new[]
            {
            new NavbarItem { Controller = "Home", Action = "Index", Text = _localizer["posts"].Value, IsActive = true },
            new NavbarItem { Controller = "Home", Action = "About", Text = _localizer["about"].Value },
            new NavbarItem { Controller = "Home", Action = "Contact", Text = _localizer["contact"].Value },
            new NavbarItem { Controller = "Posts", Action = "Index", Text = _localizer["post manager"].Value },
        };

            return View("NavbarViewComponent.cshtml", navbarItems);
        }
    }
}

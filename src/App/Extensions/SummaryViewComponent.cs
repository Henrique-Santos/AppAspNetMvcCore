using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotifier _notifier;

        public SummaryViewComponent(INotifier notifier)
        {
            _notifier = notifier;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Pelo método GetNotifications não ser assincrono, é necessario o uso do Task.FromResult
            var notifications = await Task.FromResult(_notifier.GetNotifications());
            // Adicionando os erros no Model State
            notifications.ForEach(notification => ViewData.ModelState.AddModelError(string.Empty, notification.Message));
            return View();
        }
    }
}
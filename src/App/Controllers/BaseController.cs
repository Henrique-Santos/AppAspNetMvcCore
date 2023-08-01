using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotifier _notifier;

        protected BaseController(INotifier notifier)
        {
            _notifier = notifier;
        }

        protected bool IsValidOperation()
        {
            return !_notifier.HasNotification();
        }
    }
}
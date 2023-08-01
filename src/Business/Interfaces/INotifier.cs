using Business.Notifications;

namespace Business.Interfaces
{
    public interface INotifier
    {
        void Handle(Notification notification);
        bool HasNotification();
        List<Notification> GetNotifications();
    }
}
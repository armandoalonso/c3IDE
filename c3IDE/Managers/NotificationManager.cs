using System;

namespace c3IDE.Managers
{
    public static class NotificationManager
    {
        private static Action<string> _notificationCallback;
        private static Action<string> _errorCallback;

        /// <summary>
        /// sets the notification callback, used to display notifications
        /// </summary>
        /// <param name="callback"></param>
        public static void SetInfoCallback(Action<string> callback)
        {
            _notificationCallback = callback;
        }

        /// <summary>
        /// sets up the notification callback used 
        /// </summary>
        /// <param name="callback"></param>
        public static void SetErrorCallback(Action<string> callback)
        {
            _errorCallback = callback;
        }

        /// <summary>
        /// publishes a new notification
        /// </summary>
        /// <param name="notification"></param>
        public static void PublishNotification(string notification)
        {
            _notificationCallback?.Invoke(notification);
        }

        /// <summary>
        /// publishes a new error notification
        /// </summary>
        /// <param name="notification"></param>
        public static void PublishErrorNotification(string notification)
        {
            _errorCallback?.Invoke(notification);
        }
    }
}

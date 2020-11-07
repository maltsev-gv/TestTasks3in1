using System;
using System.Windows;

namespace TestTask.Services
{
    public class ThreadHelper
    {
        public static void RunInMainThread(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}

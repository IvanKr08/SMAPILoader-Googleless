using Android.App;
using Android.Widget;

using Xamarin.Essentials;

namespace SMAPIGameLoader;

internal static class ToastNotifyTool
{
    static Toast LastToast;
    static void NotifyAtMainThread(string message, ToastLength duration = ToastLength.Long)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Notify(message, duration);
        });
    }
    public static void Notify(string message, ToastLength duration = ToastLength.Long)
    {
        if (MainThread.IsMainThread == false)
        {
            NotifyAtMainThread(message, duration);
            return;
        }

        if (LastToast != null)
            LastToast?.Cancel();

        LastToast = Toast.MakeText(Application.Context, message, duration);
        LastToast.Show();
    }
}

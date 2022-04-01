using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace VCCSharp.Models;

internal class ThreadRunner
{
    public bool IsRunning;

    public ThreadRunner()
    {
        Application.Current.Exit += (_, _) => IsRunning = false;
    }

    public void Run(Action callback)
    {
        if (IsRunning) return;

        IsRunning = true;

        while (IsRunning)
        {
            Thread.Sleep(100);

            if (IsRunning)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(callback);
                }
                catch (InvalidCastException)
                {
                    Debug.WriteLine("InvalidCastException occurred on callback dispatch");
                }
            }
        }
    }
}

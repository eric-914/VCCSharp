using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DX8.Tester.Model;

public delegate void ThreadRunnerEventHandler(object? sender, EventArgs e);

internal class ThreadRunner
{
    public event ThreadRunnerEventHandler? Tick;

    public bool IsRunning { get; private set; }

    public int Interval { get; set; } = 200;

    public ThreadRunner()
    {
        Application.Current.Exit += (_, _) => Stop();
    }

    public void Start()
    {
        Debug.WriteLine("ThreadRunner.Start()");
        IsRunning = true;

        Task.Run(() =>
        {
            while (IsRunning)
            {
                Thread.Sleep(Interval);

                if (IsRunning)
                {
                    Application.Current.Dispatcher.Invoke(() => Tick?.Invoke(this, EventArgs.Empty));
                }
            }
        });
    }

    public void Stop()
    {
        IsRunning = false;
        Debug.WriteLine("ThreadRunner.Stop()");
    }
}

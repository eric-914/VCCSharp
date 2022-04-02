using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DX8.Tester.Model;

public delegate void ThreadRunnerEventHandler(object? sender, EventArgs e);

internal class ThreadRunner
{
    public event ThreadRunnerEventHandler? Tick;

    private bool _isRunning;

    public void Run()
    {
        _isRunning = true;

        Task.Run(() =>
        {
            while (_isRunning)
            {
                Thread.Sleep(100);

                if (_isRunning)
                {
                    Application.Current.Dispatcher.Invoke(() => Tick?.Invoke(this, EventArgs.Empty));
                }

            }
        });
    }

    public void Stop()
    {
        _isRunning = false;
    }
}

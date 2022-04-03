using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DX8.Tester.Model;

public delegate void ThreadRunnerEventHandler(object? sender, EventArgs e);

public interface IThreadRunner
{
    event ThreadRunnerEventHandler? Tick;
    bool IsRunning { get; }
    int Interval { get; set; }
    void Start();
    void Stop();
}

internal class ThreadRunner : IThreadRunner
{
    private readonly Dispatcher _dispatcher;
    public event ThreadRunnerEventHandler? Tick;

    public bool IsRunning { get; private set; }

    public int Interval { get; set; } = 200;

    public ThreadRunner(Dispatcher dispatcher)
    {
        _dispatcher = dispatcher;
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
                    try
                    {
                        _dispatcher.Invoke(() => Tick?.Invoke(this, EventArgs.Empty));
                    }
                    catch (TaskCanceledException)
                    {
                        //--This can happen if you're debugging the shutdown process.
                        Debug.WriteLine("Dispatcher.Invoke task cancelled.");
                    }
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

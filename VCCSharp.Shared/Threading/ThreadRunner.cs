using System.Diagnostics;
using VCCSharp.Shared.Models;

namespace VCCSharp.Shared.Threading;

public delegate void TickEventHandler(object? sender, EventArgs e);

public interface IThreadRunner
{
    event TickEventHandler? Tick;
    bool IsRunning { get; }
    int Interval { get; set; }
    void Start();
    void Stop();
}

public class ThreadRunner : IThreadRunner
{
    public event TickEventHandler? Tick;

    private readonly IDispatcher _dispatcher;
    private readonly IInterval _configuration = new NullInterval();

    public bool IsRunning { get; private set; }

    public int Interval { get => _configuration.Interval; set => _configuration.Interval = value; }

    public ThreadRunner(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public ThreadRunner(IDispatcher dispatcher, IInterval configuration)
    {
        _dispatcher = dispatcher;
        _configuration = configuration;
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

using System;
using System.Threading;

namespace DX8.Tester
{
    internal class ThreadRunner
    {
        public bool IsRunning = true;

        public void Run(Action callback)
        {
            while (IsRunning)
            {
                Thread.Sleep(100);
                callback();
            }
        }
    }
}

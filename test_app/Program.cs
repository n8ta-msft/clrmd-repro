
using System.Diagnostics;
using Microsoft.Diagnostics.Runtime;

public static class Program
{

    public static object shared_lock = new object();
    
    public static void DoWork()
    {
        for(int i = 0; i<30000;i++) {
            Console.WriteLine("Working thread...");
            Thread.Sleep(100);
        }
    }

    public static void HoldLock()
    {
        lock (shared_lock)
        {
            Thread.Sleep(1000000);
        }
    }
    
    public static int Main()
    {

        var test_lock = new object();
        var t1 = new Thread(Program.DoWork);
        t1.Start();
        var t2 = new Thread(Program.DoWork);
        t2.Start();
        var t3 = new Thread(Program.HoldLock);
        t3.Start();
        var t4 = new Thread(Program.HoldLock);
        t4.Start();
        
        var stacks = new Dictionary<int, string[]?>();
            
        using DataTarget dataTarget = DataTarget.AttachToProcess(Process.GetCurrentProcess().Id, true);
        using ClrRuntime runtime = dataTarget.ClrVersions.Single().CreateRuntime();

        foreach (ClrThread t in runtime.Threads)
        {
            try
            {
                stacks.Add(
                    t.ManagedThreadId,
                    // Max frames: 10000 since EnumerateStackTrace may loop indefinitely for corrupted stacks.
                    t.EnumerateStackTrace(false, maxFrames: 1000).Select(f =>
                    {
                        if (f.Method != null)
                        {
                            return f.Method.Type.Name + "." + f.Method.Name;
                        }
                        return "";
                    }).ToArray()
                );
            }
            catch (Exception e)
            {
                stacks.Add(t.ManagedThreadId, new string[] { $"Failed with exception {e}" });
            }
        }
        
        
        foreach (var thread in stacks)
        {
            Console.WriteLine($"{thread.Key} {string.Join(',', thread.Value)}");
        }
        
        return 0;
    }
}
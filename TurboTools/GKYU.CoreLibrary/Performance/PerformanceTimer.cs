using System;
using System.Collections.Generic;

namespace GKYU.CoreLibrary.Performance
{
    using System.Diagnostics;

    /// <summary>
    /// The PerformanceTimer is used to record events during profiling and testing of components and features
    /// </summary>
    public class PerformanceTimer
    {
        public string Name { get; set; }
        public Stopwatch StopWatch { get; set; }
        public long startTime = 0;
        public Dictionary<string, long> Events { get; set; }
        public long endTime = 0;
        public long Duration
        {
            get
            {
                return StopWatch.ElapsedMilliseconds - startTime;
            }
        }

        public long TotalTime
        {
            get
            {
                return endTime - startTime;
            }
        }
        public Dictionary<string, object> Metrics { get; set; }

        public PerformanceTimer()
        {
            StopWatch = new Stopwatch();
            Events = new Dictionary<string, long>();
            Metrics = new Dictionary<string, object>();
        }
        public PerformanceTimer(PerformanceTimer reference)
        {
            Name = reference.Name;
            StopWatch = reference.StopWatch;
            startTime = reference.startTime;
            Events = reference.Events;
            endTime = reference.endTime;
            Metrics = reference.Metrics;
        }
        public PerformanceTimer(string name)
        {
            Name = name;
            StopWatch = new Stopwatch();
            Events = new Dictionary<string, long>();
            Metrics = new Dictionary<string, object>();
        }
        public static long Compare(PerformanceTimer test1, PerformanceTimer test2)
        {
            long deltaRuntime = test1.TotalTime - test2.TotalTime;
            Console.WriteLine("Runtime Delta = {0}", deltaRuntime);
            return test1.TotalTime - test2.TotalTime;
        }
        public long DeltaTime(string endEvent, string beginEvent)
        {
            return Events[endEvent] - Events[beginEvent];
        }
        public long Subtract(PerformanceTimer timer, string eventName)
        {
            return Events[eventName] - timer.Events[eventName];
        }
        public void Start()
        {
            StopWatch = Stopwatch.StartNew();
        }
        public void Mark(string eventName)
        {
            Events.Add(eventName, StopWatch.ElapsedMilliseconds);
        }
        public void Stop()
        {
            endTime = StopWatch.ElapsedMilliseconds;
        }
        public void Report()
        {
            Console.WriteLine("****************************************************************");
            Console.WriteLine("Report:  {0}", Name);
            Console.WriteLine("****************************************************************");
            // Events
            Console.WriteLine("****  Events:");
            foreach (string key in Events.Keys)
            {
                Console.WriteLine("{0:0000} :  {1}", Events[key], key);
            }
            // Static Metrics
            Console.WriteLine("****  Static Metrics:");
            Console.WriteLine("Duration:  {0}", TotalTime);
            // Dynamic Metrics
            Console.WriteLine("****  Dynamic Metrics:");
            foreach (KeyValuePair<string, object> pair in Metrics)
            {
                Console.WriteLine("{0}:  {1}", pair.Key, pair.Value.ToString());
            }
        }
    }
}

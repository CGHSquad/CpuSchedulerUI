using System;
using System.Collections.Generic;
using System.Linq;
using CpuSchedulerUI.Models;
using System.Threading.Tasks;

namespace CpuSchedulerUI
{
    public static class Utils
    {
        public static int LastRunCpuActiveTime { get; set; }
        public static int LastRunTotalTime { get; set; }
        public static List<Process> GenerateProcesses(int count)
        {
            var list = new List<Process>();
            var rand = new Random();
            int currentTime = 0;

            for (int i = 0; i < count; i++)
            {
                currentTime += rand.Next(1, 4);
                list.Add(new Process
                {
                    Id = i + 1,
                    ArrivalTime = currentTime,
                    BurstTime = rand.Next(2, 6),
                    Priority = rand.Next(1, 4),
                    RemainingTime = 0,
                    StartTime = -1
                });
            }

            return list;
        }

        public static (string output, double avgWt, double avgTat) FormatResultWithMetrics(List<Process> processes, string algo)
        {
            processes = processes.OrderBy(p => p.Id).ToList();

            double totalWt = 0, totalTat = 0;
            int count = processes.Count;
            var valid = processes.Where(p => p.StartTime >= 0 && p.CompletionTime > 0).ToList();
            int burst = valid.Sum(p => p.BurstTime);
            int firstStart = valid.Min(p => p.ArrivalTime);
            int lastEnd = valid.Max(p => p.CompletionTime);
            int elapsed = Math.Max(1, lastEnd - firstStart); // avoid divide by 0

            double cpuUtilization = (double)Utils.LastRunCpuActiveTime / Utils.LastRunTotalTime * 100;

            var nl = Environment.NewLine;
            var output = $"Results for {algo}:{nl}{nl}";
            output += $"PID | Arrival | Burst | Start | Complete | Waiting | Turnaround{nl}";
            output += new string('-', 65) + nl;

            foreach (var p in processes)
            {
                if (p.CompletionTime <= 0 || p.StartTime < 0)
                    continue;

                output += $"{p.Id,3} | {p.ArrivalTime,7} | {p.BurstTime,5} | {p.StartTime,5} | {p.CompletionTime,8} | {p.WaitingTime,7} | {p.TurnaroundTime,10}{nl}";
                totalWt += p.WaitingTime;
                totalTat += p.TurnaroundTime;
            }

            double avgWt = totalWt / count;
            double avgTat = totalTat / count;

            output += $"\nAverage Waiting Time: {avgWt:F2}{nl}";
            output += $"Average Turnaround Time: {avgTat:F2}{nl}";
            output += $"CPU Utilization: {cpuUtilization:F2}%{nl}";

            return (output, avgWt, avgTat);
        }
    }
}
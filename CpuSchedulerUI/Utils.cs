using System;
using System.Collections.Generic;
using System.Linq;
using CpuSchedulerUI.Models;
using System.Threading.Tasks;

namespace CpuSchedulerUI
{
    public static class Utils
    {
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

        public static string FormatResult(List<Process> processes, string algo)
        {
            processes = processes.OrderBy(p => p.Id).ToList();

            double totalWt = 0, totalTat = 0;
            int count = processes.Count;
            int burst = processes.Sum(p => p.BurstTime);
            int last = processes.Max(p => p.CompletionTime);
            int elapsed = last;

            var nl = Environment.NewLine;
            var output = $"Results for {algo}:{nl}{nl}";
            output += $"PID | Arrival | Burst | Start | Complete | Waiting | Turnaround{nl}";
            output += new string('-', 65) + nl;

            foreach (var p in processes)
            {
                // skip invalid data
                if (p.CompletionTime <= 0 || p.StartTime < 0)
                    continue;

                output += $"{p.Id,3} | {p.ArrivalTime,7} | {p.BurstTime,5} | {p.StartTime,5} | {p.CompletionTime,8} | {p.WaitingTime,7} | {p.TurnaroundTime,10}{nl}";
                totalWt += p.WaitingTime;
                totalTat += p.TurnaroundTime;
            }

            double avgWt = totalWt / count;
            double avgTat = totalTat / count;
            double throughput = elapsed > 0 ? (double)count / elapsed : 0;
            double cpuUtilization = elapsed > 0 ? (double)burst / elapsed * 100 : 0;

            output += $"\nAverage Waiting Time: {avgWt:F2}{nl}";
            output += $"Average Turnaround Time: {avgTat:F2}{nl}";
            output += $"Throughput: {throughput:F2} processes/unit time{nl}";
            output += $"CPU Utilization: {cpuUtilization:F2}%{nl}";

            return output;
        }
    }
}
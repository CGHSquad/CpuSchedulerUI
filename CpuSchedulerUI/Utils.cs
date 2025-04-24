using System;
using System.Collections.Generic;
using System.Linq;
using CpuSchedulerUI.Models;
namespace CpuSchedulerUI;

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
                Priority = rand.Next(1, 4)
            });
        }

        return list;
    }

    public static string FormatResult(List<Process> processes, string algo)
    {
        double totalWT = 0, totalTAT = 0;
        int first = processes.Min(p => p.ArrivalTime);
        int last = processes.Max(p => p.CompletionTime);
        int burst = processes.Sum(p => p.BurstTime);

        var output = $"Results for {algo}:\n\n";
        output += $"PID | Arrival | Burst | Start | Complete | Waiting | Turnaround\n";
        foreach (var p in processes.OrderBy(p => p.Id))
        {
            output += $"{p.Id,3} | {p.ArrivalTime,7} | {p.BurstTime,5} | {p.StartTime,5} | {p.CompletionTime,8} | {p.WaitingTime,7} | {p.TurnaroundTime,10}\n";
            totalWT += p.WaitingTime;
            totalTAT += p.TurnaroundTime;
        }

        output += $"\nAverage Waiting Time: {totalWT / processes.Count:F2}\n";
        output += $"Average Turnaround Time: {totalTAT / processes.Count:F2}\n";
        output += $"Throughput: {(double)processes.Count / (last - first):F2} processes/unit time\n";
        output += $"CPU Utilization: {(double)burst / (last - first) * 100:F2}%\n";

        return output;
    }
}
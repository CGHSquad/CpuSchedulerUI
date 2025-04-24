using System;
using System.Collections.Generic;
using System.Linq;
using CpuSchedulerUI.Models;

namespace CpuSchedulerUI.Schedulers
{
    public static class SrtfScheduler
    {
        public static List<Process> Run(List<Process> original)
        {
            var processes = original.Select(p => p.Clone()).ToList();
            foreach (var p in processes)
                p.RemainingTime = p.BurstTime;

            int time = 0, completed = 0;

            while (completed < processes.Count)
            {
                var available = processes
                    .Where(p => p.ArrivalTime <= time && p.RemainingTime > 0)
                    .OrderBy(p => p.RemainingTime)
                    .ThenBy(p => p.ArrivalTime)
                    .FirstOrDefault();

                if (available == null)
                {
                    time++;
                    continue;
                }

                if (available.StartTime == -1)
                    available.StartTime = time;

                available.RemainingTime--;
                time++;

                if (available.RemainingTime == 0)
                {
                    available.CompletionTime = time;
                    completed++;
                }
            }

            PrintResults(processes, "SRTF");
            return processes;
        }

        private static void PrintResults(List<Process> processes, string label)
        {
            Console.WriteLine($"\nResults for {label}:");
            double totalWT = 0, totalTAT = 0;

            foreach (var p in processes)
            {
                Console.WriteLine($"P{p.Id} - AT: {p.ArrivalTime}, BT: {p.BurstTime}, CT: {p.CompletionTime}, WT: {p.WaitingTime}, TAT: {p.TurnaroundTime}");
                totalWT += p.WaitingTime;
                totalTAT += p.TurnaroundTime;
            }

            Console.WriteLine($"\nAverage Waiting Time: {totalWT / processes.Count:F2}");
            Console.WriteLine($"Average Turnaround Time: {totalTAT / processes.Count:F2}");
        }
        
    }
}

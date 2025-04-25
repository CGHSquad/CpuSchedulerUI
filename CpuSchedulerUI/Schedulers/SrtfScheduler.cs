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

            int time = 0, completed = 0, activeTime = 0;

            while (completed < processes.Count)
            {
                var available = processes
                    .Where(p => p.ArrivalTime <= time && p.RemainingTime > 0)
                    .OrderBy(p => p.RemainingTime)
                    .ThenBy(p => p.ArrivalTime)
                    .FirstOrDefault();

                if (available == null)
                {
                    time++; // CPU idle
                    continue;
                }

                if (available.StartTime == -1)
                    available.StartTime = time;

                available.RemainingTime--;
                time++;
                activeTime++;

                if (available.RemainingTime == 0)
                {
                    available.CompletionTime = time;
                    completed++;
                }
            }

            Utils.LastRunCpuActiveTime = activeTime; // Store for later metrics
            Utils.LastRunTotalTime = time;
            return processes;
        }
    }
}
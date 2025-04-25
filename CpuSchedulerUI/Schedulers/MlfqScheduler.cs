using System;
using System.Collections.Generic;
using System.Linq;
using CpuSchedulerUI.Models;

namespace CpuSchedulerUI.Schedulers
{
    public static class MlfqScheduler
    {
        public static List<Process> Run(List<Process> original)
        {
            var processes = original.Select(p => p.Clone()).ToList();
            foreach (var p in processes)
                p.RemainingTime = p.BurstTime;

            Queue<Process> q1 = new Queue<Process>();
            Queue<Process> q2 = new Queue<Process>();
            Queue<Process> q3 = new Queue<Process>();

            int time = 0, completed = 0, activeTime = 0;
            int q1Quantum = 4, q2Quantum = 8;

            while (completed < processes.Count)
            {
                foreach (var p in processes.Where(p => p.ArrivalTime == time))
                    q1.Enqueue(p);

                if (RunQueue(q1, q1Quantum, ref time, ref completed, ref activeTime, processes)) continue;
                if (RunQueue(q2, q2Quantum, ref time, ref completed, ref activeTime, processes)) continue;
                if (RunQueue(q3, -1, ref time, ref completed, ref activeTime, processes)) continue;

                time++; // CPU idle
            }

            Utils.LastRunCpuActiveTime = activeTime;
            Utils.LastRunTotalTime = time;
            return processes;
        }

        private static bool RunQueue(Queue<Process> queue, int quantum, ref int time, ref int completed, ref int activeTime, List<Process> all)
        {
            if (queue.Count == 0) return false;

            var p = queue.Dequeue();
            if (p.StartTime == -1) p.StartTime = time;

            int slice = (quantum == -1 || p.RemainingTime <= quantum) ? p.RemainingTime : quantum;

            for (int i = 0; i < slice; i++)
            {
                time++;
                p.RemainingTime--;
                activeTime++;

                int currentTime = time;
                var arrivals = all.Where(pr => pr.ArrivalTime == currentTime).ToList();
                foreach (var newProc in arrivals)
                    queue.Enqueue(newProc);
            }

            if (p.RemainingTime == 0)
            {
                p.CompletionTime = time;
                completed++;
            }
            else
            {
                queue.Enqueue(p);
            }

            return true;
        }
    }
}
using System.Collections.Generic;
using CpuSchedulerUI.Models;
using CpuSchedulerUI.Schedulers;
using NUnit.Framework;

namespace CpuSchedulerUI.Tests
{
    [TestFixture]
    public class EdgeCaseTests
    {
        [Test]
        public void AllProcessesArriveAtZero_SRTF_ShouldCompleteCorrectly()
        {
            var processes = new List<Process>
            {
                new Process { Id = 1, ArrivalTime = 0, BurstTime = 5, Priority = 1 },
                new Process { Id = 2, ArrivalTime = 0, BurstTime = 3, Priority = 2 },
                new Process { Id = 3, ArrivalTime = 0, BurstTime = 8, Priority = 3 }
            };

            var results = SrtfScheduler.Run(processes);

            Assert.That(results, Has.Count.EqualTo(3));
            Assert.That(results.TrueForAll(p => p.CompletionTime >= p.StartTime));
        }

        [Test]
        public void IdenticalBurstTimes_MLFQ_ShouldCompleteCorrectly()
        {
            var processes = new List<Process>
            {
                new Process { Id = 1, ArrivalTime = 0, BurstTime = 5, Priority = 1 },
                new Process { Id = 2, ArrivalTime = 1, BurstTime = 5, Priority = 2 },
                new Process { Id = 3, ArrivalTime = 2, BurstTime = 5, Priority = 3 }
            };

            var results = MlfqScheduler.Run(processes);

            Assert.That(results, Has.Count.EqualTo(3));
            Assert.That(results.TrueForAll(p => p.CompletionTime >= p.StartTime));
        }

        [Test]
        public void LongVsShortBursts_SRTF_ShouldPrioritizeShorter()
        {
            var processes = new List<Process>
            {
                new Process { Id = 1, ArrivalTime = 0, BurstTime = 20, Priority = 1 },
                new Process { Id = 2, ArrivalTime = 2, BurstTime = 2, Priority = 2 },
                new Process { Id = 3, ArrivalTime = 4, BurstTime = 1, Priority = 3 }
            };

            var results = SrtfScheduler.Run(processes);

            Assert.That(results, Has.Count.EqualTo(3));

            var p1 = results.Find(p => p.Id == 1);
            var p3 = results.Find(p => p.Id == 3);

            Assert.That(p1, Is.Not.Null);
            Assert.That(p3, Is.Not.Null);
            Assert.That(p3!.StartTime, Is.LessThan(p1!.CompletionTime));
        }

        [Test]
        public void SkewedPriorities_MLFQ_ShouldHandleFairly()
        {
            var processes = new List<Process>
            {
                new Process { Id = 1, ArrivalTime = 0, BurstTime = 4, Priority = 5 },
                new Process { Id = 2, ArrivalTime = 1, BurstTime = 6, Priority = 1 },
                new Process { Id = 3, ArrivalTime = 2, BurstTime = 3, Priority = 3 }
            };

            var results = MlfqScheduler.Run(processes);

            Assert.That(results, Has.Count.EqualTo(3));
            Assert.That(results.TrueForAll(p => p.CompletionTime >= p.StartTime));
        }
    }
}


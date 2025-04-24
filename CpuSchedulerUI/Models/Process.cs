namespace CpuSchedulerUI.Models
{
    public class Process
    {
        public int Id { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int RemainingTime { get; set; }
        public int CompletionTime { get; set; }
        public int Priority { get; set; }
        public int StartTime { get; set; } = -1;

        public int ResponseTime => StartTime - ArrivalTime;
        public int TurnaroundTime => CompletionTime - ArrivalTime;
        public int WaitingTime => TurnaroundTime - BurstTime;

        public Process Clone() => (Process)this.MemberwiseClone();
    }
}
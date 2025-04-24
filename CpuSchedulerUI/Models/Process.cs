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

        public int ResponseTime => (StartTime >= 0 && ArrivalTime >= 0) ? StartTime - ArrivalTime : 0;
        public int TurnaroundTime => (CompletionTime >= ArrivalTime) ? CompletionTime - ArrivalTime : 0;
        public int WaitingTime => TurnaroundTime - BurstTime;

        public Process Clone() => (Process)this.MemberwiseClone();
    }
}
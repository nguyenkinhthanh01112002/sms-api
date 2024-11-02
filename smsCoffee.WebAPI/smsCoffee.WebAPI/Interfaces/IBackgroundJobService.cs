namespace smsCoffee.WebAPI.Interfaces
{
    public interface IBackgroundJobService
    {
        public void InitializeRecurringJobs();
        public string EnqueueEmailJob(string to, string subject, string body);
    }
}

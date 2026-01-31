namespace AutoMechanic.API.Hangfire
{
    public class HangfireTestJob : IHangfireTestJob
    {
        public bool TestJob()
        {
            System.Diagnostics.Debug.WriteLine("****************** In TestJob");
            return true;
        }
    }
}

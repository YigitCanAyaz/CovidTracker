namespace CovidTracker.Models
{
    public class CovidTracker
    {
        public CovidTracker()
        {
            Counts = new List<int>();
        }

        public string CovidDate { get; set; }

        public List<int> Counts { get; set; }
    }
}

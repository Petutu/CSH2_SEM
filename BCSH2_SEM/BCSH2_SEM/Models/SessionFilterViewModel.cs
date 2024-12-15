namespace BCSH2_SEM.Models
{
    public class SessionFilterViewModel
    {
        public string SearchMember { get; set; }
        public string SearchTrainer { get; set; }
        public string SearchSessionType { get; set; }
        public IEnumerable<Session> Sessions { get; set; }
        public IEnumerable<string> SessionTypes { get; set; }
    }

}

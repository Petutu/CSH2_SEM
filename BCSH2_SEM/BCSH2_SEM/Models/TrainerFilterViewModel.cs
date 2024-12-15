namespace BCSH2_SEM.Models
{
    public class TrainerFilterViewModel
    {
        public string SearchFirstName { get; set; }
        public string SearchLastName { get; set; }
        public string SearchSpecialization { get; set; }
        public IEnumerable<Trainer> Trainers { get; set; }
        public IEnumerable<string> Specializations { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2_SEM.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }

        [NotMapped]
        public ICollection<Session> Sessions { get; set; }
    }
}

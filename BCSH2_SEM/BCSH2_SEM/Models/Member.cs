using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2_SEM.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MembershipType { get; set; }
        public DateTime JoinDate { get; set; }

        [NotMapped]
        public ICollection<Session> Sessions { get; set; }
    }
}

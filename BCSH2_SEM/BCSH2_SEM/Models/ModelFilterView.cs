namespace BCSH2_SEM.Models
{
    public class MemberFilterViewModel
    {
        public string SearchFirstName { get; set; }
        public string SearchLastName { get; set; }
        public string SearchMembershipType { get; set; }
        public IEnumerable<Member> Members { get; set; }
    }
}

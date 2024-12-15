using System.ComponentModel.DataAnnotations.Schema;

namespace BCSH2_SEM.Models
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        public int Duration { get; set; } 
        public string SessionType { get; set; }

       
        public int MemberId { get; set; }
      
        public Member Member { get; set; }

        public int? TrainerId { get; set; }
        
        public Trainer Trainer { get; set; }
    }
}

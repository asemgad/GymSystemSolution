using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.Models
{
    public class Booking : BaseU
    {
        public Member Member { get; set; }
        public int MemberId { get; set; }

        public Session Session { get; set; }
        public int SessionId { get; set; }

        // BookingDate= CreatedAt

        public bool IsAttendee { get; set; }


    }
}

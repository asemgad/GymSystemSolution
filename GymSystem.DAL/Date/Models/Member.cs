using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        // Created at ==  joinDate

        #region Relationships

        public HealthRecord HealthRecord { get; set; } = default!;

        public ICollection<MemberShip> MemberShips { get; set; }
        public ICollection<Booking> Bookings { get; set; }


        #endregion
    }
}

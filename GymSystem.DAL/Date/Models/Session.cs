using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.Models
{
    public class Session : BaseU
    {
        public string Description { get; set; } = default!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region Relations

        public Trainer Trainer { get; set; }
        public int TrainerId { get; set; }

        public Category Category { get; set; }

        public int CategoryId { get; set; }

        public ICollection<Booking> Bookings { get; set; }

        #endregion
    }
}

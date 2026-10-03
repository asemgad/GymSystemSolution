using GymSystem.DAL.Date.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.Models
{
    public class Trainer : GymUser
    {
        // HireDate = createdAt

        public Speciality Speciality { get; set; }

        #region Relations
        public ICollection<Session> Sessions { get; set; } = default!;

        #endregion

    }
}

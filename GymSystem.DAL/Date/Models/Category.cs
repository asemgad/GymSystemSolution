using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Date.Models
{
    public class Category : BaseU

    {
        public string CategoryName { get; set; }

        #region Relations

        public ICollection<Session> Sessions { get; set; } = default!;
        #endregion
    }
}

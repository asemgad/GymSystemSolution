using GymSystem.DAL.Date.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.TrainersViewModel
{
    public class TrainerViewModel
    {
        public int  Id { get; set; }
        public string  Name { get; set; }
        public string  Email { get; set; }
        public string  Phone { get; set; }
        public string  Adress { get; set; }

        public DateOnly DateOfBirth { get; set; }
        public Speciality Specialization { get; set; }


    }
}

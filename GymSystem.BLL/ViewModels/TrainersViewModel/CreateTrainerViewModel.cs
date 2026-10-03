using GymSystem.DAL.Date.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.ViewModels.TrainersViewModel
{
    public class CreateTrainerViewModel
    {
        #region Personal Information

        [Required(ErrorMessage = "Name Is Required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone Is Required")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",
            ErrorMessage = "Phone must be a valid Egyptian number starting with 010, 011, 012, or 015 followed by 8 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Date Of Birth Is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }

        #endregion

        #region Address Information

        [Required(ErrorMessage = "Building Number Is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number must be a positive number")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street Is Required")]
        [StringLength(30, ErrorMessage = "Street cannot exceed 30 characters")]
        public string Street { get; set; }

        [Required(ErrorMessage = "City Is Required")]
        [StringLength(30, ErrorMessage = "City cannot exceed 30 characters")]
        public string City { get; set; }

        #endregion

        #region Professional Information

        [Required(ErrorMessage = "Specialty Is Required")]
        public Speciality Speciality { get; set; }

        #endregion
    }
}

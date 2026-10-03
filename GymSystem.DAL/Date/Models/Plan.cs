namespace GymSystem.DAL.Date.Models
{
    public class Plan : BaseU
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public int DurationDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        #region Relations
        public ICollection<MemberShip> MemberShips { get; set; }

        #endregion
    }
}

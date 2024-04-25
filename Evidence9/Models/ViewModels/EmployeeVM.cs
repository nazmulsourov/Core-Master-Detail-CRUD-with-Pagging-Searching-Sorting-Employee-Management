namespace Evidence9.Models.ViewModels
{
    public class EmployeeVM
    {
        public EmployeeVM()
        {
            this.SkillList = new List<int>();
        }
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; } = null!;

        public DateTime JoinDate { get; set; }

        public decimal Salary { get; set; }

        public string? Picture { get; set; }
        public IFormFile? PictureFile { get; set; }

        public bool ActiverStatus { get; set; }
        public List<int> SkillList { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DuAn_Api.Models
{
    public class ProgramModel
    {
        [Key]
        public int programId { get; set; }

        [Required]
        public String programName { get; set; } = "";

        [Required]
        public DateTime startDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime endDate { get; set; } = DateTime.Now;
    }
}

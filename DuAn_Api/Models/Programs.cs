using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DuAn_Api.Models
{
    public class ProgramModel
    {
        [Key]
        public int programId { get; set; }

        [Required]
        public string programName { get; set; } = "";

        [Required]
        public DateTime startDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime endDate { get; set; } = DateTime.Now;

        public List<ImageModel> Images { get; set; } = new List<ImageModel>();
    }
}

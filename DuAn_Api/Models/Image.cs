using System.ComponentModel.DataAnnotations;

namespace DuAn_Api.Models
{
    public class ImageModel
    {
        [Key]
        public int ImageId { get; set; }

        [Required]
        public int ProgramId { get; set; }

        [Required]
        public string ImgLink { get; set; } = "";

        [Required]
        public string ImageName { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public string ImageDes { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HRRecruitmentSystem.Models
{
    public class JobApplicationViewModel
    {
        public int JobOfferId { get; set; }
        public string? JobTitle { get; set; }

        [Required(ErrorMessage = "Please upload your CV.")]
        [Display(Name = "Upload CV (PDF only)")]
        public IFormFile CvFile { get; set; }
    }
}
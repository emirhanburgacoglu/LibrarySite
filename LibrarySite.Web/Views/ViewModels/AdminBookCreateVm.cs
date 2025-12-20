using System.ComponentModel.DataAnnotations;

namespace LibrarySite.Web.ViewModels
{
    public class AdminBookCreateVm
    {
        [Required]
        [StringLength(80)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Author { get; set; } = string.Empty;
    }
}

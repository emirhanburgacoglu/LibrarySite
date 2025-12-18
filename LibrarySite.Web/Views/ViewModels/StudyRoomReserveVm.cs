using System;
using System.ComponentModel.DataAnnotations;

namespace LibrarySite.Web.ViewModels
{
    public class StudyRoomReserveVm
    {
        public int StudyRoomId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}

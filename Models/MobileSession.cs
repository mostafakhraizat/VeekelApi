using System.ComponentModel.DataAnnotations.Schema;

namespace VeekelApi.Models
{
    public class MobileSession
    {
        public int Id{ get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string FcmToken { get; set; }
        public string DeviceType { get; set; }
    }
}

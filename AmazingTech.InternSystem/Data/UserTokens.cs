using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data
{
    [Table("UserTokens")]
    public class UserTokens
    {
        [Key]
        public string Id { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}

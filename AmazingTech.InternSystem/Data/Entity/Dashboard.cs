using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmazingTech.InternSystem.Data.Entity
{
    [Table("Dashboard")]
    public class Dashboard
    {
        [Key]
        public string Id { get; set; }

        public int ReceivedCV { get; set; }

        public int Interviewed { get; set; }

        public int Passed { get; set; }

        public int Interning { get; set; }

        public int Interned { get; set; }

    }
}

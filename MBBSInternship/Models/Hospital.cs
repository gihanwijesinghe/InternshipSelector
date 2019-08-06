using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TotalSlots { get; set; }

        [ForeignKey("DistrictId")]
        public District District { get; set; }
        public int DistrictId { get; set; }
    }
}

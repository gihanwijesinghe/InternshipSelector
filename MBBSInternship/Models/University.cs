using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Models
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("DistrictId")]
        public District District { get; set; }
        public int DistrictId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Models
{
    public class PersonHospitalRelationship
    {
        public int Id { get; set; }
        public int PreferenceNumber { get; set; }

        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public int PersonId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital Hospital { get; set; }
        public int HospitalId { get; set; }
    }
}

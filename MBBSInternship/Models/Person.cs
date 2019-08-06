using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("UniversityId")]
        public University University { get; set; }
        public int UniversityId { get; set; }

        public int Rank { get; set; }
        public string NIC { get; set; }
        public Gender Gender { get; set; }

        public ICollection<PersonHospitalRelationship> HospitalChoices { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1
    }
}

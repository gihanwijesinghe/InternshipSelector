using MBBSInternship.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.ViewModels.Users
{
    public class PersonViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public University University { get; set; }
        public int UniversityId { get; set; }

        public int Rank { get; set; }
        public string NIC { get; set; }
        public Gender Gender { get; set; }
    }
}

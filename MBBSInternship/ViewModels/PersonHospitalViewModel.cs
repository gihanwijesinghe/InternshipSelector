using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.ViewModels
{
    public class PersonHospitalViewModel
    {
        public int Id { get; set; }
        public int PreferenceNumber { get; set; }
        public string HospitalName { get; set; }
        public string DistrictName { get; set; }
        public int TotalSlots { get; set; }
    }
}

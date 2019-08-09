using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.ViewModels
{
    public class HospitalViewModel
    {
        public int Id { get; set; }
        public int RemainingSlots { get; set; }
        public int TotalSlots { get; set; }
    }
}

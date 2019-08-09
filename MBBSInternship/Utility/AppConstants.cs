using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MBBSInternship.Utility
{
    public static class AppConstants
    {
        public static string LocalConnectionString = @"Data Source=CL-GIHANW\SQLEXPRESS;Initial Catalog=InternshipTest;Trusted_Connection=True;";
        public static string GCloudConnectionString = @"Data Source=34.93.123.97;Initial Catalog=InternshipTest;Integrated Security=False;User ID=TestUser;Password=Gihan@123;MultipleActiveResultSets=True";
    }
}

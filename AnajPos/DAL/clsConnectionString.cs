using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AnajPos.DAL
{
    class clsConnectionString
    {
        public static string cs = ConfigurationManager.ConnectionStrings["dbcs"].ConnectionString;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;

namespace Slickflow.Designer
{
    public class DBConfig
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["WfDBConnectionString"].ToString();
    }
}

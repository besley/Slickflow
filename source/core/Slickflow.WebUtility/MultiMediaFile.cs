using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slickflow.WebUtility
{
    public class MultiMediaFile
    {
        public string Name { get; set; }    
        public MultiMediaTypeEnum MeidaType {  get; set; }
        public string base64Content { get; set; }
    }
}

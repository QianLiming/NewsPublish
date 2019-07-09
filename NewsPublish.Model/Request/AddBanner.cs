using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Request
{
    public class AddBanner
    {
        public string Image { set; get; }
        public string Url { set; get; }   
        public string Remark { set; get; }
    }
}

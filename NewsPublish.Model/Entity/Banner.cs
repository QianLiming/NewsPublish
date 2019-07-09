using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class Banner
    {
        public int Id { set; get; }
        public string  Image { set; get; }
        public string Url { set; get; }
        public DateTime AddTime { set; get; }
        public string Remark { set; get; }    
    }
}

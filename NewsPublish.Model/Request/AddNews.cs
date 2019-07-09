using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Request
{
    public class AddNews
    {
        
        public int NewsClassifyId { set; get; }
        public string Title { set; get; }
        public string Image { set; get; }
        public string Contents { set; get; }    
        public string Remark { set; get; }      
    }
}

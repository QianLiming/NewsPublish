using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class NewsComment
    {
        public int Id { set; get; }
        public int NewsId { set; get; }       
        public string Contents { set; get; }   
        public DateTime AddTime { set; get; }
        public string Remark { set; get; }
        public virtual News News { set; get; }
    }
}

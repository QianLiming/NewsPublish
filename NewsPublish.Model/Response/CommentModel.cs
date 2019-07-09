using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Response
{
    public class CommentModel
    {
        public int Id { set; get; }
        public string NewsName { set; get; }
        public string Contents { set; get; }
        public DateTime AddTime { set; get; }
        public string Remark { set; get; }     
        public string Floor { set; get; }
    }
}
 
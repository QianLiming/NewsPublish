using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Request
{
    public class AddComment
    {
        public int NewsId { set; get; }
        public string Contents { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Response
{
    public class NewsModel
    {
        public int Id { set; get; }
        public string ClassifyName{ set; get; }
        public string Title { set; get; }
        public string Image { set; get; }
        public string Contents { set; get; }
        public string PublishDate { set; get; }
        public string Remark { set; get; }
        public int CommentCount { set; get; }
    }
}

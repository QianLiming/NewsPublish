using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class News
    {
        public News()
        {
            this.NewsComment = new HashSet<NewsComment>();
        }
        public int Id { set; get; }
        public int NewsClassifyId { set; get; }
        public string Title { set; get; }
        public string Image { set; get; }
        public string Contents { set; get; }
        public DateTime PublishDate { set; get; }
        public string Remark { set; get; }
        public virtual NewsClassify NewsClassify { set; get; }
        public virtual ICollection<NewsComment> NewsComment { set; get; }
    }
}

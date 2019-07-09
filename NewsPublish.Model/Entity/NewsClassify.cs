using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Entity
{
    public class NewsClassify
    {
        public NewsClassify()
        {
            this.News = new HashSet<News>();
        }
        public int Id { set; get; }
        public string Name { set; get; }
        public int Sort { set; get; }
        public string Remark { set; get; }
        public virtual ICollection<News> News { set; get; }
    }
}

using NewsPublish.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace NewsPublish.Model.Response
{
    public class NewsClassifyModel
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int Sort { set; get; }
        public string Remark { set; get; }       
    }
}

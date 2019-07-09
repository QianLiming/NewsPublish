using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NewsPublish.Model.Request
{
    public class AddNewsClassify
    {
        public string Name { set; get; }
        public int Sort { set; get; }
        public string Remark { set; get; }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NewsPublish.Model.Request
{
    public class EditorNewsClassify
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int Sort { set; get; }
        public string Remark { set; get; }
    }
}
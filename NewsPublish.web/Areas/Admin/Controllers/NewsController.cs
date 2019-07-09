using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Services;

namespace NewsPublish.web.Areas.Admin.Controllers
{
     
    [Area("Admin")]
    public class NewsController : Controller
    {
        private NewsService _newsService;
        private IHostingEnvironment _host;
        public NewsController(NewsService newsService, IHostingEnvironment host)
        {
            this._newsService = newsService;
            _host = host;
        }
        #region news
        // GET: News
        public ActionResult Index()
        {
            var newsClassifys = _newsService.GetNewsClassifyList();
            return View(newsClassifys);
        }

        [HttpGet]
        public JsonResult GetNews(int pageIndex, int pageSize, int classifyId, string keyword)
        {
            List<Expression<Func<News, bool>>> wheres = new List<Expression<Func<News, bool>>>();
            if (classifyId > 0)
                wheres.Add(a=>a.NewsClassifyId==classifyId);
            if (!string.IsNullOrEmpty(keyword))
                wheres.Add(a => a.Title.Contains(keyword));
            int total = 0;
            var news = _newsService.NewsPageQuery(pageSize, pageIndex, out total, wheres);
            return Json(new { total = total, data = news.data });
        }
    
        public ActionResult NewsAdd()
        {
            var newsClassifys = _newsService.GetNewsClassifyList();
            return View(newsClassifys);
        }
        [HttpPost]
        public async Task<JsonResult> AddNews(AddNews news, IFormCollection collection)
        {
            if (news.NewsClassifyId<0||string.IsNullOrEmpty(news.Title)||string.IsNullOrEmpty(news.Contents))
            {
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "参数有误"
                });
            }
            var files = collection.Files;
            if (files.Count > 0)
            {
                var webRootPath = _host.WebRootPath;
                string relativeDirPath = "\\NewsPic";
                string absolutePath = webRootPath + relativeDirPath;

                string[] fileTypes = new string[] { ".gif", ".jpg", ".jpeg", ".png", ".bmp" };
                string extension = Path.GetExtension(files[0].FileName);
                if (fileTypes.Contains(extension.ToLower()))
                {
                    if (!Directory.Exists(absolutePath)) Directory.CreateDirectory(absolutePath);
                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    var filePath = absolutePath + "\\" + fileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await files[0].CopyToAsync(stream);
                    }
                    news.Image = "/NewsPic/" + fileName;
                    return Json(_newsService.AddNews(news));
                }
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "图片格式有误"
                });
            }
            return Json(new ResponseModel()
            {
                code = 0,
                result = "请上传图片文件"
            });
        }

        [HttpPost]
        public JsonResult DelNews(int Id)
        {
            if (Id<0)
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "参数有误"
                });
            return Json(_newsService.DeloneNews(Id));
        }
        #endregion
        #region NewsClassify
        public ActionResult NewsClassify()
        {
            var newsClassifys = _newsService.GetNewsClassifyList();
            return View(newsClassifys);
        }
        public ActionResult NewsclassifyAdd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewsclassifyAdd(AddNewsClassify addNewsClassify)
        {
            if (string.IsNullOrEmpty(addNewsClassify.Name))
                return Json(new ResponseModel() { code = 0, result = "请输入新闻类别名称" });
            return Json(_newsService.AddNewsClassify(addNewsClassify));
        }
        public ActionResult NewsclassifyEdit(int id)
        {
            return View(_newsService.GetOneNewsClassify(id));
        }
        [HttpPost]
        public ActionResult NewsclassifyEdit(EditorNewsClassify editorNewsClassify)
        {
            if (string.IsNullOrEmpty(editorNewsClassify.Name))
                return Json(new ResponseModel() { code = 0, result = "请输入新闻类别名称" });
            return Json(_newsService.EditorNewsClassify(editorNewsClassify));
        }
        #endregion       
    }
}
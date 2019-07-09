using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using NewsPublish.Services;

namespace NewsPublish.web.Controllers
{
    public class NewsController : Controller
    {
       
        private NewsService _newsService;
        private CommentService _commentService;
        public NewsController(NewsService newsService, CommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }
        // GET: News
        public ActionResult Classify(int Id)
        {
            if (Id < 0)
                Response.Redirect("/Home/Index");
            ViewData["NewsList"] = new ResponseModel();
            ViewData["ClassifyName"] = "首页";
            ViewData["NewCommentNews"] = new ResponseModel();
            var classify = _newsService.GetOneNewsClassify(Id);
            if (classify.code==0)
                 Response.Redirect("/Home/Index");
            else
            {
                ViewData["ClassifyName"] = classify.data.Name;
                var newsList = _newsService.GetNewsList(a => a.NewsClassifyId == Id, 6);
                ViewData["NewsList"] = newsList;
                var newCommentNews = _newsService.GetNewCommentNewsList(a => a.NewsClassifyId == Id, 5);
                ViewData["NewCommentNews"] = newCommentNews;
                ViewData["Title"] = classify.data.Name;
            }            
            return View(_newsService.GetNewsClassifyList());
        }

        // GET: News/Details/5
        public ActionResult Detail(int Id)
        {
            ViewData["Title"] = "详情";
            if (Id < 0)
                Response.Redirect("/Home/Index");
            var news = _newsService.GetoneNews(Id);            
            ViewData["News"] = new ResponseModel();
            ViewData["RecommentNews"] = new ResponseModel();
            ViewData["Comment"] = new ResponseModel();
            if (news.code == 0)
                Response.Redirect("/Home/Index");
            else {
                ViewData["News"] = news;
                var recommentNews = _newsService.GetRecommendNewsList(Id);
                ViewData["RecommentNews"] = recommentNews;
                var comments = _commentService.GetCommentList(a=>a.NewsId==Id);
                ViewData["Comments"] = comments;
                ViewData["Title"] = news.data.Title+"详情";
            }            
            
            return View(_newsService.GetNewsClassifyList());
        }

        [HttpPost]
        public JsonResult AddComment(AddComment addComment)
        {
            if (addComment.NewsId <= 0)
            {
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "新闻不存在"
                });
            }
            if (string.IsNullOrEmpty(addComment.Contents))
            {
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "内容不能为空"
                });
            }
            return Json(_commentService.AddComment(addComment));
        }
    }
}
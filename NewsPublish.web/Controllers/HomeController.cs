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
    public class HomeController : Controller
    {
        private BannerService _bannerService;
        private NewsService _newsService;

        public HomeController(BannerService bannerService, NewsService newsService)
        {
            _bannerService = bannerService;
            _newsService = newsService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "首页";
            return View(_newsService.GetNewsClassifyList());
        }
        [HttpGet]
        public JsonResult GetBanner()
        {
            return Json(_bannerService.GetBannerList());
        }
        [HttpGet]
        public JsonResult GetTotalNews()
        {
            return Json(_newsService.GetNewsCount(a=>true));
        }
        [HttpGet]
        public JsonResult GetHomeNews()
        {
            return Json(_newsService.GetNewsList(a=>true,6));
        }
        [HttpGet]
        public JsonResult GetNewCommentNews()
        {
            return Json(_newsService.GetNewCommentNewsList(a => true, 5));
        }
        [HttpGet]
        public JsonResult SearchOneNews(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "关键字不能为空"
                });
            }
            return Json(_newsService.GetSearchNewsList(a => a.Title.Contains(keyword)));
        }

        public IActionResult Wrong()
        {
            ViewData["Title"] = "404";
            return View(_newsService.GetNewsClassifyList());
        }
    }
}

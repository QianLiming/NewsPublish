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
    public class CommentController : Controller
    {
        private CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }
        // GET: Comment
        public ActionResult Index()
        {
            return View(_commentService.GetCommentList(a=>true));
        }
        [HttpPost]
        public JsonResult DelComment(int Id)
        {
            if (Id < 0)
                return Json(new ResponseModel()
                {
                    code = 0,
                    result = "参数有误"
                });
            return Json(_commentService.DeleteComment(Id));
        }     
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using System.Linq.Expressions;
using System.Linq;

namespace NewsPublish.Services
{
    public class CommentService
    {
        private Db _db;
        private NewsService _newsService;
        public CommentService(Db db, NewsService newsService)
        {
            this._db = db;
            this._newsService = newsService;
        }
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public ResponseModel AddComment(AddComment comment)
        {
            var news = _newsService.GetoneNews(comment.NewsId);
            if (news.code==0)
                return new ResponseModel() { code = 0, result = "该新闻不存在" };
            var com = new NewsComment()
            {
                AddTime=DateTime.Now,
                NewsId=comment.NewsId,
                Contents = comment.Contents             
            };
            _db.NewsComment.Add(com);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel() {
                    code = 200,
                    result = "评论添加成功",
                    data=new {
                        contents=comment.Contents,
                        floor="#"+(news.data.CommentCount+1),
                        addTime=DateTime.Now 
                    }
                };
            return new ResponseModel() { code = 0, result = "评论添加失败" };
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ResponseModel DeleteComment(int Id)
        {
            var comment = _db.NewsComment.Find(Id);
            if (comment == null)
                return new ResponseModel() { code = 0, result = "该评论不存在" };
            _db.NewsComment.Remove(comment);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel() { code = 200, result = "评论删除成功" };
            return new ResponseModel() { code = 0, result = "评论删除失败" };
        }
        /// <summary>
        ///  获取评论集合
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ResponseModel GetCommentList(Expression<Func<NewsComment, bool>> where)
        {
            var comments = _db.NewsComment.Include("News").Where(where).OrderBy(a => a.AddTime).ToList();
            var response = new ResponseModel();
            response.code = 200;
            response.result = "评论获取成功";
            response.data = new List<CommentModel>();
            int floor = 1;
            foreach (var comment in comments)
            {
                response.data.Add(new CommentModel() {
                    Id=comment.Id,
                    NewsName = comment.News.Title,
                    Contents = comment.Contents,
                    AddTime = comment.AddTime,
                    Remark = comment.Remark,
                    Floor = "#"+floor,

                });
                floor++;
            }
            response.data.Reverse();
            return response;
        }
    }
}

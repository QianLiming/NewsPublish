using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NewsPublish.Services
{
    public class NewsService
    {
        private Db _db;
        public NewsService(Db db)
        {
            this._db = db;
        }
        #region NewsClassify
        /// <summary>
        /// 添加新闻类别
        /// </summary>
        /// <param name="newsClassify"></param>
        /// <returns></returns>
        public ResponseModel AddNewsClassify(AddNewsClassify newsClassify)
        {
            var exit = _db.NewsClassify.FirstOrDefault(c => c.Name == newsClassify.Name) != null;
            if (exit)
                return new ResponseModel() { code = 0, result = "该类别已存在" };
            var classify = new NewsClassify
            {
                Name = newsClassify.Name,
                Remark = newsClassify.Remark,
                Sort = newsClassify.Sort,

            };
            _db.NewsClassify.Add(classify);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel() { code = 200, result = "新闻类别添加成功" };
            return new ResponseModel() { code = 0, result = "新闻类别添加失败" };
        }
        /// <summary>
        /// 获得一个新闻类别
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel GetOneNewsClassify(int id)
        {
            var classify = _db.NewsClassify.Find(id);
            if (classify == null)
                return new ResponseModel() { code = 0, result = "该类别不存在" };
            return new ResponseModel()
            {
                code = 200,
                result = "类别获取成功",
                data = new NewsClassifyModel()
                {
                    Id = classify.Id,
                    Sort = classify.Sort,
                    Name = classify.Name,
                    Remark = classify.Remark
                }
            };
        }
        /// <summary>
        /// 获得一个新闻类别
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private NewsClassify GetOneNewsClassify(Expression<Func<NewsClassify, bool>> where)
        {
            return _db.NewsClassify.FirstOrDefault(where);
        }

        /// <summary>
        /// 编辑新闻类别
        /// </summary>
        /// <param name="newsClassify"></param>
        /// <returns></returns>
        public ResponseModel EditorNewsClassify(EditorNewsClassify newsClassify)
        {
            var classify = GetOneNewsClassify(a => (a.Id == newsClassify.Id));
            if (classify == null)
                return new ResponseModel() { code = 0, result = "该类别不存在" };
            classify.Name = newsClassify.Name;
            classify.Sort = newsClassify.Sort;
            classify.Remark = newsClassify.Remark;
            _db.NewsClassify.Update(classify);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel() { code = 200, result = "新闻类别编辑成功" };
            return new ResponseModel() { code = 0, result = "新闻类别编辑 失败" };
        }
        /// <summary>
        /// 获取新闻类别的列表
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetNewsClassifyList()
        {
            var classifys = _db.NewsClassify.OrderByDescending(a => a.Sort).ToList();
            var response = new ResponseModel()
            {
                code = 200,
                result = "新闻类别列表查询成功"
            };
            response.data = new List<NewsClassifyModel>();
            foreach (var classify in classifys)
            {
                response.data.Add(new NewsClassifyModel()
                {
                    Id = classify.Id,
                    Sort = classify.Sort,
                    Name = classify.Name,
                    Remark = classify.Remark
                });
            }
            return response;
        }
        #endregion
        #region News
        public ResponseModel AddNews(AddNews news)
        {
            var classify = GetOneNewsClassify(a => (a.Id == news.NewsClassifyId));
            if (classify == null)
                return new ResponseModel() { code = 0, result = "新闻类别不存在" };
            var n = new News()
            {
                Title = news.Title,
                Image = news.Image,
                NewsClassifyId = news.NewsClassifyId,
                Contents = news.Contents,
                Remark = news.Remark,
                PublishDate = DateTime.Now,
            };
            _db.News.Add(n);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel() { code = 200, result = "新闻添加成功" };
            return new ResponseModel() { code = 0, result = "新闻添加失败" };
        }
        /// <summary>
        /// 获取一个新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel GetoneNews(int id)
        {
            var news = _db.News.Include("NewsClassify").Include("NewsComment").FirstOrDefault(a => a.Id == id);
            if (news == null)
                return new ResponseModel() { code = 0, result = "该新闻不存在" };
            return new ResponseModel()
            {
                code = 200,
                result = "新闻获取成功",
                data = new NewsModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Image = news.Image,
                    ClassifyName = news.NewsClassify.Name,
                    Contents = news.Contents,
                    Remark = news.Remark,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count()
                }
            };
        }
        /// <summary>
        /// 删除一个新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel DeloneNews(int id)
        {
            var news = _db.News.FirstOrDefault(a => a.Id == id);
            if (news == null)
                return new ResponseModel() { code = 0, result = "该新闻不存在" };
            _db.Remove(news);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel() { code = 200, result = "新闻删除成功" };
            return new ResponseModel() { code = 0, result = "新闻删除失败" };
        }
        /// <summary>
        /// /分页查询新闻
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public ResponseModel NewsPageQuery(int pageSize, int pageIndex, out int total, List<Expression<Func<News, bool>>> where)
        {
            var list = _db.News.Include("NewsClassify").Include("NewsComment");
            foreach (var item in where)
            {
                list = list.Where(item);
            }
            total = list.Count();
            var pageData = list.OrderByDescending(c => c.PublishDate).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            var response = new ResponseModel()
            {
                code = 200,
                result = "分页新闻获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var news in pageData)
            {
                response.data.Add(new NewsModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Image = news.Image,
                    ClassifyName = news.NewsClassify.Name,
                    Contents = news.Contents.Length > 50 ? news.Contents.Substring(0, 50)+"..." : news.Contents,
                    Remark = news.Remark,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count()
                });
            }
            return response;
        }
        /// <summary>
        /// 查询新闻列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ResponseModel GetNewsList(Expression<Func<News, bool>> where, int topCount)
        {
            var list = _db.News.Include("NewsClassify").Include("NewsComment").Where(where).OrderByDescending(a => a.PublishDate).Take(topCount).ToList();
            var response = new ResponseModel()
            {
                code = 200,
                result = "新闻列表获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var news in list)
            {
                response.data.Add(new NewsModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Image = news.Image,
                    ClassifyName = news.NewsClassify.Name,
                    Contents = news.Contents.Length > 50 ? news.Contents.Substring(0, 50) + "..." : news.Contents,
                    Remark = news.Remark,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count()
                });
            }
            return response;
        }
        /// <summary>
        /// 获取最新评论的文章
        /// </summary>
        /// <param name="where"></param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        public ResponseModel GetNewCommentNewsList(Expression<Func<News, bool>> where, int topCount)
        {
            var newsIds = _db.NewsComment.OrderByDescending(a => a.AddTime).GroupBy(a => a.NewsId).Select(a => a.Key).Take(topCount);
            var list = _db.News.Include("NewsClassify").Include("NewsComment").Where(where).Where(a => newsIds.Contains(a.Id)).OrderByDescending(a => a.PublishDate).ToList();
            var response = new ResponseModel()
            {
                code = 200,
                result = "最新评论新闻获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var news in list)
            {
                response.data.Add(new NewsModel()
                {
                    Id = news.Id,
                    Title = news.Title,
                    Image = news.Image,
                    ClassifyName = news.NewsClassify.Name,
                    Contents = news.Contents.Length > 50 ? news.Contents.Substring(0, 50) : news.Contents,
                    Remark = news.Remark,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComment.Count()
                });
            }
            return response;
        }
        /// <summary>
        /// 搜索一个新闻
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ResponseModel GetSearchNewsList(Expression<Func<News, bool>> where)
        {
            var news = _db.News.Where(where).FirstOrDefault();
            if (news == null)
                return new ResponseModel() { code = 0, result = "该新闻搜索失败" };
            return new ResponseModel() { code = 200, result = "新闻搜索成功", data = news.Id };
        }
        /// <summary>
        /// 获取新闻的数量
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public ResponseModel GetNewsCount(Expression<Func<News, bool>> where)
        {
            var count = _db.News.Where(where).Count();
            return new ResponseModel() { code = 200, result = "新闻数量获取成功", data = count };
        }
        /// <summary>
        /// 获取新闻推荐列表
        /// </summary>
        /// <param name="newsId"></param>
        /// <returns></returns>
        public ResponseModel GetRecommendNewsList(int newsId)
        {
            var news = _db.News.FirstOrDefault(a => a.Id == newsId);
            if (news == null)
                return new ResponseModel() { code = 0, result = "该新闻不存在" };
            var newsList = _db.News.Include("NewsComment").Where(a => a.NewsClassifyId == news.NewsClassifyId && a.Id != news.Id).
               OrderByDescending(a => a.PublishDate).OrderByDescending(a => a.NewsComment.Count).Take(6);
            var response = new ResponseModel()
            {
                code = 200,
                result =  "推荐新闻获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var n in newsList)
            {
                response.data.Add(new NewsModel()
                {
                    Id = n.Id,
                    Title = n.Title,
                    Image = n.Image,
                    ClassifyName = n.NewsClassify.Name,
                    Contents = n.Contents.Length > 50 ? n.Contents.Substring(0, 50) : n.Contents,
                    Remark = n.Remark,
                    PublishDate = n.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = n.NewsComment.Count()
                });
            }
            return response;
        }
        #endregion
    }
}

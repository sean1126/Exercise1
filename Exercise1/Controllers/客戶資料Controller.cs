using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exercise1.Models;

namespace Exercise1.Controllers
{
    public class 客戶資料Controller : BaseController
    {

        public ActionResult GetCustDataFromView()
        {

            return View(repoCustView.All());
        }

        // Post: 客戶資料
        [HttpPost]
        public ActionResult Index(string searchStr)
        {
            // 如果使用者沒有輸入關鍵字，撈取全部資料，否則進行搜尋
            var data = searchStr == "" ?
                            repoCust.Where(p => p.是否已刪除 != true) :
                            repoCust.Where(p => p.是否已刪除 != true && p.客戶名稱.Contains(searchStr));
            return View(data);
        }



        // GET: 客戶資料
        public ActionResult Index()
        {
            var data = repoCust.All(false);
            ViewBag.類別Id = new SelectList(repoCustCategory.All(),"Id","類別名稱");
            return View(data);
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repoCust.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            var model = new 客戶資料() { 是否已刪除 = false };//Coding By David
            ViewBag.類別Id = new SelectList(repoCustCategory.All(), "Id", "類別名稱");
            return View(model);
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,類別Id")] 客戶資料 客戶資料)
        {

            if (ModelState.IsValid)
            {
                repoCust.Add(客戶資料);
                repoCust.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repoCust.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            ViewBag.類別Id = new SelectList(repoCustCategory.All(), "Id", "類別名稱",客戶資料.類別Id);
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,類別Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                #region 原程式碼，不使用Repository時的方法，有東西沒搞懂，先Mark起來 
                /*
                db.Entry(客戶資料).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

                */
                #endregion

                repoCust.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repoCust.UnitOfWork.Commit();

                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repoCust.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 delObj = repoCust.Find(id);
            repoCust.Delete(delObj);
            repoCust.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoCust.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }



        #region 嘗試用，暫時隱藏
        /*
        //public ActionResult SearchCustByAjaxAcitonLink(string searchStr)
        //{
        //    var data = searchStr == "" ?
        //                db.客戶資料.Where(p => p.是否已刪除 != true)
        //                : db.客戶資料.Where(p => p.是否已刪除 != true && p.客戶名稱.Contains(searchStr));
        //    return View("Index", data);
        //}
        [HttpPost]
        public ActionResult SearchCust(FormCollection form)
        {
            string searchStr = form["searchBox"];
            var data = searchStr == "" ?
                        db.客戶資料.Where(p => p.是否已刪除 != true)
                        : db.客戶資料.Where(p => p.是否已刪除 != true && p.客戶名稱.Contains(searchStr));
            
            return RedirectToAction("Index", data); 

        }
        public ActionResult SearchCustByActionLink(string keyWord1)
        {
            // 如果使用者沒有輸入關鍵字，撈取全部資料，否則進行搜尋
            var data = keyWord1 == "" ?
                            custRepo.Where(p => p.是否已刪除 != true) :
                            custRepo.Where(p => p.是否已刪除 != true && p.客戶名稱.Contains(keyWord1));

            #region 第一版作法，不使用Repsitoy
            //var data = keyWord1 == "" ?
            //            db.客戶資料.Where(p => p.是否已刪除 != true)
            //            : db.客戶資料.Where(p => p.是否已刪除 != true && p.客戶名稱.Contains(keyWord1));
            #endregion

            return View("Index", data);
        }
        */


        #endregion


    }
}

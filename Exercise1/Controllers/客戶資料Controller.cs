using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exercise1.Models;
using NPOI.HSSF.UserModel;
using System.IO;

namespace Exercise1.Controllers
{
    public class 客戶資料Controller : BaseController
    {

        public ActionResult GetCustDataFromView()
        {
            return View(repoCustView.All());
        }

        // GET: 客戶資料
        public ActionResult Index()
        {
            var data = repoCust.All(isContainDel:false);
            ViewBag.類別Id = GenerateCategorySelectList();
            return View(data);
        }


        // Post: 客戶資料
        [HttpPost]
        public ActionResult Index(string searchStr,int 類別Id)
        {
            var data = repoCust.SearchByCategory(searchStr, 類別Id);  // 如果使用者沒有輸入關鍵字，撈取全部資料，否則進行搜尋
            ViewBag.類別Id = GenerateCategorySelectList();
            return View(data);
        }

        private List<SelectListItem> GenerateCategorySelectList()
        {
            List<SelectListItem> sl = new List<SelectListItem>(new SelectList(repoCustCategory.All(), "Id", "類別名稱"));
            SelectListItem li = new SelectListItem();
            li.Value = "0";
            li.Text = "全部";
            sl.Insert(0, li);
            return sl;
        }




        public ActionResult Export(string searchStr, int 類別Id)
        {
            IQueryable<客戶資料> cust = repoCust.SearchByCategory(searchStr, 類別Id);
            MemoryStream ms = ExportDataToExcel(cust);
            return File(ms.ToArray(), "application/vnd.ms-excel", "客戶資料.xls");
        }

        private MemoryStream ExportDataToExcel(IQueryable<客戶資料> cust)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet("試算表");


            //設定表頭
            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("客戶類別");
            sheet.GetRow(0).CreateCell(1).SetCellValue("客戶名稱");
            sheet.GetRow(0).CreateCell(2).SetCellValue("統一編號");
            sheet.GetRow(0).CreateCell(3).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(4).SetCellValue("傳真");
            sheet.GetRow(0).CreateCell(5).SetCellValue("地址 ");
            sheet.GetRow(0).CreateCell(6).SetCellValue("Email");

            //匯出資料
            int count = 1;
            foreach (var item in cust)
            {
                sheet.CreateRow(count);
                sheet.GetRow(count).CreateCell(0).SetCellValue(item.客戶類別.類別名稱);
                sheet.GetRow(count).CreateCell(1).SetCellValue(item.客戶名稱);
                sheet.GetRow(count).CreateCell(2).SetCellValue(item.統一編號);
                sheet.GetRow(count).CreateCell(3).SetCellValue(item.電話);
                sheet.GetRow(count).CreateCell(4).SetCellValue(item.傳真);
                sheet.GetRow(count).CreateCell(5).SetCellValue(item.地址);
                sheet.GetRow(count).CreateCell(6).SetCellValue(item.Email);
                count++;
            }
            workbook.Write(ms);
            workbook = null;
            return ms;
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
            IQueryable<客戶聯絡人> custContact = repoContact.Where(p => p.客戶Id == id.Value);
//            var custContact = repoContact.Where(p => p.客戶Id == id.Value);

            ViewBag.類別Id = new SelectList(repoCustCategory.All(), "Id", "類別名稱",客戶資料.類別Id);
            ViewBag.custContact = custContact;
            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IList<BatchUpdateContact> data,
            [Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,類別Id")] 客戶資料 客戶資料)
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


                客戶聯絡人Repository repoContactForCommitWithCust = RepositoryHelper.Get客戶聯絡人Repository(repoCust.UnitOfWork);

                foreach (var item in data)
                {
                    var contact = repoContactForCommitWithCust.Find(item.Id);
                    contact.職稱 = item.職稱;
                    contact.手機 = item.手機;
                    contact.電話 = item.電話;
                }
                repoCust.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repoContactForCommitWithCust.UnitOfWork.Commit();
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

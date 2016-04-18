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
using PagedList;
using PagedList.Mvc;

namespace Exercise1.Controllers
{
    public class 客戶聯絡人Controller : BaseController
    {
        private 客戶資料Entities dbEntity = new 客戶資料Entities();


        #region 原始Method,Mark
        //// GET: 客戶聯絡人
        //public ActionResult Index()
        //{
        //    var data = repoContact.All(false);
        //    ViewBag.positionName = GeneratePositionSL();
        //    return View(data);
        //}

        #endregion

        // Get: 客戶聯絡人
        public ActionResult Index(string searchStr="",string positionName="all",string sortField="姓名",string sortBy="ASC",int page=1)
        {
            var data = repoContact.SearchWithPosition(searchStr, positionName,sortField,sortBy).ToPagedList(page,3);
            ViewBag.positionName = GeneratePositionSL();
            return View(data);
        }

        private List<SelectListItem> GeneratePositionSL()
        {
            List<SelectListItem> positionLi = new List<SelectListItem>(new SelectList(repoContact.GetPosition()));
            //SelectList positionLi = new SelectList(repoContact.GetPosition());
           

            SelectListItem allLi = new SelectListItem
            {
                Value = "all",
                Text = "全部"
            };
            positionLi.Insert(0, allLi);
            return positionLi; 
        }

        public ActionResult Export(string searchStr = "", string positionName = "all", string sortField = "姓名", string sortBy = "ASC")
        {
            IQueryable<客戶聯絡人> custContact = repoContact.SearchWithPosition(searchStr, positionName, sortField, sortBy);
            MemoryStream ms = ExportDataToExcel(custContact);
            return File(ms.ToArray(), "application/vnd.ms-excel", "客戶聯絡人.xls");
        }

        private MemoryStream ExportDataToExcel(IQueryable<客戶聯絡人> custContact)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet) workbook.CreateSheet("試算表");

            //設定表頭
            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("職稱");
            sheet.GetRow(0).CreateCell(1).SetCellValue("姓名");
            sheet.GetRow(0).CreateCell(2).SetCellValue("Email");
            sheet.GetRow(0).CreateCell(3).SetCellValue("手機");
            sheet.GetRow(0).CreateCell(4).SetCellValue("電話");
            sheet.GetRow(0).CreateCell(5).SetCellValue("客戶名稱 ");

            //匯出資料
            int count = 1;
            foreach (var item in custContact)
            {
                sheet.CreateRow(count);
                sheet.GetRow(count).CreateCell(0).SetCellValue(item.職稱);
                sheet.GetRow(count).CreateCell(1).SetCellValue(item.姓名);
                sheet.GetRow(count).CreateCell(2).SetCellValue(item.Email);
                sheet.GetRow(count).CreateCell(3).SetCellValue(item.手機);
                sheet.GetRow(count).CreateCell(4).SetCellValue(item.電話);
                sheet.GetRow(count).CreateCell(5).SetCellValue(item.客戶資料.客戶名稱);
                count++;
            }
            workbook.Write(ms);
            workbook = null;
            return ms;
        }


        // GET: 客戶聯絡人/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repoContact.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        

        // GET: 客戶聯絡人/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(dbEntity.客戶資料, "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶聯絡人/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需

        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repoContact.Add(客戶聯絡人);
                repoContact.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(dbEntity.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repoContact.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoCust.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: 客戶聯絡人/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否已刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                repoContact.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                repoContact.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoContact.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: 客戶聯絡人/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = repoContact.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }




        // POST: 客戶聯絡人/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repoContact.Delete(id);
            repoContact.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoContact.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

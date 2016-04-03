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
    public class 客戶銀行資訊Controller : BaseController
    {


        // GET: 客戶銀行資訊
        public ActionResult Index()
        {
            // Include的用法研究 待研究 --- mark by Sean
            // var 客戶銀行資訊 = db.客戶銀行資訊.Include(客 => 客.客戶資料 );
            //return View(客戶銀行資訊.ToList());

            var data = repoCustBank.All(false);
            return View(data);
        }
        [HttpPost]
        public ActionResult Index(string searchStr)
        {
            var data =
                   searchStr == "" ?
                    repoCustBank.All(false) :
                    repoCustBank.All(false).Where(p => p.銀行名稱.Contains(searchStr));
            return View(data);
        }

        public ActionResult Export()
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = (HSSFSheet) workbook.CreateSheet("試算表");

            IQueryable <客戶銀行資訊> custBank = repoCustBank.All(false);

            //設定表頭
            sheet.CreateRow(0);
            sheet.GetRow(0).CreateCell(0).SetCellValue("銀行名稱 ");
            sheet.GetRow(0).CreateCell(1).SetCellValue("銀行代碼 ");
            sheet.GetRow(0).CreateCell(2).SetCellValue("分行代碼");
            sheet.GetRow(0).CreateCell(3).SetCellValue("帳戶名稱");
            sheet.GetRow(0).CreateCell(4).SetCellValue("帳戶號碼");
            sheet.GetRow(0).CreateCell(5).SetCellValue("客戶名稱 ");



            int count = 1;
            foreach (客戶銀行資訊 item in custBank) //實際塞入資料
            {
                sheet.CreateRow(count);
                sheet.GetRow(count).CreateCell(0).SetCellValue(item.銀行名稱);
                sheet.GetRow(count).CreateCell(1).SetCellValue(item.銀行代碼);
                sheet.GetRow(count).CreateCell(2).SetCellValue(item.分行代碼.Value);
                sheet.GetRow(count).CreateCell(3).SetCellValue(item.帳戶名稱);
                sheet.GetRow(count).CreateCell(4).SetCellValue(item.帳戶號碼);
                sheet.GetRow(count).CreateCell(5).SetCellValue(item.客戶資料.客戶名稱);
                count++;
            }
            workbook.Write(ms);
            workbook = null;
            return File(ms.ToArray(), "application/vnd.ms-excel", "客戶銀行資訊.xls");

        }

        // GET: 客戶銀行資訊/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repoCust.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: 客戶銀行資訊/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repoCustBank.Add(客戶銀行資訊);
                repoCustBank.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repoCust.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoCust.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否已刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                repoCustBank.UnitOfWork.Context.Entry(客戶銀行資訊).State = EntityState.Modified;
                repoCustBank.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoCust.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: 客戶銀行資訊/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoCustBank.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: 客戶銀行資訊/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repoCustBank.Delete(id);
            repoCustBank.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoCustBank.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

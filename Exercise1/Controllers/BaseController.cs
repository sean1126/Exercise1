using Exercise1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercise1.Controllers
{
    public abstract class BaseController : Controller
    {

        protected 客戶資料Repository repoCust = RepositoryHelper.Get客戶資料Repository();
        protected 客戶資料ViewRepository repoCustView = RepositoryHelper.Get客戶資料ViewRepository();
        protected 客戶聯絡人Repository repoContact = RepositoryHelper.Get客戶聯絡人Repository();
        protected 客戶銀行資訊Repository repoCustBank = RepositoryHelper.Get客戶銀行資訊Repository();
        protected 客戶類別Repository repoCustCategory = RepositoryHelper.Get客戶類別Repository();


        protected override void HandleUnknownAction(string actionName)
        {
            this.RedirectToAction("Index", "Home").ExecuteResult(this.ControllerContext);
        }
    }
}
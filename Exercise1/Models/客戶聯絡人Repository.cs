using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
	
namespace Exercise1.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{
        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p=>p.是否已刪除 != true);
        }

        /// <summary>
        /// 讀取所有客戶聯絡人資料，可選擇是否包含已刪除的資料
        /// </summary>
        /// <param name="isContainDelete">是否包含已刪除的聯絡人</param>
        /// <returns></returns>
        public IQueryable<客戶聯絡人> All(bool isContainDelete)
        {
            if (isContainDelete)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        public 客戶聯絡人 Find(int id)
        {
            return this.All(true).FirstOrDefault(p=>p.Id == id);
        }

        public override void Delete(客戶聯絡人 delObj)
        {
            delObj.是否已刪除 = true;
        }

        internal IQueryable<客戶聯絡人> SearchWithPosition(string searchStr, string positionName, string sortField, string sortBy)
        {
            IQueryable<客戶聯絡人> rtnData = All();

            if (searchStr != "")
                rtnData = rtnData.Where(p => p.姓名.Contains(searchStr));
            if (positionName != "all")
                rtnData = rtnData.Where(p => p.職稱 == positionName);

            return rtnData.OrderBy(sortField + " " + sortBy);
        }

        public void Delete(int id)
        {
            客戶聯絡人 delObj = this.Find(id);
            this.Delete(delObj);
        }

        internal IQueryable<客戶聯絡人> SearchWithPosition(string searchStr, string positionName)
        {
            if (searchStr == "" && positionName == "all")
            {
                return this.All(isContainDelete: false);
            }
            else if (searchStr == "")
            {
                return Where(p => p.職稱 == positionName);
            }
            else if (positionName == "all")
            {
                return Where(p => p.姓名.Contains(searchStr));
            }
            else {
                return Where(p => p.姓名.Contains(searchStr) && p.職稱 == positionName);
            }
        }

        public IQueryable<string> GetPosition()
        {
            return  All().Select(p => p.職稱).Distinct();

        }

	}

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}
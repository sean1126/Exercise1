using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Collections.Generic;
	
namespace Exercise1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{

        /// <summary>
        /// 撈出全部客戶資料(不包含已刪除)
        /// </summary>
        /// <returns></returns>
        public override IQueryable<客戶資料> All()
        {
            return All(isContainDel: false);
        }

        public IQueryable<客戶資料> All(bool isContainDel)
        {
            if (isContainDel)
            {
                return base.All();
            }
            else {
                return this.Where(p => p.是否已刪除 == false);
            }
        }

        /// <summary>
        /// 根據傳入字串與類別，搜尋客戶資料
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> SearchByCategory(string searchText, int categoryId)
        {
            IQueryable<客戶資料> rtnData = All();
            if (categoryId != 0)
                rtnData = rtnData.Where(p => p.類別Id == categoryId);
            if (searchText != "")
                rtnData = rtnData.Where(p => p.客戶名稱.Contains(searchText));
            return rtnData;
        }


        /// <summary>
        /// 根據傳入字串與類別，搜尋客戶資料
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> SearchByCategory(string searchText, int categoryId, string sortField, string sortBy)
        {
            IQueryable<客戶資料> rtnData = All();

            //如果需要類別篩選
            if (categoryId != 0)
                rtnData = rtnData.Where(p => p.類別Id == categoryId);

            //如果需要搜尋
            if (searchText != "")
                rtnData = rtnData.Where(p => p.客戶名稱.Contains(searchText));

            rtnData = rtnData.OrderBy(sortField + " "+ sortBy);

            //switch (sortField)
            //{
            //    case "Category":
            //        rtnData = sortBy == "ASC"? rtnData.OrderBy(p => p.類別Id): rtnData.OrderByDescending(p => p.類別Id);
            //        break;
            //    case "CustName":
            //        rtnData = sortBy == "ASC" ? rtnData.OrderBy(p => p.客戶名稱) : rtnData.OrderByDescending(p => p.客戶名稱);
            //        break;
            //    default:
            //        break;
            //}
            return rtnData;
        }



        public 客戶資料 Find(int id)
        {
            return this.All(true).FirstOrDefault(p => p.Id == id);

        }

        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
        }
    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}
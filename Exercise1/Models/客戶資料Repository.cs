using System;
using System.Linq;
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
            if (searchText == "" && categoryId == 0)
            {
                return All().OrderBy(p => p.客戶名稱);
            }
            else if (categoryId == 0)
            {
                return this.Where(p => p.客戶名稱.Contains(searchText));
            }
            else if (searchText == "")
            {
                return this.Where(p => p.類別Id == categoryId);
            }
            else {
                return this.Where(p => p.類別Id == categoryId && p.客戶名稱.Contains(searchText));
            }
        }


        /// <summary>
        /// 根據傳入字串與類別，搜尋客戶資料
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IQueryable<客戶資料> SearchByCategory(string searchText, int categoryId, string sortField, string sortBy)
        {
            if (searchText == "" && categoryId == 0)
            {
                return All().OrderBy(p => p.客戶名稱);
            }
            else if (categoryId == 0)
            {
                return this.Where(p => p.客戶名稱.Contains(searchText));
            }
            else if (searchText == "")
            {
                return this.Where(p => p.類別Id == categoryId);
            }
            else {
                return this.Where(p => p.類別Id == categoryId && p.客戶名稱.Contains(searchText));
            }
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
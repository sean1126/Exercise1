using System;
using System.Linq;
using System.Collections.Generic;
	
namespace Exercise1.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{
        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => p.是否已刪除 != true);
        }

        public IQueryable<客戶資料> All(bool isContainDel)
        {
            if (isContainDel)
            {
                return base.All();
            }
            else {
                return this.All(); //濾掉已刪除的客戶
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
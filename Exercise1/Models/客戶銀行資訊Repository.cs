using System;
using System.Linq;
using System.Collections.Generic;
	
namespace Exercise1.Models
{   
	public  class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
	{
        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p => p.是否已刪除 != true);
        }

        public IQueryable<客戶銀行資訊> All( bool isContainDelete)
        {
            if (isContainDelete)
            {
              return  base.All();
            }
            else {
              return this.All();
            }
        }

        public 客戶銀行資訊 Find(int id)
        {
            return All(true).FirstOrDefault(p => p.Id == id);
        }

        public override void Delete(客戶銀行資訊 entity)
        {
            entity.是否已刪除 = true;
        }

        public void Delete(int id)
        {
            客戶銀行資訊 entity = this.Find(id);
            Delete(entity);
        }


    }

	public  interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
	{

	}
}
using System;
using System.Linq;
using System.Collections.Generic;
	
namespace Exercise1.Models
{   
	public  class 客戶資料ViewRepository : EFRepository<客戶資料View>, I客戶資料ViewRepository
	{

	}

	public  interface I客戶資料ViewRepository : IRepository<客戶資料View>
	{

	}
}
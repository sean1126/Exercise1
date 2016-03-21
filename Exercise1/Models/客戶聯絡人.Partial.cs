namespace Exercise1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;





    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var data = db.客戶聯絡人.Where(p => p.Email == this.Email && p.Id != this.Id && p.客戶Id == this.客戶Id);
            bool isDuplicate = data.Count() > 0 ? true : false;
            if (isDuplicate)
            {
                yield return new ValidationResult("聯絡人email不允許重複", new string[] { "Email" });
            }
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(250, ErrorMessage="欄位長度不得大於 250 個字元")]
        [Required]
        public string Email { get; set; }
        
        [手機格式驗證]
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 手機 { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }
        public Nullable<bool> 是否已刪除 { get; set; }
    
        public virtual 客戶資料 客戶資料 { get; set; }
    }
}

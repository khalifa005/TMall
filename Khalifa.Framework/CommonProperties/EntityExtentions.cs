using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Khalifa.Framework.CommonProperties
{
    public class EntityExtentions 
    {
        public abstract class ChangeInfo
        {
            public DateTime? CreatedAt { get; set; }
            [Required]
            public string CreatedBy { get; set; }
            public DateTime? LastModifiedAt { get; set; }
            public string LastModifiedBy { get; set; }
        }
        
        public abstract class CreateInfo
        {
            public DateTime? CreatedAt { get; set; }
            [Required]
            public string CreatedBy { get; set; }
        }
        
        public abstract class Lookup : ChangeInfo
        {
            public int Id { get; set; }

            public string NameArabic { get; set; }
            [Required]
            public string NameEnglish { get; set; }
            public bool IsActive { get; set; }
        }

        public abstract class LookupWithDescription : Lookup
        {
            public string DescriptionArabic { get; set; }
            [Required]
            public string DescriptionEnglish { get; set; }
        }

        public abstract class MultiString
        {
            public string NameArabic { get; set; }
            [Required]
            public string NameEnglish { get; set; }
        }

        public abstract class MultiDescription
        {
            public string DescriptionArabic { get; set; }
            [Required]
            public string DescriptionEnglish { get; set; }
        }

        public abstract class MultiStringWithDescription : MultiString
        {
            public string DescriptionArabic { get; set; }
            [Required]
            public string DescriptionEnglish { get; set; }
        }
    
    
    }
}

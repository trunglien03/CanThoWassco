//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NienLuan2.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class LO_TRINH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LO_TRINH()
        {
            this.KHACH_HANG = new HashSet<KHACH_HANG>();
        }
    
        public string LT_MA { get; set; }
        public string NV_USERNAME { get; set; }
        public string LT_TEN { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KHACH_HANG> KHACH_HANG { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}

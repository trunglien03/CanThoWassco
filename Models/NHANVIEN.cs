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
    
    public partial class NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHANVIEN()
        {
            this.LO_TRINH = new HashSet<LO_TRINH>();
        }
    
        public string NV_USERNAME { get; set; }
        public string VT_MA { get; set; }
        public string CV_MA { get; set; }
        public string PB_MA { get; set; }
        public string NV_PASSWORD { get; set; }
        public string NV_HOTEN { get; set; }
        public string NV_SDT { get; set; }
        public string NV_EMAIL { get; set; }
        public string NV_AVATAR { get; set; }
    
        public virtual CHUCVU CHUCVU { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LO_TRINH> LO_TRINH { get; set; }
        public virtual VAITRO VAITRO { get; set; }
        public virtual PHONGBAN PHONGBAN { get; set; }
    }
}

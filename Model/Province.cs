//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Province
    {
        public Province()
        {
            this.Supplies = new HashSet<Supply>();
        }
    
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Supply> Supplies { get; set; }
    }
}

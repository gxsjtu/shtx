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
    
    public partial class MyAppGroup
    {
        public int id { get; set; }
        public int GroupID { get; set; }
        public string tel { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<int> Flag { get; set; }
    
        public virtual XHMarketGroup XHMarketGroup { get; set; }
    }
}
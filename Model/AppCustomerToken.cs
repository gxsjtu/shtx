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
    
    public partial class AppCustomerToken
    {
        public string tel { get; set; }
        public string devicetoken { get; set; }
        public Nullable<int> salt { get; set; }
        public Nullable<System.DateTime> updatedate { get; set; }
        public Nullable<bool> isSound { get; set; }
        public Nullable<int> PhoneType { get; set; }
        public Nullable<int> PrePhoneType { get; set; }
    }
}

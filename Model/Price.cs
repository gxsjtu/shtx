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
    
    public partial class Price
    {
        public int PriceID { get; set; }
        public Nullable<int> MarketID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string LPrice { get; set; }
        public string HPrice { get; set; }
        public string APrice { get; set; }
        public Nullable<System.DateTime> AddDate { get; set; }
        public string Water { get; set; }
        public string NewPrice { get; set; }
        public string Storehouse { get; set; }
        public string StrikeBargain { get; set; }
        public string Stock { get; set; }
        public string SpecialL { get; set; }
        public string SpecialH { get; set; }
        public string PriceChange { get; set; }
    }
}

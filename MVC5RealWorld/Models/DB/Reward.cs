//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC5RealWorld.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reward
    {
        public int RewardID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> BronzeValue { get; set; }
        public Nullable<int> SilverValue { get; set; }
        public Nullable<int> GoldValue { get; set; }
        public string MaturityLevel { get; set; }
        public Nullable<decimal> Multiplier { get; set; }
    }
}
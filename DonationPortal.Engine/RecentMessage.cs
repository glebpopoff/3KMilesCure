//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DonationPortal.Engine
{
    using System;
    using System.Collections.Generic;
    
    public partial class RecentMessage
    {
        public int ID { get; set; }
        public int DonationID { get; set; }
        public System.DateTime DateReceived { get; set; }
    
        public virtual RiderMessageDonation RiderMessageDonation { get; set; }
    }
}

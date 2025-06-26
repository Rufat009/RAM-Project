using System;

namespace project.Models
{
    public class Check
    {
        public string Service { get; set; }
        public string ItemName { get; set; }
        public decimal AmountDeducted { get; set; }
        public decimal BalanceAfter { get; set; }
        public DateTime TimeBought { get; set; }
    }
} 
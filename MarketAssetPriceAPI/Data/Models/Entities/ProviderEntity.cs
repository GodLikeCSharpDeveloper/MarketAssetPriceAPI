﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MarketAssetPriceAPI.Data.Models.Entities
{
    public class ProviderEntity
    {
        public int Id { get; set; }
        public string? ProviderName { get; set; }
        public string? Symbol { get; set; }
        [NotMapped]
        public string? Exchange { get; set; }
        public int? ExchangeId { get; set; }
        public int? DefaultOrderSize { get; set; }

    }
}

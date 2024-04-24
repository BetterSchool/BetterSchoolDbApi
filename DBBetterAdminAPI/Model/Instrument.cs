using BetterAdminDbAPI.Model.@enum;

namespace BetterAdminDbAPI.Model
{
    public class Instrument
    {
        public int? InstrumentId { get; set; }
        public decimal Price { get; set; }
        public InstrumentType Type { get; set; }
    }
}

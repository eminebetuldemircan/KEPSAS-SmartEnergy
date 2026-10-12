namespace SmartEnergy.Web.Models;

public class ConsumptionFilterViewModel
{
    public Guid? MeterId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public List<MeterViewModel> Meters { get; set; }
        = new List<MeterViewModel>();

    public List<ConsumptionRecordViewModel> Records { get; set; }
        = new List<ConsumptionRecordViewModel>();
}
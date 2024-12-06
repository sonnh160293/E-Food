namespace FoodOnline.Application.DTOs.GetDTO
{
    public class FeeDTO
    {
        public string Name { get; set; }
        public int Fee { get; set; }
        public int InsuranceFee { get; set; }
        public string DeliveryType { get; set; }
        public int A { get; set; }
        public string Dt { get; set; }
        public List<ExtFeeDTO> ExtFees { get; set; }
        public bool Delivery { get; set; }
        public int? BranchId { get; set; }
    }
}

namespace FoodOnline.Application.DTOs.GetDTO
{
    public class CartGetDTO
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }

        public int ProductId { get; set; }
        public ProductGetDTO Product { get; set; }
        public int Quantity { get; set; }
    }
}

namespace TrainComponentManagement.BLL.DTOs
{
    public class CreateOrUpdateComponentDto
    {
        public string Name { get; set; } = null!;
        public string UniqueNumber { get; set; } = null!;
        public bool CanAssignQuantity { get; set; }
        public int? Quantity { get; set; }
    }
}

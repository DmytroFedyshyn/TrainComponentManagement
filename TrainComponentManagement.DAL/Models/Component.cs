namespace TrainComponentManagement.DAL.Models
{
    public class Component
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string UniqueNumber { get; set; } = default!;
        public bool CanAssignQuantity { get; set; }
        public int? Quantity { get; set; }
    }
}
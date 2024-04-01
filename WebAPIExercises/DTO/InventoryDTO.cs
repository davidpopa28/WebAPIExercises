namespace WebAPIExercises.DTO
{
    public class InventoryDTO
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public int NrOfProducts { get; set; }
    }
}

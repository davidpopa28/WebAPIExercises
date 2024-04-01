namespace WebAPIExercises.Models
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public Guid ProductId { get; set; }
        public int NrOfProducts { get; set; }
        public Store Store { get; set; } 
        public Product Product { get; set; }
    }
}

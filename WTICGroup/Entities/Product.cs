namespace WTICGroup.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Quantiy { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        public bool? Edited => CreatedDate != UpdatedDate;
    }
}

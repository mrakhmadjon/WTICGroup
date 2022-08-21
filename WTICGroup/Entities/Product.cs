namespace WTICGroup.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Quantiy { get; set; }

        public decimal? Price { get; set; }

        public decimal? TotalPrice { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTimeOffset? CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTimeOffset? UpdatedDate { get; set; }

        public bool? Edited => CreatedDate != UpdatedDate;
    }
}

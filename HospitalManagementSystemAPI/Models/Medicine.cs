namespace HospitalManagementSystemAPI.Models
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int QuantityInMG { get; set; }
        public int Units { get; set; }
        public double Price { get; set; }
    }
}

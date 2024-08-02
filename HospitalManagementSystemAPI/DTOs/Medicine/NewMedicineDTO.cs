using System.ComponentModel.DataAnnotations;

namespace HospitalManagementSystemAPI.DTOs.Medicine
{
    public class NewMedicineDTO
    {
        [Required(ErrorMessage = "Medicine name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter a valid quantity.")]
        public int QuantityInMG { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid price.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Number of units is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Enter a valid value.")]
        public int Units { get; set; }
    }
}

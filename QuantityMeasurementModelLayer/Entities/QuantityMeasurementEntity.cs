 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace QuantityMeasurementModelLayer.Entities
{
    [Table("QuantityMeasurements")]   // maps this class to the QuantityMeasurements table
    public class QuantityMeasurementEntity
    {
        [Key]                                                    // primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    // auto-increment
        public int Id { get; set; }
 
        [Required]
        [MaxLength(20)]
        public string Operation { get; set; }
 
        public double FirstValue { get; set; }
        public double SecondValue { get; set; }
        
        public string FirstUnit { get; set; }
        public string SecondUnit { get; set; }
        
        public string MeasurementType { get; set; }
 
        [Required]
        [MaxLength(50)]
        public string Result { get; set; }
 
        // Required for Redis JSON deserialization
        public QuantityMeasurementEntity() { }
 
        public QuantityMeasurementEntity(string operation, double firstValue, double secondValue, string result, string firstUnit = null, string secondUnit = null)
        {
            Operation = operation;
            FirstValue  = firstValue;
            SecondValue  = secondValue;
            Result    = result;
            FirstUnit  = firstUnit;
            SecondUnit  = secondUnit;
            MeasurementType = operation;
        }
    }
}

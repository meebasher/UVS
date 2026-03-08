using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UVS.Domain.Entities
{
    [Table("employees")]
    public class Employee
    {
        [Key]
        [Column("employeeid")]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [Column("employeename")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column("employeesalary")]
        [Range(0, int.MaxValue, ErrorMessage = "Salary must be a non-negative integer.")]
        public int Salary { get; set; }
    }
}

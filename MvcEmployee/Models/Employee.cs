using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace MvcEmployee.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public short Age { get; set; }
        [Display(Name = "Profile Image")]
        public byte[] ProfileImage { get; set; }
        [Display(Name = "Annual Salary")]
        public int AnnualSalary { get; set; }
    }

    public class EmployeeDBContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
    }
}
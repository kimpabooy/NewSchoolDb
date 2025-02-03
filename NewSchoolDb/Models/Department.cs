using System;
using System.Collections.Generic;

namespace NewSchoolDb.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public decimal? Salary { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}

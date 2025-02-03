using System;
using System.Collections.Generic;

namespace NewSchoolDb.Models;

public partial class Class
{
    public int ClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public int? StaffId { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}

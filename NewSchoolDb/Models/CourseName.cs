using System;
using System.Collections.Generic;

namespace NewSchoolDb.Models;

public partial class CourseName
{
    public int CourseNameId { get; set; }

    public string CourseName1 { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}

using System;
using System.Collections.Generic;

namespace NewSchoolDb.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string CourseName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int? SubjectId { get; set; }

    public virtual Subject? Subject { get; set; }
}

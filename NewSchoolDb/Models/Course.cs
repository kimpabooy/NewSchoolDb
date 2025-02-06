using System;
using System.Collections.Generic;

namespace NewSchoolDb.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public int? CourseNameId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? SubjectId { get; set; }

    public virtual CourseName? CourseName { get; set; }

    public virtual Subject? Subject { get; set; }
}

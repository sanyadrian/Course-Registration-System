using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Lab6.DataAccess;

[ModelMetadataType(typeof(AcademicRecordMetadata))]
public partial class AcademicRecord
{
    public string CourseCode { get; set; } = null!;

    public string StudentId { get; set; } = null!;
    
    public int? Grade { get; set; }

    public virtual Course CourseCodeNavigation { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}

using System.ComponentModel.DataAnnotations;
using Azure.Core.Pipeline;
namespace Lab6.DataAccess;


public class AcademicRecordMetadata
{
    [Range(0, 100, ErrorMessage = "Must be between 0 and 100")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "Must be between 0 and 100")]
    public int? Grade { get; set; }
}
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Lab6.DataAccess;

public partial class Role
{
    public int Id { get; set; }

    public string? Role1 { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public int getEmployeedId() {
        Id = 0;
        foreach (var employee in Employees) {
            return Id = employee.Id;
        }
        return Id;
    }

}

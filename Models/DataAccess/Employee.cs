using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace Lab6.DataAccess;
[ModelMetadataType(typeof(EmployeeMetadata))]
public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;


    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public IEnumerable<string> GetRoleNames()
        {
            return Roles.Select(role => role.Role1);
        }


}

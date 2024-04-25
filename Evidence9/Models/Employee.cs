using System;
using System.Collections.Generic;

namespace Evidence9.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public DateTime JoinDate { get; set; }

    public decimal Salary { get; set; }

    public string? Picture { get; set; }

    public bool ActiverStatus { get; set; }

    public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
}

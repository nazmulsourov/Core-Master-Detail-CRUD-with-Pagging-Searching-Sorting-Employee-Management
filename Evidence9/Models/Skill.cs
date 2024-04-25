using System;
using System.Collections.Generic;

namespace Evidence9.Models;

public partial class Skill
{
    public int SkillId { get; set; }

    public string SkillName { get; set; }

    public virtual ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
}

using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class SkillGroupAttribute : Attribute
{
    public SkillGroup Group { get; }

    public SkillGroupAttribute(SkillGroup group)
    {
        Group = group;
    }
}
using System.Linq;

public enum SkillID
{
    [SkillGroup(SkillGroup.Medicine)] FirstAid,
    [SkillGroup(SkillGroup.Medicine)] Traumatology,
    [SkillGroup(SkillGroup.Medicine)] Toxicology,
    [SkillGroup(SkillGroup.Medicine)] Pharmacology,
    [SkillGroup(SkillGroup.Medicine)] Epidemiology,
    [SkillGroup(SkillGroup.Medicine)] Veterinary,
    [SkillGroup(SkillGroup.Medicine)] Psychology,
    [SkillGroup(SkillGroup.Medicine)] Operating,
    [SkillGroup(SkillGroup.FireArm)] Pistols,
    [SkillGroup(SkillGroup.FireArm)] Rifles,
    [SkillGroup(SkillGroup.FireArm)] SniperRifles,
    [SkillGroup(SkillGroup.FireArm)] MachineGun,
    [SkillGroup(SkillGroup.FireArm)] Shotguns,
}

public static class SkillIDExtensions
{
    public static SkillGroup GetGroup(this SkillID skill)
    {
        var type = typeof(SkillID);
        var memberInfo = type.GetMember(skill.ToString());
        if (memberInfo.Length > 0)
        {
            SkillGroupAttribute attribute = memberInfo[0].GetCustomAttributes(typeof(SkillGroupAttribute), false).FirstOrDefault() as SkillGroupAttribute;
            return attribute.Group;
        }

        return SkillGroup.Undefined;
    }
}
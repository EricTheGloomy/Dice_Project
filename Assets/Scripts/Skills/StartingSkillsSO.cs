using UnityEngine;

[CreateAssetMenu(fileName = "StartingSkillsConfig", menuName = "Skills/Starting Skills Configuration")]
public class StartingSkillsSO : ScriptableObject
{
    public SkillSO[] startingSkills;
}

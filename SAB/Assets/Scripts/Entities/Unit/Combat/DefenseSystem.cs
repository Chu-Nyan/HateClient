using SAB.Unit.Combat;
using System.Collections.Generic;

public class DefenseSystem
{
    private List<SkillSequence> _sequences;

    public DefenseSystem()
    {
        _sequences = new();
    }

    public void Attack(AttackContext context, HitResult hit)
    {
        SkillSequence sequence = SkillGenerator.Instance.GenerateSequence(context,hit);
        _sequences.Add(sequence);
    }

    public bool Tick(IHasStats stats)
    {
        for (int i = _sequences.Count - 1; i >= 0; i--)
        {
            if (_sequences[i].TickAndCheck(stats) == false)
                continue;

            //Debug.Log($"{_sequences[i].Context.SkillData.StringID} 제거됨");
            _sequences.RemoveAt(i);
        }
        return true;
    }
}

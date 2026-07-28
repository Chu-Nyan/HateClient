namespace SAB.Unit
{
    public struct ModifierStat
    {
        public float Sum;
        public float Mul;

        public static ModifierStat operator +(ModifierStat a, ModifierStat b)
        {
            return new ModifierStat
            {
                Sum = a.Sum + b.Sum,
                Mul = a.Mul + b.Mul
            };
        }
    }
}

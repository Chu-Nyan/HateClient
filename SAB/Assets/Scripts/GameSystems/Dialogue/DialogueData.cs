public class DialogueData
{
    public readonly static DialogueData[] Sample = new DialogueData[] { new("1", 1), new("2", 2), new("3", 3) };

    public readonly string Text;
    public readonly float Time;

    public DialogueData(string text, float time)
    {
        Text = text;
        Time = time;
    }
}

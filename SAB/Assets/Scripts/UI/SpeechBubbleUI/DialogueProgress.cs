public class DialogueProgress
{
    public int ID;
    public float PlayTime;

    public DialogueProgress(int id, float playTime)
    {
        ID = id;
        PlayTime = playTime;
    }

    public void Reset()
    {
        PlayTime = 0;
    }
}

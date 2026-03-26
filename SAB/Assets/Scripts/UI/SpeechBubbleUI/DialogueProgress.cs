public class DialogueProgress
{
    public int ID;
    public int ProgressStep;
    public float RemainTime;

    public DialogueProgress(int id, int progressStep, float time)
    {
        ID = id;
        ProgressStep = progressStep;
        RemainTime = time;
    }
}

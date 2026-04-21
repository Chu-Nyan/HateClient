using System.Collections.Generic;

namespace SAB.Cutscene
{
    public class CutsceneVCamData
    {
        public string Name;
        public Dictionary<string, int> VCamIDByClipName; 
        public Dictionary<int, VCamStaticData> StaticData;
        public Dictionary<int, VCamFollowData> FollowData;
    }
}

namespace SAB.Facade
{
    public class GamePlayFacade
    {
        public readonly CutsceneFacade Cutscene;
        public readonly MapFacade Map;

        public GamePlayFacade(CutsceneFacade cutscene, MapFacade changeMap)
        {
            Cutscene = cutscene;
            Map = changeMap;
        }
    }
}

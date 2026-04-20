namespace SAB.Cutscene
{
    public struct GenericAniTrackData
    {
        public int ID;
        public string Name;
        public string MeshPath;

        public GenericAniTrackData(int id, string name, string meshPath)
        {
            ID = id;
            Name = name;
            MeshPath = meshPath;
        }

        public readonly override string ToString()
        {
            return $"ID : {ID}, Name : {Name}, Mesh Path : {MeshPath}";
        }
    }
}

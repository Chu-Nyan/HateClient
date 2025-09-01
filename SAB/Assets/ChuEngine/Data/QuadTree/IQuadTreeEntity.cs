using System.Collections.Generic;
using Chu.Data;

public interface IQuadTreeEntity
{
    public RectBound RectBound { get; }
    public HashSet<int> InsertedNodesID { get; }

    public void RegisterQuadTreeNodeID(int id);
    public void ResetInsertedNodes();
}

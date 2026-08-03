using Chu.Data;
using System.Collections.Generic;

namespace Chu.Collections
{
    public interface IQuadTreeEntity
    {
        public RectBound RectBound { get; }
        public HashSet<int> InsertedNodesID { get; }

        public void AddQuadTreeNodeID(int id);
        public void ClearInsertedNodes();
    }
}
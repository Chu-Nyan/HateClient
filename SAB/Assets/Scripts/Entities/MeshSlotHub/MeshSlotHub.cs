using Chu.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.MeshSlot
{
    public partial class MeshSlotHub : MonoBehaviour
    {
        [SerializeField]
        private SerializablePair<SlotType, Component>[] _bodyPartComponent;
        private Dictionary<SlotType, IMeshAdapter> _handlerByPart;

        private void Awake()
        {
            _handlerByPart = new Dictionary<SlotType, IMeshAdapter>();
            for (int i = 0; i < _bodyPartComponent.Length; i++)
            {
                SerializablePair<SlotType, Component> item = _bodyPartComponent[i];
                IMeshAdapter handler = ConvertMeshHandler(item.Value);

#if (UNITY_EDITOR)
                if (_handlerByPart.TryAdd(item.Key, handler) == false)
                {
                    Debug.Log("중복 파츠 추가");
                }
#endif
                _handlerByPart[item.Key] = handler;
            }
        }

        public void SetMesh(SlotType part, Mesh mesh)
        {
            if (_handlerByPart.ContainsKey(part) == false)
            {
                Debug.Log("Slot이 존재하지 않음");
                return;
            }
            _handlerByPart[part].SetMesh(mesh);
        }

        private IMeshAdapter ConvertMeshHandler(Component renderer)
        {
            if (renderer is SkinnedMeshRenderer)
            {
                var item = (SkinnedMeshRenderer)renderer;
                return new SkinnedMeshAdapter(item);
            }
            else if (renderer is MeshFilter)
            {
                var item = (MeshFilter)renderer;
                return new MeshFilterAdapter(item);
            }
            else
            {
                throw new System.Exception();
            }
        }
    }
}

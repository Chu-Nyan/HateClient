using SAB.EntityAgent.AI;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace SAB.EntityAgent
{
    /// <summary>
    /// Agent 통합 관리
    /// </summary>
    public class AgentController : MonoBehaviour
    {
        private static int _idCounter = 0;
        private const int _playerAgentID = 0;
        private Agent[] _agents;
        private Dictionary<int, Agent> _activeAgentByReceiverId;
        private Queue<Agent> _idleAgent;

        private void Awake()
        {
            _agents = new Agent[200];
            _activeAgentByReceiverId = new();
            _idleAgent = new();
            GenerateAgent(new PlayerBrain());
        }

        private void Update()
        {
            foreach (var item in _activeAgentByReceiverId)
            {
                item.Value.Tick();
            }
        }

        public void BindReceiver(IInputReceiver receiver, UnitController.Oner oner)
        {
            Agent handler;
            if (oner == UnitController.Oner.Player)
            {
                handler = _agents[_playerAgentID];
            }
            else
            {
                if (_idleAgent.Count == 0)
                {
                    Agent aiHandler = GenerateAgent(AIGenerator.Instance.TempGenerate());
                    _idleAgent.Enqueue(aiHandler);
                }
                handler = _idleAgent.Dequeue();
            }

            handler.SetReceivers(receiver);
            handler.SetActive(true);
            _activeAgentByReceiverId[receiver.ReceiverID] = handler;
        }

        public void UnbindReceiver(IInputReceiver receiver)
        {
            _activeAgentByReceiverId[receiver.ReceiverID].SetActive(false);
            _activeAgentByReceiverId.Remove(receiver.ReceiverID);
        }

        private Agent GenerateAgent(IBrainStrategy strategy)
        {
            // TODO : PlayerHandler를 제외하고 돌려쓸 수 있게 변경
            var handler = new Agent(_idCounter);
            _idCounter++;
            handler.SetBehaviorStrategy(strategy);
            _agents[handler.ID] = handler;
            _idleAgent.Enqueue(handler);
            return handler;
        }
    }
}

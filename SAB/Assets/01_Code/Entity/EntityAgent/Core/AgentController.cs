using SAB.EntityAgent.AI;
using System.Collections.Generic;
using UnityEngine;

namespace SAB.EntityAgent
{
    /// <summary>
    /// Agent 통합 관리
    /// </summary>
    public class AgentController : MonoBehaviour
    {
        private const int PlayerAgentID = 0;

        private static int _idCounter = 0;
        private Agent[] _agents;
        private Dictionary<int, Agent> _activeAgentByReceiverId;
        private Queue<Agent> _idleNPCAgent;

        public Agent Player
        {
            get => _agents[PlayerAgentID];
        }

        private void Awake()
        {
            _agents = new Agent[200];
            _activeAgentByReceiverId = new();
            _idleNPCAgent = new();
            GenerateAgent(new PlayerBrain());
        }

        private void Update()
        {
            foreach (var item in _activeAgentByReceiverId)
            {
                item.Value.Tick();
            }
        }

        public void BindReceiver(IInputReceiver receiver, BrainType type)
        {
            if (type == BrainType.None)
                return;

            Agent handler;

            if (type == BrainType.Player)
            {
                handler = _agents[PlayerAgentID];
            }
            else
            {
                if (_idleNPCAgent.Count == 0)
                {
                    _idleNPCAgent.Enqueue(GenerateAgent(AIGenerator.Instance.TempGenerate()));
                }

                handler = _idleNPCAgent.Dequeue();
            }

            _activeAgentByReceiverId[receiver.ReceiverID] = handler;
            handler.SetReceivers(receiver);
            handler.SetActive(true);
        }

        public void UnbindReceiver(IInputReceiver receiver)
        {
            _activeAgentByReceiverId[receiver.ReceiverID].SetActive(false);
            _activeAgentByReceiverId.Remove(receiver.ReceiverID);
        }

        private Agent GenerateAgent(IBrainStrategy brain)
        {
            Agent agent = new(_idCounter, brain);
            _agents[agent.ID] = agent;
            _idCounter++;
            return agent;
        }
    }
}

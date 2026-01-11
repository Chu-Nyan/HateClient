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
        private static int _idCounter = 1;
        private const int _playerAgentID = 0;
        private Agent[] _agents;
        private Dictionary<int, Agent> _activeAgentByReceiverId;
        private Queue<Agent> _idleNPCAgent;

        private void Awake()
        {
            _agents = new Agent[200];
            _activeAgentByReceiverId = new();
            _idleNPCAgent = new();
            GeneratePlayerAgent();
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
                if (_idleNPCAgent.Count == 0)
                    GenerateNPCAgent(AIGenerator.Instance.TempGenerate());

                handler = _idleNPCAgent.Dequeue();
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

        private Agent GenerateNPCAgent(IBrainStrategy strategy)
        {
            Agent agent = GenerateAgent(_idCounter, strategy);
            _idCounter++;
            _idleNPCAgent.Enqueue(agent);
            return agent;
        }

        private Agent GeneratePlayerAgent()
        {
            return GenerateAgent(_playerAgentID, new PlayerBrain());
        }

        private Agent GenerateAgent(int id, IBrainStrategy strategy)
        {
            var agent = new Agent(id);
            agent.SetBehaviorStrategy(strategy);
            _agents[agent.ID] = agent;
            return agent;
        }
    }
}

using Chu.AI;
using Chu.Art;
using Chu.Collision;
using Chu.Utility.UnityHelper;
using SAB.Cutscene;
using SAB.EntityAgent;
using SAB.Item;
using SAB.Unit;
using SAB.Unit.Combat;
using SAB.Unit.State;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour, IMovementReceiver, IOffenseReceiver, IDefendable, ICutsceneObject
{
    [SerializeField]
    private Transform _attackOrigin;

    private int _instanceID;

    private CharacterStats _stats;
    private StateContext _stateContext;
    private FSM<CharacterState, StateContext> _state;

    [SerializeField]
    private NavMeshAgent _nav;
    [SerializeField]
    private MeshSlotHub _meshHub;
    private CharacterAnimator _animator;

    private CharacterBody _body;
    private OffenseSystem _combatSystem;
    private DefenseSystem _defenseSystem;

    private EquipmentSystem _equipmentSys;

    private event Action<Character> Deactivated;

    public int InstanceID
    {
        get => _instanceID;
    }

    public int ReceiverID
    {
        get => _instanceID;
    }

    public CharacterStats Stats
    {
        get => _stats;
    }

    public List<Skill> Skills
    {
        get => _combatSystem.SkillList;
    }

    private void Awake()
    {
        _defenseSystem = new DefenseSystem();
        _stateContext = new StateContext();
        _stats = new CharacterStats();
        CircleShape shape = ShapeFactory.Instance.Generate<CircleShape>();
        shape.Setup(new CircleRangeData(Vector2.one, 1));
        _body = new(_instanceID, transform.position, transform.eulerAngles.y, this, shape);
    }

    public void Init(int instanceID)
    {
        _instanceID = instanceID;
        _combatSystem = new OffenseSystem(instanceID, _attackOrigin);
        _state = GenerateStateHandler();
        _animator = new CharacterAnimator(GetComponent<Animator>());
        _equipmentSys = new();

        _state.Setup(_stateContext);
        _animator.RegisterAnimationEvent(AniState.Attack, "AttackFinished", new AniEventData(), 1, a => _stateContext.IsAttacking = false);
        _animator.RegisterAnimationEvent(AniState.Attack, "BasicAttack", new AniEventData(), 0.5f, _combatSystem.AttackWithAnimator);
    }

    private void Update()
    {
        StateUpdate();
        if (_stateContext.IsMoveing == true)
        {
            var pos = transform.position.ToVector2XZ();
            var eulerY = transform.eulerAngles.y;
            _body.OnPositionChanged(pos, eulerY);
        }

        _state.Tick(_stateContext);
        _combatSystem.Tick();
        _defenseSystem.Tick(_stats);
        _animator.Tick(_stateContext, 1); // 1 << 이동속도 퍼센트로 넣기
    }

    private void StateUpdate()
    {
        _stateContext.IsMoveing = _nav.velocity.sqrMagnitude > 0.1f;
    }

    private FSM<CharacterState, StateContext> GenerateStateHandler()
    {
        var stateHandler = new FSM<CharacterState, StateContext>(new StateResolver());
        stateHandler.AddStates(new MovementState());
        stateHandler.AddStates(new AttackState());
        return stateHandler;
    }

    public void SetupStats(BaseStats baseStats)
    {
        _stats.SetBaseData(baseStats);
        var skill = SkillGenerator.Instance.GetSkill(1);
        _combatSystem.AddSkill(skill);
    }

    public void SetDestination(Vector3 destination)
    {
        if (_stateContext.IsAttacking == true)
            return;

        _nav.SetDestination(destination);
    }

    public void SetPositionWithNavMash(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out var hit, 2f, NavMesh.AllAreas))
        {
            _nav.Warp(hit.position);
        }
    }

    public void StopMovement()
    {
        _nav.ResetPath();
    }

    public void SetCustomizing(CustomizingData data)
    {
        SetCustomizing(CustomizingPart.Eye, data.Eye);
        SetCustomizing(CustomizingPart.Eyebrow, data.Eyebrow);
        SetCustomizing(CustomizingPart.Hair, data.Hair);
        SetCustomizing(CustomizingPart.Mouth, data.Mouth);
    }

    public void SetCustomizing(CustomizingPart part, int number)
    {
        Mesh mesh = HumanoidCustomizingUtility.GetMesh(part, number);
        _meshHub.SetMesh(part.ToString(), mesh);
    }

    public void Attack(int skillIndex, Vector3 targetPoint)
    {
        if (_stateContext.IsAttacking == true)
            return;
        if (_combatSystem.IsUsed(skillIndex) == true)
            return;

        _stateContext.IsAttacking = true;
        StopMovement();
        transform.rotation = Quaternion.LookRotation(targetPoint - transform.position);
        _animator.SetAttack();
        float dmg = _stats.GetDamage();
        AniEventData data = _combatSystem.TriggerAttackAndGetAniEventData(dmg, skillIndex, targetPoint);
        _animator.ChangeEventData(AniState.Attack, "BasicAttack", data);
    }

    public void Defend(AttackContext attack, HitResult hit)
    {
        _defenseSystem.Attack(attack, hit);

        if (_stats.IsDead == true)
            Die();
    }

    public void SetCombatMode(bool value)
    {
        _stateContext.IsCombatMode = value;
        _animator.SetCombatMode(value);
    }

    public void Die()
    {
        //TODO : 죽음 처리 로직이 끝난 후 SetActive로 마무리
        SetActive(false);
    }

    public void SetActive(bool value)
    {
        if (gameObject.activeSelf == value)
            return;

        if (value == false)
        {
            Deactivated?.Invoke(this);
            Deactivated = null;
        }

        gameObject.SetActive(value);
    }

    public void RegisterDeactivated(Action<Character> callback)
    {
        Deactivated += callback;
    }

    public void Equip(IHasEquipmentData equipment)
    {
        _equipmentSys.Equip(equipment);

        EquipmentTemplateData template = equipment.EquipmentTemplateData.Template;
        Mesh mesh = AssetManager.LoadAssetSync<Mesh>(template.MeshPath);
        _meshHub.SetMesh(template.Slot.ToString(), mesh);

        WeaponStance stance = _equipmentSys.Stance;
        _animator.ChangeStance(stance);
        float BasicAttackTimeing = 0.5f;// 무기 데이터에서 추출할 것

    }

    public void OnOwnerChanged(BrainType type)
    {
        _body.OnOwnerChanged(type == BrainType.Player);
    }

    public void SetCutsceneData(IObjectConfig data)
    {
        if (data is not SpawnRequest request)
            throw new Exception(data.GetType().ToString());

        transform.position = request.Position;
        transform.rotation = request.Rotation;
    }
}

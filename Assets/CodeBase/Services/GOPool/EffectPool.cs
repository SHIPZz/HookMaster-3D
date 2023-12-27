using System.Collections.Generic;
using System.Linq;
using CodeBase.Enums;
using CodeBase.Gameplay.Effects;
using CodeBase.Services.Factories.Effect;

public class EffectPool
{
    private readonly int _additionalSize = 2;
    private readonly Queue<EffectView> _objects = new Queue<EffectView>();
    private readonly IEffectFactory _effectFactory;
    private int _count = 30;

    public EffectPool(IEffectFactory effectFactory)
    {
        _effectFactory = effectFactory;
    }

    public List<EffectView> GetAll() =>
        _objects.ToList();

    public EffectView Pop(EffectTypeId effectTypeId)
    {
        if (_objects.Count <= 0)
        {
            CreateObjects(_count * (_additionalSize - 1), effectTypeId);
        }

        EffectView obj = _objects.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Push(EffectView obj)
    {
        obj.gameObject.SetActive(false);
        _objects.Enqueue(obj);
    }

    private void CreateObjects(int count, EffectTypeId effectTypeId)
    {
        for (var i = 0; i < count; i++)
        {
            CreateObject(effectTypeId);
        }
    }

    private void CreateObject(EffectTypeId effectTypeId)
    {
        EffectView obj = _effectFactory.Create(effectTypeId);
        obj.gameObject.SetActive(false);
        _count++;
        _objects.Enqueue(obj);
    }
}
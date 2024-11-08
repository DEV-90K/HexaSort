using UnityEngine;

public abstract class StackSpawner : MonoBehaviour
{
    [SerializeField]
    protected StackHexagon hexagonStack;
    [SerializeField]
    protected Hexagon playerHexagon;

    protected virtual StackHexagon SpawnStack(Vector3 pos)
    {
        StackHexagon stackHexagon = PoolManager.Spawn<StackHexagon>(PoolType.STACK_HEXAGON, pos, Quaternion.identity);        
        return stackHexagon;
    }

    protected Hexagon SpawnHexagon(StackHexagon stack, Color color, Vector3 pos)
    {
        Hexagon insPlayerHexagon = PoolManager.Spawn<Hexagon>(PoolType.HEXAGON, pos, Quaternion.identity);
        insPlayerHexagon.OnSetUp();
        insPlayerHexagon.Color = color;
        insPlayerHexagon.SetParent(stack.transform);
        insPlayerHexagon.Configure(stack);
        stack.AddPlayerHexagon(insPlayerHexagon);
        return insPlayerHexagon;
    }

    public virtual StackHexagon Spawn(Transform tfPos) { return null; }
}

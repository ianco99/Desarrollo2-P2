using UnityEngine;
using kuznickiAttackables;

/// <summary>
/// TODO: SEPARATE ATTACK METHODS
/// Character interface
/// </summary>
public interface ICharacter
{
    public void Jump();
    public void Move(Vector3 movementDir);
    public void LaunchAttack(IAttackable target);
    public void CheckRebound(Collision other);
    public void AddForce(Vector3 direction, float speedBoost, ForceMode forceMode);
}

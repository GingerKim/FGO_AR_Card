using UnityEngine;

public class IKController : MonoBehaviour
{
    public Animator animator;
    public Transform target;

    private void OnAnimatorIK(int layerIndex)
    {
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, target.position);
    }
}
using UnityEngine;

public enum AnimationType
{
    Idle,
    Walk,
    Attack01,
    Attack02,
    Attack03,
    Hurt,
    Death
}

public static class AnimationManager
{
    public static void PlayAnimation(Animator animator, AnimationType type, Vector2 direction)
    {
        string animationName = "";

        if (direction == Vector2.up)
        {
            animationName = "Up";
        }
        else if (direction == Vector2.down)
        {
            animationName = "Down";
        }
        else if (direction == Vector2.left)
        {
            animationName = "Left";
        }
        else if (direction == Vector2.right)
        {
            animationName = "Right";
        }

        animationName += type.ToString();

        animator.Play(animationName);
        Debug.Log($"animation: {animationName}");
    }
}

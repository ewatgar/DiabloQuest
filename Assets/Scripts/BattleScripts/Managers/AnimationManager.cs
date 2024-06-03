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

        if (direction == Vector2.up) animationName = "Up";
        else if (direction == Vector2.down) animationName = "Down";
        else if (direction == Vector2.left) animationName = "Left";
        else if (direction == Vector2.right) animationName = "Right";
        else if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) animationName = "Right";
            else animationName = "Left";
        }
        else
        {
            if (direction.y > 0) animationName = "Up";
            else animationName = "Down";
        }

        animationName += type.ToString();
        animator.Play(animationName);
        //Debug.Log("Playing animation: " + animationName);
    }
}

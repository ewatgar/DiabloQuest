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
        else if (direction.x < 0 && direction.y > 0 || direction.x > 0 && direction.y > 0)
        {
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                animationName = "Up";
            else
                animationName = "Left";
        }
        else if (direction.x < 0 && direction.y < 0 || direction.x > 0 && direction.y < 0)
        {
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
                animationName = "Down";
            else
                animationName = "Right";
        }

        animationName += type.ToString();
        Debug.Log("Animation name before play: " + animationName);
        animator.Play(animationName);
        Debug.Log("Playing animation: " + animationName);
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BounceButton : Button
{
    public LeanTweenType downEase = LeanTweenType.linear;
    public LeanTweenType upEase = LeanTweenType.linear;
    public float pressedScale = 0.8f;
    public float transitionTime = 0.5f;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        if (!Application.isPlaying)
        {
            return;
        }
        var ease = LeanTweenType.linear;
        var scale = Vector3.one;
        switch (state)
        {
            case SelectionState.Normal:
                ease = upEase;
                break;
            case SelectionState.Highlighted:
                break;
            case SelectionState.Pressed:
                ease = downEase;
                scale *= pressedScale;
                break;
            case SelectionState.Disabled:
                break;
            default:
                break;
        }
        LeanTween.scale(GetComponent<RectTransform>(), scale, transitionTime).setEase(ease).setIgnoreTimeScale(false);
    }
}
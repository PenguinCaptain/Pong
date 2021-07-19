using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Debug = System.Diagnostics.Debug;

public class ScreenBoundary : SingletonMonoBehavior<ScreenBoundary>
{
    private readonly float _boundThickness = 3.0f;
    [SerializeField] private float m_emitDuration;
    [SerializeField] private float m_fadeDuration;

    public float LightTime => m_emitDuration + m_fadeDuration;

    private Light2D _leftLight, _rightLight;

    protected override void OnAwake()
    {
        Camera cam = Camera.main;
        Debug.Assert(cam != null);
        DrawBoundaries(cam);
        DrawBottomLines(cam);
    }

    public void EmitLight(PlayerType type)
    {
        var seq = DOTween.Sequence();
        if (type == PlayerType.Player1)
        {
            Tween t1 = DOTween.To(() => _leftLight.intensity, value => _leftLight.intensity = value,1,m_emitDuration);
            Tween t2 = DOTween.To(() => _leftLight.intensity, value => _leftLight.intensity = value,0,m_fadeDuration);
            seq.Append(t1).Append(t2);
        }
        else
        {
            Tween t1 = DOTween.To(() => _rightLight.intensity, value => _rightLight.intensity = value,1,m_emitDuration);
            Tween t2 = DOTween.To(() => _rightLight.intensity, value => _rightLight.intensity = value,0,m_fadeDuration);
            seq.Append(t1).Append(t2);
        }

        seq.Play();
    }

    private void DrawBoundaries(Camera cam)
    {
        var lowerBound = transform.Find("LowerBound").gameObject;
        BuildBounds(lowerBound, GetWorldPoint(0, 0, cam), GetWorldPoint(1, 0, cam));
        var upperBound = transform.Find("UpperBound").gameObject;
        BuildBounds(upperBound, GetWorldPoint(0, 1, cam), GetWorldPoint(1, 1, cam));
    }

    private void DrawBottomLines(Camera cam)
    {
        Vector2 downLeft = GetWorldPoint(0, 0, cam),
            upLeft = GetWorldPoint(0, 1, cam),
            downRight = GetWorldPoint(1, 0, cam),
            upRight = GetWorldPoint(1, 1, cam);

        var leftGoal = transform.Find("LeftGoal").gameObject;
        BuildBounds(leftGoal, downLeft, upLeft);
        _leftLight = BuildLight(leftGoal, downLeft, upLeft);

        var rightGoal = transform.Find("RightGoal").gameObject;
        BuildBounds(rightGoal, downRight, upRight);
        _rightLight = BuildLight(rightGoal, downRight, upRight);
    }

    private Light2D BuildLight(GameObject target, Vector2 a, Vector2 b)
    {
        if (target == null || !target.TryGetComponent<Light2D>(out var l2d)) return null;
        if (!(Math.Abs(a.x - b.x) < 0.001)) return null;
        var mult = a.x >= 0 ? 1 : -1;

        l2d.shapePath[0] = a;
        l2d.shapePath[1] = b;
        l2d.shapePath[2] = new Vector2(b.x + _boundThickness * mult, b.y);
        l2d.shapePath[3] = new Vector2(a.x + _boundThickness * mult, a.y);
        Array.Sort(l2d.shapePath, (l, r) => -l.x.CompareTo(r.x));
        if (l2d.shapePath[0].y>l2d.shapePath[1].y) Swap(ref l2d.shapePath[0], ref l2d.shapePath[1]);
        if (l2d.shapePath[2].y<l2d.shapePath[3].y) Swap(ref l2d.shapePath[2], ref l2d.shapePath[3]);
        l2d.enabled = true;
        return l2d;
    }

    private void Swap(ref Vector3 a, ref Vector3 b)
    {
        var tmp = a;
        a = b;
        b = tmp;
    }

    private void BuildBounds(GameObject target, Vector2 a, Vector2 b)
    {

        if (target!=null && target.TryGetComponent<BoxCollider2D>(out var boxCol))
        {
            if (Math.Abs(a.x - b.x) < 0.001)
            {
                int mult = a.x >= 0 ? 1 : -1;
                boxCol.size = new Vector2(_boundThickness, Math.Abs(a.y - b.y));
                boxCol.offset = new Vector2(a.x + _boundThickness * mult / 2, 0);
            }
            else if (Math.Abs(a.y - b.y) < 0.001)
            {
                int mult = a.y >= 0 ? 1 : -1;
                boxCol.size = new Vector2(Math.Abs(a.x - b.x), _boundThickness);
                boxCol.offset = new Vector2(0, a.y + _boundThickness * mult / 2);
            }
        }
    }


    private Vector2 GetWorldPoint(float x, float y, Camera cam)
    {
        var ret = new Vector2(x, y);
        ret = cam.ViewportToWorldPoint(ret);
        return ret;
    }
}
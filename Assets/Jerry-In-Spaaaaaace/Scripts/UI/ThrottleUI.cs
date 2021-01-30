using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ThrottleUI : MonoBehaviour
{
    public float minValue = -300f;
    public float maxValue = 300f;
    public RectTransform handleTrans;

    public void SetThrottleAlpha(float alpha)
    {
        if (handleTrans != null)
        {
            Vector2 pos = handleTrans.anchoredPosition;
            pos.y = Mathf.Lerp(minValue, maxValue, alpha);
            handleTrans.anchoredPosition = pos;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ThrottleUI))]
    public class Inspector : Editor
    {
        private ThrottleUI script;
        private float EditorAlpha;

        private void OnEnable()
        {
            script = (ThrottleUI)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorAlpha = EditorGUILayout.Slider("Editor Alpha", EditorAlpha, 0f, 1f);

            script.SetThrottleAlpha(EditorAlpha);
        }
    }

#endif
}

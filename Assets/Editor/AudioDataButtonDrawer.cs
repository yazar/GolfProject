using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR
[CustomEditor(typeof(AudioData))]
public class AudioDataButtonDrawer : Editor
{
    private AudioSource _testSource;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var script = (AudioData)target;

        if(GUILayout.Button("Play", GUILayout.Height(40)))
        {
            script.PlaySound(_testSource);
        }
        
    }
    private void OnEnable()
    {
        _testSource = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
    }

    private void OnDisable()
    {
        DestroyImmediate(_testSource.gameObject);
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class PlaySoundOnToggle : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private float volumeScale = 0.6f;
    [SerializeField] private AudioClip clip;

    private void Awake()
    {
        toggle.onValueChanged.AddListener(isOn =>
        {
            Vibration.Vibrate(80);
            MusicManager.Instance.PlaySoundEffect(clip, volumeScale);
        });
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        toggle ??= GetComponent<Toggle>();
    }
#endif
}

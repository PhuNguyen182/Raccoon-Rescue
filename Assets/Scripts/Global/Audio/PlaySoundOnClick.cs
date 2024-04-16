using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlaySoundOnClick : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private AudioClip clip;

    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            Vibration.Vibrate(80);
            MusicManager.Instance.PlaySoundEffect(clip);
        });
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        button ??= GetComponent<Button>();
    }
#endif
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    //Animation Event
    void Step()
    {
        GameManager.Instance.AudioManager.PlaySoundFx(SoundType.Step);
    }
}

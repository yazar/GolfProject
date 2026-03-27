using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class AudioManager : MonoBehaviour
{
   [SerializeField] private AudioMixer audioMixer;
   [SerializeField] private AudioLibrary audioLibrary;
   
   [Space(10)]
   [Header("Fx")]
   [SerializeField] private List<AudioSource> fxSourceList;
   
   [Space(10)]
   [Header("Music")]
   [SerializeField] private AudioSource musicSource;

   private List<AudioClip> _randomMusicClips = new List<AudioClip>();

   private Dictionary<SoundType, AudioData> _typeToData = new Dictionary<SoundType, AudioData>();

   private int _fxSourceIndex = 0;
   private int _musicIndex = 0;

   public SoundType testType;
   private void Start()
   {
      foreach (var data in audioLibrary.fxLibrary)
      {
         _typeToData[data.soundType] = data;
      }
      
      _randomMusicClips = new List<AudioClip>();
      foreach (var data in audioLibrary.musicLibrary)
      {
         _randomMusicClips.Add(data.audioClip);
      }
      _randomMusicClips.Shuffle();
      
      bool soundState = soundOn;
      soundOn = soundState;
      
      bool musicState = musicOn;
      musicOn = musicState;
      
      PlayRandomMusic();
   }

   [ContextMenu("PlayTestFx")]
   private void PlayTestFx()
   {
      PlaySoundFx(testType);
   }

   public void PlaySoundFx(SoundType soundType)
   {
      fxSourceList[_fxSourceIndex].clip = _typeToData[soundType].audioClip;
      fxSourceList[_fxSourceIndex].volume = _typeToData[soundType].volume;
      fxSourceList[_fxSourceIndex].pitch = _typeToData[soundType].pitch;
      fxSourceList[_fxSourceIndex].pitch +=
         (_typeToData[soundType].randomizePitch ? _typeToData[soundType].randomizationAmount : 0f);
                                           
      fxSourceList[_fxSourceIndex].Play();
      
      _fxSourceIndex = (_fxSourceIndex + 1) % fxSourceList.Count;
   }
   void PlayRandomMusic()
   {
      if(_musicIndex >= _randomMusicClips.Count)
      {
         _randomMusicClips.Shuffle();
         _musicIndex = 0;
      }

      var clip = _randomMusicClips[_musicIndex];

      musicSource.clip = clip;
      musicSource.Play();

      _musicIndex++;
      
      Invoke(nameof(PlayRandomMusic), clip.length);
   }

   public bool soundOn
   {
      get => PlayerPrefs.GetInt("SoundOff") == 0;
      private set
      {
         if (value)
         {
            audioMixer.ClearFloat("MasterVol");
         }
         else
         {
            audioMixer.SetFloat("MasterVol", -80.0f);
         }
         
         //GameManager.Instance.settingsMenuUI.SetSoundToggleState(value);
         PlayerPrefs.SetInt("SoundOff", value ? 0 : 1);
         PlayerPrefs.Save();
      }
   }
   
   public bool musicOn
   {
      get => PlayerPrefs.GetInt("MusicOff") == 0;
      private set
      {
         if (value)
         {
            audioMixer.ClearFloat("MusicVol");
         }
         else
         {
            audioMixer.SetFloat("MusicVol", -80.0f);
         }
         
         //GameManager.Instance.settingsMenuUI.SetMusicToggleState(value);
         PlayerPrefs.SetInt("MusicOff", value ? 0 : 1);
         PlayerPrefs.Save();
      }
   }
   
   public void SwitchSoundState()
   {
      soundOn = !soundOn;
   }
   
   public void SwitchMusicState()
   {
      musicOn = !musicOn;
   }
}


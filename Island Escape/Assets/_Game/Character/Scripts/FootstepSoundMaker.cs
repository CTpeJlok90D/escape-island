using System;
using UnityEngine;
using UnityEngine.Audio;

public class FootstepSoundMaker : MonoBehaviour
{
    [SerializeField] private AudioResource _defualtSound;
    [SerializeField] private AudioSource _source_PREFAB;
    [SerializeField] private CharacterViewInstance _characterViewInstance;
    [SerializeField] private AnimationCurve _volumePerVelocity;
    [SerializeField] private AnimationCurve _distancePerVelocity;
    [SerializeField] private float _muteTime = 0.25f;

    private bool _mute;
    private float _defualtMaxDistance;
    private float _defualtMinDistance;
    private float _defualtVolume;
    private Rigidbody _characterRigidbody;
    private AudioSource _sourceInstance;

    private void Awake()
    {
        _sourceInstance = Instantiate(_source_PREFAB, transform);
        
        _defualtMaxDistance = _sourceInstance.maxDistance;
        _defualtMinDistance = _sourceInstance.minDistance;
        _defualtVolume = _sourceInstance.volume;
        
        CharacterInstanceReference characterPawn = _characterViewInstance.LinkedCharacter as CharacterInstanceReference;
    
        if (characterPawn == null)
        {
            Debug.LogError("CharacterInstanceReference is null");
            enabled = false;
            return;
        }
    
        _characterRigidbody = characterPawn.GetComponent<Rigidbody>();
    }

    public void OnFootstep()
    {
        if (_mute)
        {
            return;
        }
        
        float moveSpeed = _characterRigidbody.linearVelocity.magnitude;
        float distance = _volumePerVelocity.Evaluate(moveSpeed);
        float volume = _distancePerVelocity.Evaluate(moveSpeed);

        _sourceInstance.maxDistance = distance * _defualtMaxDistance;
        _sourceInstance.minDistance = distance * _defualtMinDistance;
        _sourceInstance.volume = _defualtVolume * volume;

        _sourceInstance.resource = _defualtSound;
        _sourceInstance.Play();
        SilenceTime();
    }

    private async void SilenceTime()
    {
        try
        {
            _mute = true;
            await Awaitable.WaitForSecondsAsync(_muteTime);
            _mute = false;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_characterViewInstance == null)
        {
            _characterViewInstance = GetComponent<CharacterViewInstance>();
        }
    }
#endif
}
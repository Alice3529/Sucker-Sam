using System;
using _Scripts.Canvas_Scripts;
using _Scripts.Collectables;
using _Scripts.Player;
using UnityEngine;

namespace _Scripts.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        
        public PlayerMovememt _Player;
        public GameCollectables _Collectables;
        public StartGameTimer _StartGameTimer;
        
        private static SoundManager _instance;
        public static SoundManager Instance => _instance;

        private AudioSource _audioSource;
        
        public AudioClip OnConsumedCollectable;
        public AudioClip OnConsumedEnemy;
        public AudioClip OnDiedBy;
        public AudioClip OnIsPoweredUpStarted;
        public AudioClip OnIsPoweredUpChanged;
        public AudioClip OnIsPoweredUpTimeoutClose;
        public AudioClip OnIsPoweredUpTimeout;
        public AudioClip OnQuickTransition;

        public AudioClip OnGameWin;
        public AudioClip OnCollectablesDecreased;
        public AudioClip OnCollectablesIncreased;

        public AudioClip OnStartGameTimerTicked;
        public AudioClip OnGameStarted;

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            if (!_instance)
                _instance = this;
            else
            {
                Destroy(this);
            }

            _Player = TryToFindObject(_Player);
            _Collectables = TryToFindObject(_Collectables);
            _StartGameTimer = TryToFindObject(_StartGameTimer);

            RegisterForEvents();

        }

        private T TryToFindObject<T>(T currentValue) where T : MonoBehaviour
        {
            if (currentValue == null)
                currentValue = FindObjectOfType<T>();

            if (currentValue == null)
                throw new Exception(
                    "Please assign value, or put them in the scene othewrise Player can not play sound on player events");

            return currentValue;
        }

        private void RegisterForEvents()
        {
            _Player.OnIsPoweredUpStarted += PlayOnPoweredUpStarted;
            _Player.OnIsPoweredUpSecondsChanged += PlayOnPoweredUpTimeChanged;
            _Player.OnIsPoweredUpTimeout += PlayOnPoweredUpTimeout;
            _Player.OnConsumedCollectable += PlaySoundOnConsumedCollectable;
            _Player.OnConsumedEnemy += PlaySoundOnConsumedEnemy;
            _Player.OnDiedBy += PlaySoundOnDiedBy;
            _Player.OnChangedDirection += PlaySoundOnChangedTransition;

            _Collectables.OnNoCollectables += PlaySoundOnNoCollectables;
            _Collectables.OnCollectablesRemainingIncreased += PlaySoundOnCollectablesRemainingIncreased;
            _Collectables.OnCollectablesRemainingDecreased += PlaySoundOnCollectablesRemainingDecreased;

            _StartGameTimer.OnStartGameTimerChanged += StartGameTimerChanged;
        }

        private void StartGameTimerChanged(int obj)
        {
            if (obj>0)  _audioSource.PlayOneShot(OnStartGameTimerTicked);
            if (obj==0) _audioSource.PlayOneShot(OnGameStarted);
        }

        
        private void PlaySoundOnCollectablesRemainingDecreased(int obj)
        {
            _audioSource.PlayOneShot(OnCollectablesDecreased);
        }

        private void PlaySoundOnCollectablesRemainingIncreased(int obj)
        {
            _audioSource.PlayOneShot(OnCollectablesIncreased);
        }

        private void PlaySoundOnNoCollectables()
        {
            _audioSource.PlayOneShot(OnGameWin);
        }


        private void PlayOnPoweredUpTimeChanged(bool arg1, int arg2)
        {
            if (arg2 < 3)
                _audioSource.PlayOneShot(OnIsPoweredUpTimeoutClose);
            else
                _audioSource.PlayOneShot(OnIsPoweredUpChanged);
        }

        private void PlaySoundOnChangedTransition()
        {
            _audioSource.PlayOneShot(OnQuickTransition);
        }

        private void OnDestroy()
        {
            _Player.OnIsPoweredUpStarted -= PlayOnPoweredUpStarted;
            _Player.OnIsPoweredUpTimeout -= PlayOnPoweredUpTimeout;
            _Player.OnConsumedCollectable -= PlaySoundOnConsumedCollectable;
            _Player.OnConsumedEnemy -= PlaySoundOnConsumedEnemy;
            _Player.OnDiedBy -= PlaySoundOnDiedBy;
        }

        private void PlaySoundOnConsumedCollectable(ICollectable obj)
        {
            _audioSource.PlayOneShot(OnConsumedCollectable);
        }

        private void PlaySoundOnConsumedEnemy(Enemy.Enemy obj)
        {
            _audioSource.PlayOneShot(OnConsumedEnemy);
        }

        private void PlaySoundOnDiedBy(Enemy.Enemy obj)
        {
            _audioSource.PlayOneShot(OnDiedBy);
        }

        private void PlayOnPoweredUpStarted(bool arg1, float arg2)
        {
            _audioSource.PlayOneShot(OnIsPoweredUpStarted);
        }
        
        private void PlayOnPoweredUpTimeout(bool arg1, float arg2)
        {
            _audioSource.PlayOneShot(OnIsPoweredUpTimeout);
        }
    }
}

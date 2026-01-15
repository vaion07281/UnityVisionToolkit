using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace UnityVisionToolkit.Runtime
{
    /// <summary>
    /// Manages audio playback for background music and sound effects.
    /// Inherits from Singleton to ensure a single instance persists across scenes.
    /// </summary>
    public class AudioManager : Singleton<AudioManager>
    {
        private AudioSource _musicSource;
        private ObjectPool<AudioSource> _sfxPool;

        // Pool settings
        private const int DefaultPoolCapacity = 10;
        private const int MaxPoolSize = 60;

        /// <summary>
        /// Initializes the AudioManager, setting up the music source and SFX pool.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Setup Music Source
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;

            // Setup SFX Pool
            _sfxPool = new ObjectPool<AudioSource>(
                createFunc: OnCreateSFXSource,
                actionOnGet: OnGetSFXSource,
                actionOnRelease: OnReleaseSFXSource,
                actionOnDestroy: OnDestroySFXSource,
                collectionCheck: true,
                defaultCapacity: DefaultPoolCapacity,
                maxSize: MaxPoolSize
            );
        }

        /// <summary>
        /// Plays background music.
        /// If the same clip is already playing, it does nothing.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="volume">The volume (0.0 to 1.0).</param>
        public void PlayMusic(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] Attempted to play null music clip.");
                return;
            }

            // Do nothing if the same song is already playing
            if (_musicSource.clip == clip && _musicSource.isPlaying)
            {
                return;
            }

            _musicSource.clip = clip;
            _musicSource.volume = volume;
            _musicSource.Play();
        }

        /// <summary>
        /// Plays a sound effect once.
        /// Retrives an AudioSource from the pool, plays the clip, and returns it to the pool when finished.
        /// </summary>
        /// <param name="clip">The audio clip to play.</param>
        /// <param name="volume">The volume (0.0 to 1.0).</param>
        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null)
            {
                Debug.LogWarning("[AudioManager] Attempted to play null SFX clip.");
                return;
            }

            AudioSource source = _sfxPool.Get();
            source.clip = clip;
            source.volume = volume;
            source.spatialBlend = 0f; // 2D Sound
            source.Play();

            StartCoroutine(ReleaseToPoolAfterDelay(source, clip.length));
        }

        /// <summary>
        /// Waits for the clip to finish playing, then returns the AudioSource to the pool.
        /// </summary>
        private IEnumerator ReleaseToPoolAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            _sfxPool.Release(source);
        }

        // --- Pool Callbacks ---

        private AudioSource OnCreateSFXSource()
        {
            GameObject obj = new GameObject("SFX_Source");
            obj.transform.SetParent(transform);
            AudioSource source = obj.AddComponent<AudioSource>();
            return source;
        }

        private void OnGetSFXSource(AudioSource source)
        {
            source.gameObject.SetActive(true);
        }

        private void OnReleaseSFXSource(AudioSource source)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
        }

        private void OnDestroySFXSource(AudioSource source)
        {
            if (source != null)
            {
                Destroy(source.gameObject);
            }
        }
    }
}

using UnityEngine;

namespace Firebase
{
    /// <summary>
    /// Singleton object with DontDestroyOnLoad flag automatically set.
    /// </summary>
    public abstract class FirebaseMonoSingleton<T> : AFirebaseSingleton<T> where T : FirebaseMonoSingleton<T>
    {
        public override string TypeName
        {
            get
            {
                return "FirebaseMonoSingleton";
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (IsMarkedForDestroy)
            {
                return;
            }

            if (transform.parent == null)
            {
                DontDestroyOnLoad(this);
            }
        }

        protected override void DestroyInstance()
        {
            base.DestroyInstance();

            Destroy(this.gameObject);
        }

        public static T GetOrBuildInstance()
        {
            // Instance requiered for the first time, we look for it
            if (instance == null)
            {
                instance = TryFindInstance();

                // Object not found, we create a temporary one
                if (instance == null)
                {
                    Debug.Log("Creating MonoSingleton instance of type : " + typeof(T).ToString());
                    instance = new GameObject("Generated " + typeof(T).ToString()).AddComponent<T>();

                    // Problem during the creation, this should not happen
                    if (instance == null)
                    {
                        Debug.LogError("Problem during the creation of MonoSingleton of type : " + typeof(T).ToString());
                    }
                }
            }

            return instance;
        }

        public new static T GetInstance
        {
            get
            {
                return GetOrBuildInstance();
            }
        }
    }
}

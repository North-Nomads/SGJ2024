using UnityEngine.SceneManagement;
using UnityEngine;

namespace SGJ.Infrastructure
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initiailze()
        {
            // Initialize default service locator.
            ServiceLocator.Initiailze();

            // Register all your services next.
            // ServiceLocator.Current.Register<IMyGameServiceA>(new MyGameServiceA());

            // Application is ready to start, load your main scene.
            SceneManager.LoadSceneAsync(1);
        }
    }
}
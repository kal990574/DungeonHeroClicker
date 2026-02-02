using System.Runtime.InteropServices;

namespace _01.Scripts.Core.Utils
{
    public static class WebGLFileSync
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncFileSystem();
#endif

        public static void Sync()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SyncFileSystem();
#endif
        }
    }
}
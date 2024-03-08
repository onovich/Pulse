namespace MortiseFrame.Pulse {

    public static class PLog {
        public static void Log(string msg) {
            UnityEngine.Debug.Log(msg);
        }

        public static void LogWarning(string msg) {
            UnityEngine.Debug.LogWarning(msg);
        }

        public static void LogError(string msg) {
            UnityEngine.Debug.LogError(msg);
        }

        public static void LogException(System.Exception e) {
            UnityEngine.Debug.LogException(e);
        }
    }
}
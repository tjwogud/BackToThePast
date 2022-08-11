using UnityModManagerNet;

namespace BackToThePast.Utils
{
    public static class LoggerExtensions
    {
        public static void Log(this UnityModManager.ModEntry.ModLogger logger, object obj)
        {
            logger.Log(obj?.ToString());
        }
    }
}

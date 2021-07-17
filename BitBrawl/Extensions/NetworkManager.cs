using RedGrin;

namespace BitBrawl.Extensions
{
    public static class NetworkManagerExtensions
    {
        public static uint GetTick(this NetworkManager networkManager, double latency = 0)
        {
            return (uint)((networkManager.ServerTime - latency) * GameCore.ServerTickRate);
        }
    }
}

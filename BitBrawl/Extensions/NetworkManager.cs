using RedGrin;

namespace BitBrawl.Extensions
{
    public static class NetworkManagerExtensions
    {
        public static uint GetTick(this NetworkManager networkManager)
        {
            return (uint)(networkManager.ServerTime * GameCore.ServerTickRate);
        }
    }
}

using UnityEngine.Networking;

namespace Game.Network
{
    public class GameNetworkManager : NetworkManager
    {
        public GameNetworkSynchronizer Synchronizer;

        public override void OnStartServer()
        {
            base.OnStartServer();
            Synchronizer.InitializeServer();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            Synchronizer.InitializeClient(conn);
        }

        public override void OnStopClient()
        {
            base.OnStopClient();
            Synchronizer.Disconnect();
        }
    }
}
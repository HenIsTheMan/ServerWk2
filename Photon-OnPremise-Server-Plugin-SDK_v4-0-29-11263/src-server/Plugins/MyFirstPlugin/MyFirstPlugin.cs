using Photon.Hive.Plugin;
using System.Collections.Generic;
using System.Text;

namespace MyFirstPlugin {
    public class MyFirstPlugin: PluginBase {
		public override string Name => "MyFirstPlugin"; //The reserved plugin names are "Default" and "ErrorPlugin"

		//private IPluginLogger pluginLogger;

		//public override bool SetupInstance(IPluginHost host, Dictionary<string, string> config, out string errorMsg)
		//{
		//	this.pluginLogger = host.CreateLogger(this.Name);
		//	return base.SetupInstance(host, config, out errorMsg);
		//}

		public override void OnCreateGame(ICreateGameCallInfo info) {
            this.PluginHost.LogInfo(string.Format("OnCreateGame {0} by user {1}", info.Request.GameId, info.UserId));
            info.Continue(); // same as base.OnCreateGame(info);
        }

		public override void OnRaiseEvent(IRaiseEventCallInfo info) {
			base.OnRaiseEvent(info);

			if(info.Request.EvCode == 1) {
				string request = Encoding.Default.GetString((byte[])info.Request.Data);
				string response = "Message Received: " + request;

				PluginHost.BroadcastEvent(
					target: ReciverGroup.All,
					senderActor: 0,
					targetGroup: 0,
					data: new Dictionary<byte, object>() { { (byte)245, response } },
					evCode: info.Request.EvCode,
					cacheOp: 0
				);
			}
		}
	}

    public class MyPluginFactory: IPluginFactory {
        public IGamePlugin Create(IPluginHost gameHost, string pluginName, Dictionary<string, string> config,
            out string errorMsg) {
            MyFirstPlugin plugin = new MyFirstPlugin();
            if(plugin.SetupInstance(gameHost, config, out errorMsg)) {
                return plugin;
            }
            return null;
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Oxide.Core;
using Oxide.Core.Plugins;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Notifycations", "Agamemnon", "1.0.0")]
    [Description("Provides realtime notifications to players about common server and player events.")]
    class Notifycations : RustPlugin
    {
        [PluginReference] private Plugin Notify, BetterChatMute;

        private const string permHideConnections = "notifycations.hideconnections";
        private const string permHideDisconnections = "notifycations.hidedisconnections";

        private ConfigData configData;
        private bool notifyLoaded = false;

        #region Oxide Hooks
        private void OnServerInitialized()
        {
            permission.RegisterPermission(permHideConnections, this);
            permission.RegisterPermission(permHideDisconnections, this);

            if (!LoadConfigVariables())
            {
                PrintError("ERROR: The config file is corrupt. Either fix or delete it and restart the plugin.");
                PrintError("ERROR: Unloading plugin.");
                Interface.Oxide.UnloadPlugin(this.Title);
                return;
            }

            if (Notify == null)
            {
                PrintWarning("The Notify plugin isn't loaded. Notification messages are suspended.");
                notifyLoaded = false;
            }
            else
            {
                notifyLoaded = true;
            }

            if (!configData.showBans) Unsubscribe(nameof(OnUserBanned));
            if (!configData.showUnbans) Unsubscribe(nameof(OnUserUnbanned));
            if (!configData.showKicks) Unsubscribe(nameof(OnUserKicked));
            if (!configData.showConnects) Unsubscribe(nameof(OnUserConnected));
            if (!configData.showDisconnects) Unsubscribe(nameof(OnPlayerDisconnected));

            if (BetterChatMute == null)
            {
                PrintWarning("The BetterChatMute plugin isn't loaded. Mute/unmute notification messages are suspended.");
            }
            else
            {
                if (!configData.showMutes)
                {
                    Unsubscribe(nameof(OnBetterChatMuted));
                    Unsubscribe(nameof(OnBetterChatTimeMuted));
                }
                if (!configData.showUnmutes)
                {
                    Unsubscribe(nameof(OnBetterChatUnmuted));
                    Unsubscribe(nameof(OnBetterChatMuteExpired));
                }
            }
        }

        private void OnUserConnected(IPlayer player)
        {
            if (!notifyLoaded) return;
            if (player.HasPermission(permHideConnections)) return;

            var country = Lang("Unknown");
            webrequest.Enqueue("http://ip-api.com/json/" + player.Address, null, (code, response) =>
            {
                if (code == 200 && response != null)
                    country = JsonConvert.DeserializeObject<Response>(response)?.Country;

                NotifyAll(configData.connectType, Lang("ConnectMessage",
                    new KeyValuePair<string, string>("player", player.Name),
                    new KeyValuePair<string, string>("steamid", player.Id),
                    new KeyValuePair<string, string>("country", country.ToString())));
            }, this);
        }

        private void OnPlayerDisconnected(BasePlayer player, string reason)
        {
            if (!notifyLoaded) return;
            if (permission.UserHasPermission(player.UserIDString, permHideDisconnections)) return;
            if (configData.hideFlyHackDisconnects && reason.Contains("FlyHack")) return;
            if (configData.hideTimeOutDisconnects && reason == "Timed Out") return;

            NotifyAll(configData.disconnectType, Lang("DisconnectMessage",
                new KeyValuePair<string, string>("player", player.displayName),
                new KeyValuePair<string, string>("steamid", player.UserIDString)));
        }

        private void OnUserBanned(string name, string id, string ipAddress, string reason)
        {
            if (!notifyLoaded) return;

            string targetName = "Unknown";
            IPlayer target = covalence.Players.FindPlayer(id);
            if (target != null) targetName = target.Name;

            NotifyAll(configData.banType, Lang("BanMessage",
                new KeyValuePair<string, string>("player", targetName),
                new KeyValuePair<string, string>("steamid", id),
                new KeyValuePair<string, string>("reason", reason)));
        }

        private void OnUserUnbanned(string name, string id, string ipAddress)
        {
            if (!notifyLoaded) return;

            NotifyAll(configData.unbanType, Lang("UnbanMessage",
                new KeyValuePair<string, string>("player", name),
                new KeyValuePair<string, string>("steamid", id)));
        }

        private void OnUserKicked(IPlayer player, string reason)
        {
            if (!notifyLoaded) return;
            if (configData.hideUnresponsiveKicks && reason == "Unresponsive") return;

            NotifyAll(configData.kickType, Lang("KickMessage",
                new KeyValuePair<string, string>("player", player.Name),
                new KeyValuePair<string, string>("steamid", player.Id),
                new KeyValuePair<string, string>("reason", reason)));
        }

        private void OnPluginLoaded(Plugin name)
        {
            if (name.ToString() == "Oxide.Plugins.Notify")
            {
                PrintWarning("The Notify plugin has been loaded. Notification messages will resume.");
                notifyLoaded = true;
            }
            else if (name.ToString() == "Oxide.Plugins.BetterChatMute")
            { 
                PrintWarning("The BetterChatMute plugin has been loaded. Mute/unmute notification messages will resume.");

                if (!configData.showMutes)
                {
                    Unsubscribe(nameof(OnBetterChatMuted));
                    Unsubscribe(nameof(OnBetterChatTimeMuted));
                }
                if (!configData.showUnmutes)
                {
                    Unsubscribe(nameof(OnBetterChatUnmuted));
                    Unsubscribe(nameof(OnBetterChatMuteExpired));
                }
            }
        }

        private void OnPluginUnloaded(Plugin name)
        {
            if (name.ToString() == "Oxide.Plugins.Notify")
            {
                PrintWarning("The Notify plugin has been unloaded. Notification messages are suspended.");
                notifyLoaded = false;
            }
            else if (name.ToString() == "Oxide.Plugins.BetterChatMute")
            { 
                PrintWarning("The BetterChatMute plugin has been unloaded. Mute/unmute notification messages are suspended.");
            }
        }
        #endregion

        #region Plugin Hooks
        private void OnBetterChatMuted(IPlayer target, IPlayer player, string reason)
        {
            NotifyAll(configData.muteType, Lang("MuteMessage",
                new KeyValuePair<string, string>("player", target.Name),
                new KeyValuePair<string, string>("steamid", target.Id),
                new KeyValuePair<string, string>("reason", reason)));
        }

        private void OnBetterChatTimeMuted(IPlayer target, IPlayer player, TimeSpan time, string reason)
        {
            NotifyAll(configData.muteType, Lang("TimeMuteMessage",
                new KeyValuePair<string, string>("player", target.Name),
                new KeyValuePair<string, string>("steamid", target.Id),
                new KeyValuePair<string, string>("time", FormatTime((TimeSpan) time)),
                new KeyValuePair<string, string>("reason", reason)));
        }

        private void OnBetterChatUnmuted(IPlayer target, IPlayer player)
        {
            NotifyAll(configData.unmuteType, Lang("UnmuteMessage",
                new KeyValuePair<string, string>("player", target.Name),
                new KeyValuePair<string, string>("steamid", target.Id)));
        }

        private void OnBetterChatMuteExpired(IPlayer target)
        {
            NotifyAll(configData.unmuteType, Lang("MuteExpiredMessage",
                new KeyValuePair<string, string>("player", target.Name),
                new KeyValuePair<string, string>("steamid", target.Id)));
        }
        #endregion

        #region Configuration
        private class ConfigData
        {
            [JsonProperty(PropertyName = "Show Connections")]
            public bool showConnects = true;

            [JsonProperty(PropertyName = "Connection Notify Type")]
            public int connectType = 0;

            [JsonProperty(PropertyName = "Show Disconnections")]
            public bool showDisconnects = true;

            [JsonProperty(PropertyName = "Disconnection Notify Type")]
            public int disconnectType = 1;

            [JsonProperty(PropertyName = "Show Bans")]
            public bool showBans = true;

            [JsonProperty(PropertyName = "Ban Notify Type")]
            public int banType = 1;

            [JsonProperty(PropertyName = "Show Unbans")]
            public bool showUnbans = true;

            [JsonProperty(PropertyName = "Unban Notify Type")]
            public int unbanType = 0;

            [JsonProperty(PropertyName = "Show Kicks")]
            public bool showKicks = true;

            [JsonProperty(PropertyName = "Kick Notify Type")]
            public int kickType = 1;

            [JsonProperty(PropertyName = "Show Mutes")]
            public bool showMutes = true;

            [JsonProperty(PropertyName = "Mute Notify Type")]
            public int muteType = 1;

            [JsonProperty(PropertyName = "Show Unmutes")]
            public bool showUnmutes = true;

            [JsonProperty(PropertyName = "Unmute Notify Type")]
            public int unmuteType = 0;

            [JsonProperty(PropertyName = "Hide Time Out Disconnects")]
            public bool hideTimeOutDisconnects = true;

            [JsonProperty(PropertyName = "Hide FlyHack Disconnects")]
            public bool hideFlyHackDisconnects = true;

            [JsonProperty(PropertyName = "Hide Unresponsive Kicks")]
            public bool hideUnresponsiveKicks = true;
        }

        private bool LoadConfigVariables()
        {
            try
            {
                configData = Config.ReadObject<ConfigData>();
            }
            catch
            {
                return false;
            }

            SaveConfig(configData);
            return true;
        }

        protected override void LoadDefaultConfig()
        {
            PrintWarning("Creating new config file.");
            configData = new ConfigData();
            SaveConfig(configData);
        }

        private void SaveConfig(ConfigData config)
        {
            Config.WriteObject(config, true);
        }
        #endregion

        #region Language
        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ConnectMessage"] = "<b>{player}</b> has connected from {country}",
                ["DisconnectMessage"] = "<b>{player}</b> has disconnected",
                ["BanMessage"] = "<b>{player}</b> was banned: {reason}",
                ["UnbanMessage"] = "<b>{player}</b> was unbanned",
                ["KickMessage"] = "<b>{player}</b> was kicked: {reason}",
                ["MuteMessage"] = "<b>{player}</b> was muted: {reason}",
                ["TimeMuteMessage"] = "<b>{player}</b> was muted for {time}: {reason}",
                ["MuteExpiredMessage"] = "<b>{player}'s</b> mute has expired",
                ["UnmuteMessage"] = "<b>{player}</b> was unmuted",
                ["Unknown"] = "unknown"
            }, this);
            lang.RegisterMessages(new Dictionary<string, string>
            {
                ["ConnectMessage"] = "<b>{player}</b> s'est connecté depuis {reason}",
                ["DisconnectMessage"] = "<b>{player}</b> s'est déconnecté",
                ["BanMessage"] = "<b>{player}</b> a été banni: {reason}",
                ["UnbanMessage"] = "<b>{player}</b> a été débanni",
                ["KickMessage"] = "<b>{player}</b> a été expulsé: {reason}",
                ["MuteMessage"] = "<b>{player}</b> a été mis en sourdine: {reason}",
                ["TimeMuteMessage"] = "<b>{player}</b> a été mis en sourdine pour {time}: {reason}",
                ["MuteExpiredMessage"] = "Le muet du joueur {player}</b> a expiré",
                ["UnmuteMessage"] = "<b>{player}</b> peux de nouveau parler",
                ["Unknown"] = "inconnu"
            }, this, "fr");
        }

        private string Lang(string key) => string.Format(lang.GetMessage(key, this));
        private string Lang(string key, params KeyValuePair<string, string>[] replacements)
        {
            var message = lang.GetMessage(key, this);

            foreach (var replacement in replacements)
                message = message.Replace($"{{{replacement.Key}}}", replacement.Value);

            return message;
        }
        #endregion

        #region Helper Functions
        private void NotifyAll(int type, string message)
        {
            if (!notifyLoaded) return;

            foreach (var activePlayer in BasePlayer.activePlayerList)
                Notify?.Call("SendNotify", activePlayer, type, message);
        }

        private string FormatTime(TimeSpan time)
        {
            var values = new List<string>();

            if (time.Days != 0)
                values.Add($"{time.Days} day(s)");

            if (time.Hours != 0)
                values.Add($"{time.Hours} hour(s)");

            if (time.Minutes != 0)
                values.Add($"{time.Minutes} minute(s)");

            if (time.Seconds != 0)
                values.Add($"{time.Seconds} second(s)");

            return values.ToSentence();
        }
        #endregion

        #region Helper Classes
        class Response
        {
            [JsonProperty("country")]
            public string Country { get; set; }
        }
        #endregion
    }
}

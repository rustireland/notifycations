# Notifycations
**Notifycations** is an [Oxide](https://umod.org/) plugin that provides realtime notifications to players on a Rust server about *connections*, *disconnections*, *mutes*/*unmutes*, *kicks*, and *bans*/*unbans*.

It uses the [Notify](https://codefling.com/plugins/notify) plugin to provide this information in popup form, keeping the main server chat uncluttered. *Mute*/*unmute* notifications are optionally available if the [BetterChatMute](https://umod.org/plugins/better-chat-mute) plugin is installed.
- Each notification type can be individually enabled or disabled in the configuration.
- The information displayed in each notification is fully customizable in the language file.
- Connection/disconnection notifications can be hidden for individual users or groups.
- "Noisy" disconnection messages such as "*Timed Out*", "*Unresponsive*", and "*FlyHack Violation*" can be hidden.
### Table of Contents  
- [Requirements](#requirements)  
- [Installation](#installation)  
- [Permissions](#permissions)  
- [Commands](#commands)  
- [Configuration](#configuration)  
- [Localization](#localization)  
- [Example Notifycations and Notify Configurations](#example-notifycations-and-notify-configurations)  
  * [Example Notifycations Configuration](#example-notifycations-configuration)
  * [Example Notify Configuration](#example-notify-configuration)  
- [Credits](#credits)
## Requirements
| Depends On | Works With |
| --- | --- |
| [Notify](https://codefling.com/plugins/notify) | [BetterChatMute](https://umod.org/plugins/better-chat-mute) |
## Installation
Download the plugin:
```bash
git clone https://github.com/rustireland/notifycations.git
```
Copy it to the Oxide plugins directory:
```bash
cp notifycations/Notifycations.cs oxide/plugins
```
Oxide will compile and load the plugin automatically.
## Permissions
This plugin uses the Oxide permission system. To assign a permission, use `oxide.grant <user or group> <name or steam id> <permission>`. To remove a permission, use `oxide.revoke <user or group> <name or steam id> <permission>`.
- `notifycations.hideconnections` - Prevents connection notifications being displayed for a user or group
- `notifycations.hidedisconnections` - Prevents disconnection notifications being displayed for a user or group
## Commands
This plugin doesn't provide any console or chat commands.
## Configuration
The settings and options can be configured in the `Notifycations.json` file under the `oxide/config` directory. The use of an editor and validator is recommended to avoid formatting issues and syntax errors.

When run for the first time, the plugin will create a default configuration file with all notifications *enabled*, and will make use of the two default **Notify** types ("*Notification*" and "*Error*").
```json
{
  "Show Connections": true,
  "Connection Notify Type": 0,
  "Show Disconnections": true,
  "Disconnection Notify Type": 1,
  "Show Bans": true,
  "Ban Notify Type": 1,
  "Show Unbans": true,
  "Unban Notify Type": 0,
  "Show Kicks": true,
  "Kick Notify Type": 1,
  "Show Mutes": true,
  "Mute Notify Type": 1,
  "Show Unmutes": true,
  "Unmute Notify Type": 0,
  "Hide Time Out Disconnects": true,
  "Hide FlyHack Disconnects": true,
  "Hide Unresponsive Kicks": true
}
```
## Localization
The default messages are in the `Notifycations.json` file under the `oxide/lang/en` directory. To add support for another language, create a new language folder (e.g. **de** for German) if not already created, copy the default language file to the new folder and then customize the messages.

In addition to the default variables, `{steamid}` is also available for use in each message.
```json
{
  "ConnectMessage": "<b>{player}</b> has connected from {country}",
  "DisconnectMessage": "<b>{player}</b> has disconnected",
  "BanMessage": "<b>{player}</b> was banned: {reason}",
  "UnbanMessage": "<b>{player}</b> was unbanned",
  "KickMessage": "<b>{player}</b> was kicked: {reason}",
  "MuteMessage": "<b>{player}</b> was muted: {reason}",
  "TimeMuteMessage": "<b>{player}</b> was muted for {time}: {reason}",
  "MuteExpiredMessage": "<b>{player}'s</b> mute has expired",
  "UnmuteMessage": "<b>{player}</b> was unmuted",
  "Unknown": "unknown"
}
```
## Example Notifycations and Notify Configurations
Example configurations have been provided in the git repository and reproduced below for both plugins.

The **Notify** configuration retains the two default types ("*Notification*" and "*Error*"), and adds a selection of new types suitable for use with **Notifycations**:
| Type | Purpose |
| --- | --- |
| 0 | Notification |
| 1 | Error |
| 2 | Connect |
| 3 | Disconnect |
| 4 | Ban |
| 5 | Unban |
| 6 | Kick |
| 7 | Mute |
| 8 | Unmute |

The **Notifycations** configuration then makes use of these types to provide customized messages for each notification:

![Example Notification Images](https://github.com/rustireland/notifycations/blob/master/examples.png?raw=true)

Copy both example configurations to the Oxide plugins directory:
```
cp notifycations/Notify.json oxide/config
cp notifycations/Notifycations.json oxide/config
```
Restart both plugins from the server console:
```
oxide.restart Notify
oxide.restart Notifycations
```
### Example Notifycations Configuration
Copy and paste the contents to `oxide/config/Notifycations.json`:
```json
{
  "Show Connections": true,
  "Connection Notify Type": 2,
  "Show Disconnections": true,
  "Disconnection Notify Type": 3,
  "Show Bans": true,
  "Ban Notify Type": 4,
  "Show Unbans": true,
  "Unban Notify Type": 5,
  "Show Kicks": true,
  "Kick Notify Type": 6,
  "Show Mutes": true,
  "Mute Notify Type": 7,
  "Show Unmutes": true,
  "Unmute Notify Type": 8,
  "Hide Time Out Disconnects": true,
  "Hide FlyHack Disconnects": true,
  "Hide Unresponsive Kicks": true
}
```
### Example Notify Configuration
Copy and paste the contents to `oxide/config/Notify.json`:
```json
{
  "Permission (example: notify.use)": "",
  "Height": 50.0,
  "Width": 260.0,
  "X Margin": 20.0,
  "Y Margin": 5.0,
  "Y Indent": -50.0,
  "Notify Cooldown": 10.0,
  "Notifications (type - settings)": {
    "0": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "!",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Notification",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "1": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "X",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Error",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "2": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "!",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Connect",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "3": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "X",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Disconnect",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "4": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "X",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Ban",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "5": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "!",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Unban",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "6": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "X",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Kick",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "7": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#FF6060",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "X",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Mute",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    },
    "8": {
      "Background Color": {
        "HEX": "#000000",
        "Opacity (0 - 100)": 98.0
      },
      "Enable Gradient?": true,
      "Gradient Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 35.0
      },
      "Sprite": "assets/content/ui/ui.background.transparent.linearltr.tga",
      "Material": "Assets/Icons/IconMaterial.mat",
      "Icon Color": {
        "HEX": "#4B68FF",
        "Opacity (0 - 100)": 100.0
      },
      "Icon Text": "!",
      "Icon Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Title Key (lang)": "Unmute",
      "Title Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 50.0
      },
      "Text Color": {
        "HEX": "#FFFFFF",
        "Opacity (0 - 100)": 100.0
      },
      "Fade Out": 1.0,
      "Fade In": 0.1,
      "Sound Effect (empty - disable)": "assets/bundled/prefabs/fx/notice/item.select.fx.prefab",
      "Image Settings": {
        "Enabled": false,
        "Image": "",
        "AnchorMin": "0 0",
        "AnchorMax": "0 0",
        "OffsetMin": "12.5 12.5",
        "OffsetMax": "37.5 37.5"
      }
    }
  }
}
```
# Credits
- [Agamemnon](https://github.com/agamemnon23) - Code, testing.
- [Black_demon6](https://github.com/TheBlackdemon6) - Initial concept, feature requests, testing, and French translations.

using ClientCore;
using Rampastring.Tools;
using Rampastring.XNAUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ClientGUI
{
    public enum LocaleKey
    {
        OK,
        Cancel,
        Yes,
        No,
        MainMenu,
        GameLobby,
        CnCNetLobby,

        MainMenuWithHotKey,
        CnCNetLobbyWithHotKey,
        PrivateMessageWithHotKey,
        OptionWithHotKey,

        Option_tabDisplay,
        Option_lblIngameResolution,
        Option_lblDetailLevel,
        Option_ddDetailLevel_Low,
        Option_ddDetailLevel_Medium,
        Option_ddDetailLevel_High,
        Option_lblLanguage,
        Option_lblRenderer,
        Option_chkWindowedMode,
        Option_chkBorderlessWindowedMode,
        Option_chkBackBufferInVRAM,
        Option_chkBackBufferInVRAM_l2,
        Option_chkBackBufferInVRAM_l3,
        Option_lblClientResolution,
        Option_ddClientResolution_prefer,
        Option_chkBorderlessClient,
        Option_lblClientTheme,

        Option_tabAudio,

        Option_tabGame,

        Option_tabCnCNet,

        Option_tabUpdater,

        Option_tabComponents,

        Option_btnCancel,
        Option_btnSave,

        Lobby_btnNewGame,
        Lobby_btnJoinGame,
        Lobby_PrivateMessage,
        Lobby_AddFriend,
        Lobby_RemoveFriend,
        Lobby_Block,
        Lobby_Unblock,
        Lobby_tbChatInput,
        Lobby_lblColor,
        Lobby_lblCurrentChannel,
        Lobby_lblOnline,
        Lobby_tbGameSearch,
        Lobby_gameBroadcastChannel,
        Lobby_ClientVersionTip,
        Lobby_BannedTip,
        Lobby_GameBannedTip,
        Lobby_JoiningGameTip,
        Lobby_CreatingGameTip,
        Lobby_UpdateTip,
        Lobby_UpdateTipChoice,
        Lobby_InvalidGameIndexError,
        Lobby_JoiningGameInProgressError,
        Lobby_GameExeRunningError,
        Lobby_LocalGameIDMismatchError,
        Lobby_GameLockedError,
        Lobby_NotInSaveGameError,
        Lobby_IncorrectPasswordError,

        Lobby_JoinChannelError,
        Lobby_JoinChannelErrorDesc,
        Lobby_JoinChatChannelErrorDesc,
        Lobby_JoinBroadcastChannelErrorDesc,
        Lobby_JoinChannelErrorDescFinal,

        Lobby_InviteHead,
        Lobby_InviteChoice,
        Lobby_InviteYes,
        Lobby_InviteNo,
        Lobby_InviteJoinFailed,
        Lobby_InviteJoinFailedDesc,
        Lobby_InviteFailed,

        Lobby_MsgBlocked,

        LANLobby_SocketFailed,
        LANLobby_SocketFailed_l2,
        LANLobby_SocketFailed_l3,
        LANLobby_DuplicateNameError,
        LANLobby_ConnectError,

        IRCColor_DefaultColor,
        IRCColor_DefaultColor2,
        IRCColor_LightBlue,
        IRCColor_ForestGreen,
        IRCColor_DarkRed,
        IRCColor_Red,
        IRCColor_MediumOrchid,
        IRCColor_Orange,
        IRCColor_Yellow,
        IRCColor_Lime,
        IRCColor_Turquoise,
        IRCColor_LightSkyBlue,
        IRCColor_RoyalBlue,
        IRCColor_Fuchsia,
        IRCColor_Gray,
        IRCColor_Gray2,

        LANColor_Gray,
        LANColor_Metalic,
        LANColor_Green,
        LANColor_LimeGreen,
        LANColor_GreenYellow,
        LANColor_Goldenrod,
        LANColor_Yellow,
        LANColor_Orange,
        LANColor_Red,
        LANColor_Pink,
        LANColor_Purple,
        LANColor_SkyBlue,
        LANColor_Blue,
        LANColor_Brown,
        LANColor_Teal,

        LogOut,
        Offine,
        Connecting,
        Connected,
        LANMode,
        NoCnCNetPlayer,
        CnCNetPlayerCounter,
    }
    /// <summary>
    /// A GUI creator that also includes ClientGUI's custom controls in addition
    /// to the controls of Rampastring.XNAUI.
    /// </summary>
    public class LocalizationLabel
    {
        public static LocalizationLabel Instance = new LocalizationLabel();

        private const string LANG_KEY = "Localisation";
        public string Language;
        public const string Default_Lang = "en-US";

        private LocaleDictionary locale = new LocaleDictionary();
        private readonly LocaleDictionary defaultLocale = new LocaleDictionary
        {
            [LocaleKey.OK] = "OK",
            [LocaleKey.Cancel] = "Cancel",
            [LocaleKey.Yes] = "Yes",
            [LocaleKey.No] = "No",

            [LocaleKey.MainMenu] = "Main Menu",
            [LocaleKey.GameLobby] = "Game Lobby",
            [LocaleKey.CnCNetLobby] = "CnCNet Lobby",

            [LocaleKey.MainMenuWithHotKey] = "Main Menu (F2)",
            [LocaleKey.CnCNetLobbyWithHotKey] = "CnCNet Lobby (F3)",
            [LocaleKey.PrivateMessageWithHotKey] = "Private Messages (F4)",
            [LocaleKey.OptionWithHotKey] = "Options (F12)",

            [LocaleKey.Option_tabDisplay] = "Display",
            [LocaleKey.Option_lblIngameResolution] = "In-game Resolution:",
            [LocaleKey.Option_lblDetailLevel] = "Detail Level:",
            [LocaleKey.Option_ddDetailLevel_Low] = "Low",
            [LocaleKey.Option_ddDetailLevel_Medium] = "Medium",
            [LocaleKey.Option_ddDetailLevel_High] = "High",
            [LocaleKey.Option_lblRenderer] = "Renderer:",
            [LocaleKey.Option_lblLanguage] = "Language:",
            [LocaleKey.Option_chkWindowedMode] = "Windowed Mode",
            [LocaleKey.Option_chkBorderlessWindowedMode] = "Borderless Windowed Mode",
            [LocaleKey.Option_chkBackBufferInVRAM] = "Back Buffer in Video Memory",
            [LocaleKey.Option_chkBackBufferInVRAM_l2] = "(lower performance, but is",
            [LocaleKey.Option_chkBackBufferInVRAM_l3] = "necessary on some systems)",
            [LocaleKey.Option_lblClientResolution] = "Client Resolution:",
            [LocaleKey.Option_ddClientResolution_prefer] = "(recommended)",
            [LocaleKey.Option_chkBorderlessClient] = "Fullscreen Client",
            [LocaleKey.Option_lblClientTheme] = "Client Theme:",

            [LocaleKey.Option_tabAudio] = "Audio",

            [LocaleKey.Option_tabGame] = "Game",

            [LocaleKey.Option_tabCnCNet] = "CnCNet",

            [LocaleKey.Option_tabUpdater] = "Updater",

            [LocaleKey.Option_tabComponents] = "Components",

            [LocaleKey.Option_btnCancel] = "Cancel",
            [LocaleKey.Option_btnSave] = "Save",

            [LocaleKey.Lobby_btnNewGame] = "Create Game",
            [LocaleKey.Lobby_btnJoinGame] = "Join Game",
            [LocaleKey.Lobby_PrivateMessage] = "Private Message",
            [LocaleKey.Lobby_AddFriend] = "Add Friend",
            [LocaleKey.Lobby_RemoveFriend] = "Remove Friend",
            [LocaleKey.Lobby_Block] = "Block",
            [LocaleKey.Lobby_Unblock] = "Unblock",
            [LocaleKey.Lobby_tbChatInput] = "Type here to chat...",
            [LocaleKey.Lobby_lblColor] = "YOUR COLOR:",
            [LocaleKey.Lobby_lblCurrentChannel] = "CURRENT CHANNEL:",
            [LocaleKey.Lobby_lblOnline] = "Online:",
            [LocaleKey.Lobby_tbGameSearch] = "Filter by name, map, game mode, player...",
            [LocaleKey.Lobby_gameBroadcastChannel] = "Broadcast Channel",
            [LocaleKey.Lobby_ClientVersionTip] = "*** DTA CnCNet Client version {0} ***",
            [LocaleKey.Lobby_BannedTip] = "Cannot join channel {0}, you're banned!",
            [LocaleKey.Lobby_GameBannedTip] = "Cannot join game {0}, you've been banned by the game host!",
            [LocaleKey.Lobby_JoiningGameTip] = "Attempting to join game {0}...",
            [LocaleKey.Lobby_CreatingGameTip] = "Creating a game named {0}...",
            [LocaleKey.Lobby_UpdateTip] = "Update available",
            [LocaleKey.Lobby_UpdateTipChoice] = "An update is available. Do you want to perform the update now?",

            [LocaleKey.Lobby_InvalidGameIndexError] = "Invalid game index",
            [LocaleKey.Lobby_JoiningGameInProgressError] = "Cannot join game - joining game in progress",
            [LocaleKey.Lobby_GameExeRunningError] = "Cannot join game while the main game executable is running.",
            [LocaleKey.Lobby_LocalGameIDMismatchError] = "The selected game is for {0}!",
            [LocaleKey.Lobby_GameLockedError] = "The selected game is locked!",
            [LocaleKey.Lobby_NotInSaveGameError] = "You do not exist in the saved game!",
            [LocaleKey.Lobby_IncorrectPasswordError] = "Incorrect password!",

            [LocaleKey.Lobby_JoinChannelError] = "Error joining channels",
            [LocaleKey.Lobby_JoinChannelErrorDesc] = "Following problems were encountered when attempting to join channels for the currently set local game {0}:",
            [LocaleKey.Lobby_JoinChatChannelErrorDesc] = "- Chat channel info could not be found. No chat channel will be available for this game in Current Channel dropdown.",
            [LocaleKey.Lobby_JoinBroadcastChannelErrorDesc] = "- Broadcast channel info could not be found. Creating & hosting games will be disabled.",
            [LocaleKey.Lobby_JoinChannelErrorDescFinal] = "Please check that the local game is set correctly in client configuration, and if using a custom-defined game, that its channel info is set properly.",


            [LocaleKey.Lobby_InviteHead] = "GAME INVITATION",
            [LocaleKey.Lobby_InviteChoice] = "Join {0}?",
            [LocaleKey.Lobby_InviteYes] = "Yes",
            [LocaleKey.Lobby_InviteNo] = "No",
            [LocaleKey.Lobby_InviteJoinFailed] = "Failed to join",
            [LocaleKey.Lobby_InviteJoinFailedDesc] = "Unable to join {0}'s game. The game may be locked or closed.",
            [LocaleKey.Lobby_InviteFailed] = "{0} could not receive your invitation. They might be in game or only accepting invitations from friends. Ensure your game is unlocked and visible in the lobby before trying again.",

            [LocaleKey.Lobby_MsgBlocked] = "Message blocked from - {0}",


            [LocaleKey.LANLobby_SocketFailed] = "Creating LAN socket failed! Message:",
            [LocaleKey.LANLobby_SocketFailed_l2] = "Please check your firewall settings.",
            [LocaleKey.LANLobby_SocketFailed_l3] = "Also make sure that no other application is listening to traffic on UDP ports 1232 - 1234.",
            [LocaleKey.LANLobby_DuplicateNameError] = "Your name is already taken in the game.",
            [LocaleKey.LANLobby_ConnectError] = "Connecting to the game failed! Message: ",


            [LocaleKey.IRCColor_DefaultColor] = "Default color",
            [LocaleKey.IRCColor_DefaultColor2] = "Default color #2",
            [LocaleKey.IRCColor_LightBlue] = "Light Blue",
            [LocaleKey.IRCColor_ForestGreen] = "Green",
            [LocaleKey.IRCColor_DarkRed] = "Dark Red",
            [LocaleKey.IRCColor_Red] = "Red",
            [LocaleKey.IRCColor_MediumOrchid] = "Purple",
            [LocaleKey.IRCColor_Orange] = "Orange",
            [LocaleKey.IRCColor_Yellow] = "Yellow",
            [LocaleKey.IRCColor_Lime] = "Lime Green",
            [LocaleKey.IRCColor_Turquoise] = "Turquoise",
            [LocaleKey.IRCColor_LightSkyBlue] = "Sky Blue",
            [LocaleKey.IRCColor_RoyalBlue] = "Blue",
            [LocaleKey.IRCColor_Fuchsia] = "Pink",
            [LocaleKey.IRCColor_Gray] = "Gray",
            [LocaleKey.IRCColor_Gray2] = "Gray #2",

            [LocaleKey.LANColor_Gray] = "Gray",
            [LocaleKey.LANColor_Metalic] = "Metalic",
            [LocaleKey.LANColor_Green] = "Green",
            [LocaleKey.LANColor_LimeGreen] = "Lime Green",
            [LocaleKey.LANColor_GreenYellow] = "Green Yellow",
            [LocaleKey.LANColor_Goldenrod] = "Goldenrod",
            [LocaleKey.LANColor_Yellow] = "Yellow",
            [LocaleKey.LANColor_Orange] = "Orange",
            [LocaleKey.LANColor_Red] = "Red",
            [LocaleKey.LANColor_Pink] = "Pink",
            [LocaleKey.LANColor_Purple] = "Purple",
            [LocaleKey.LANColor_SkyBlue] = "Sky Blue",
            [LocaleKey.LANColor_Blue] = "Blue",
            [LocaleKey.LANColor_Brown] = "Brown",
            [LocaleKey.LANColor_Teal] = "Teal",

            [LocaleKey.LogOut] = "Log Out",
            [LocaleKey.Offine] = "OFFLINE",
            [LocaleKey.Connecting] = "CONNECTING...",
            [LocaleKey.Connected] = "CONNECTED",
            [LocaleKey.LANMode] = "LAN MODE",
            [LocaleKey.NoCnCNetPlayer] = "N/A",
            [LocaleKey.CnCNetPlayerCounter] = "PLAYERS ONLINE:",
        };

        public LocalizationLabel()
        {
            Initialize();
        }
        public LocaleValue this[LocaleKey key]
        {
            get
            {
                return GetLocaleValue(key);
            }
        }


        private void Initialize()
        {
            InitializeLocale();
        }

        private void InitializeLocale()
        {
            locale.Clear();
            foreach(LocaleKey key in Enum.GetValues(typeof(LocaleKey)))
            {
                if (!locale.ContainsKey(key))
                {
                    locale.Add(key, String.Empty);
                }
            }
            Language = UserINISettings.Instance.Language;
            GetLocaleFromIni();

        }

        public LocaleValue GetLocaleValue(LocaleKey key)
        {
            if (locale.ContainsKey(key) && !string.IsNullOrEmpty(locale[key]))
            {
                return locale[key];
            }
            else
            {
                return defaultLocale[key];
            }
            
        }

        protected virtual void GetLocaleFromIni()
        {
            if (File.Exists(ProgramConstants.GetLocalePath() + Language + ".ini"))
                GetINILocale(new CCIniFile(ProgramConstants.GetLocalePath() + Language + ".ini"));
            else if (File.Exists(ProgramConstants.GetLocalePath() + Default_Lang + ".ini"))
                GetINILocale(new CCIniFile(ProgramConstants.GetLocalePath() + Default_Lang + ".ini"));
        }

        /// <summary>
        /// Reads Locales from an INI file.
        /// </summary>
        protected virtual void GetINILocale(IniFile iniFile)
        {
            List<string> keys = iniFile.GetSectionKeys(LANG_KEY);

            if (keys != null)
            {
                foreach (string key in keys)
                {
                    if (Enum.TryParse(key, true, out LocaleKey localeKey))
                    {
                        locale[localeKey] = iniFile.GetStringValue(LANG_KEY, key, String.Empty);
                    }
                }
            }
        }

    }

    internal class LocaleDictionary : Dictionary<LocaleKey, LocaleValue>
    {
        public void Add(LocaleKey key, string value)
        {
            base.Add(key, value);
        }
    }

    public class LocaleValue
    {
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        private string _value;
        public LocaleValue(string value)
        {
            Value = value;
        }
        public string Format(object arg0)
        {
            try
            {
                return string.Format(Value, arg0);
            }
            catch (FormatException e)
            {
                Logger.Log(e.ToString());
                return Value;
            }
        }

        public string Format(object arg0, object arg1)
        {
            try
            {
                return string.Format(Value, arg0, arg1);
            }
            catch (FormatException e)
            {
                Logger.Log(e.ToString());
                return Value;
            }
        }
        public string Format(object arg0, object arg1, object arg2)
        {
            try
            {
                return string.Format(Value, arg0, arg1, arg2);
            }
            catch (FormatException e)
            {
                Logger.Log(e.ToString());
                return Value;
            }
        }
        public string Format(params object[] args)
        {
            try
            {
                return string.Format(Value, args);
            }
            catch (FormatException e)
            {
                Logger.Log(e.ToString());
                return Value;
            }
        }

        public static LocaleValue operator +(LocaleValue v1, string v2)
        {
            v1.Value += v2;
            return v1;
        }

        public static implicit operator LocaleValue(string v)
        {
            return new LocaleValue(v);
        }

        public static implicit operator string(LocaleValue v)
        {
            return v.Value;
        }
    }

    public static class LocalizationManager
    {

        //public static bool SetLanguage(string languageId)
        //{
        //    return 
        //}
        public static Dictionary<string, LanguageInfo> GetAllLocaleTypeFromIni()
        {
            var LanguageList = new Dictionary<string, LanguageInfo>();
            LanguageList.Add(LocalizationLabel.Default_Lang, new LanguageInfo("English(Default)",false));

            var dirInfo = new DirectoryInfo(ProgramConstants.GetLocalePath());
            var dirList = dirInfo.GetFiles("*.ini", SearchOption.AllDirectories).ToList();
            foreach (var dir in dirList)
            {
                var dirFullName = dir.FullName;
                var dirName = Path.GetFileNameWithoutExtension(dirFullName); 
                var iniFile = new CCIniFile(dirFullName);
                List<string> keys = iniFile.GetSectionKeys(dirName);
                if (dirName == LocalizationLabel.Default_Lang)
                {
                    LanguageList[LocalizationLabel.Default_Lang] = new LanguageInfo(iniFile.GetStringValue(dirName, "UIName", "English") + "(Default)", false);
                }
                else if (keys != null)
                {
                    var langInfo = new LanguageInfo(iniFile.GetStringValue(dirName, "UIName", String.Empty), iniFile.GetBooleanValue(dirName, "Hidden", false));
                    LanguageList.Add(dirName, langInfo);
                }
            }
            return LanguageList;
        }
        public static LocaleValue Lang(this LocaleKey key)  => LocalizationLabel.Instance?.GetLocaleValue(key);
    }

    public class LanguageInfo
    {
        public bool Hidden { get; }
        public string UIName { get; }
        public LanguageInfo(string name, bool hidden)
        {
            UIName = name;
            Hidden = hidden;
        }
    }
}

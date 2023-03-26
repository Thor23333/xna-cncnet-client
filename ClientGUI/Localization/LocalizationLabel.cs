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
    /// <summary>
    /// A GUI creator that also includes ClientGUI's custom controls in addition
    /// to the controls of Rampastring.XNAUI.
    /// </summary>
    public class LocalizationLabel
    {
        public static LocalizationLabel Instance = new LocalizationLabel();

        public string Language;
        public string Default_Label = LocaleKey.Default_Label.Lang();
        public static string newLinePH = "\\n";

        private LocaleDictionary locale = new LocaleDictionary();
        public static readonly LocaleDictionary defaultLocale = new LocaleDictionary
        {
            [LocaleKey.Default_Label] = "(Default)",

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

            [LocaleKey.Main_btnNewCampaign] = "Campaign",
            [LocaleKey.Main_btnLoadGame] = "Load Game",
            [LocaleKey.Main_btnSkirmish] = "Skirmish",
            [LocaleKey.Main_btnCnCNet] = "CnCNet",
            [LocaleKey.Main_btnLan] = "LAN",
            [LocaleKey.Main_btnOptions] = "Options",
            [LocaleKey.Main_btnMapEditor] = "Map Editor",
            [LocaleKey.Main_btnStatistics] = "Statistics",
            [LocaleKey.Main_btnCredits] = "View Credits",
            [LocaleKey.Main_btnExtras] = "Extras",
            [LocaleKey.Main_btnExit] = "Quit Game",
            [LocaleKey.Main_lblCnCNetStatus] = "Players Online: ",

            [LocaleKey.Option_msgboxDownloadInProgressCaption] = "Downloads in progress",
            [LocaleKey.Option_msgboxDownloadInProgressDesc] = "Optional component downloads are in progress. The downloads will be cancelled if you exit the Options menu." + newLinePH + newLinePH +
                                                              "Are you sure you want to continue?",
            [LocaleKey.Option_msgboxSaveFailCaption] = "Saving Settings Failed",
            [LocaleKey.Option_msgboxSaveFailDesc] = "Saving settings failed! Error message: {0}",
            [LocaleKey.Option_msgboxRestartCaption] = "Restart Required",
            [LocaleKey.Option_msgboxRestartDesc] = "The client needs to be restarted for some of the changes to take effect." + newLinePH + newLinePH +
                                                   "Do you want to restart now?",

            [LocaleKey.Option_tabDisplay] = "Display",
            [LocaleKey.Option_lblIngameResolution] = "In-game Resolution:",
            [LocaleKey.Option_lblDetailLevel] = "Detail Level:",
            [LocaleKey.Option_ddDetailLevel_Low] = "Low",
            [LocaleKey.Option_ddDetailLevel_Medium] = "Medium",
            [LocaleKey.Option_ddDetailLevel_High] = "High",
            [LocaleKey.Option_lblRenderer] = "Renderer:",
            [LocaleKey.Option_lblLanguage] = "Language:",
            [LocaleKey.Option_btnOutputDefaultLanguage] = "Output Default Localisation",
            [LocaleKey.Option_chkWindowedMode] = "Windowed Mode",
            [LocaleKey.Option_chkBorderlessWindowedMode] = "Borderless Windowed Mode",
            [LocaleKey.Option_chkBackBufferInVRAM] = "Back Buffer in Video Memory" + newLinePH +
                                                     "(lower performance, but is" + newLinePH +
                                                     "necessary on some systems)",
            [LocaleKey.Option_lblClientResolution] = "Client Resolution:",
            [LocaleKey.Option_ddClientResolution_prefer] = "(Default)",
            [LocaleKey.Option_chkBorderlessClient] = "Fullscreen Client",
            [LocaleKey.Option_lblClientTheme] = "Client Theme:",

            [LocaleKey.Option_tabAudio] = "Audio",
            [LocaleKey.Option_lblScoreVolume] = "Music Volume:",
            [LocaleKey.Option_lblSoundVolume] = "Sound Volume:",
            [LocaleKey.Option_lblVoiceVolume] = "Voice Volume:",
            [LocaleKey.Option_chkScoreShuffle] = "Shuffle Music",
            [LocaleKey.Option_lblClientVolume] = "Client Volume:",
            [LocaleKey.Option_chkMainMenuMusic] = "Main menu music",
            [LocaleKey.Option_chkStopMusicOnMenu] = "Don't play main menu music in lobbies",

            [LocaleKey.Option_tabGame] = "Game",
            [LocaleKey.Option_lblScrollRate] = "Scroll Rate:",
            [LocaleKey.Option_chkScrollCoasting] = "Scroll Coasting",
            [LocaleKey.Option_chkTargetLines] = "Target Lines",
            [LocaleKey.Option_chkTooltips] = "Tooltips",
            [LocaleKey.Option_lblPlayerName] = "Player Name*:",
            [LocaleKey.Option_chkShowHiddenObjects] = "Show Hidden Objects",
            [LocaleKey.Option_chkBlackChatBackground] = "Use black background for in-game chat messages",
            [LocaleKey.Option_chkAltToUndeploy] = "Undeploy units by holding Alt key instead of a regular move command",
            [LocaleKey.Option_lblNotice] = "* If you are currently connected to CnCNet, you need to log out and reconnect\\n"+
                                           "for your new name to be applied.",
            [LocaleKey.Option_btnConfigureHotkeys] = "Configure Hotkeys",

            [LocaleKey.Option_tabCnCNet] = "CnCNet",
            [LocaleKey.Option_chkPingUnofficialTunnels] = "Ping unofficial CnCNet tunnels",
            [LocaleKey.Option_chkWriteInstallPathToRegistry] = "Write game installation path to Windows" + newLinePH +
                                                               "Registry (makes it possible to join" + newLinePH +
                                                               "other games' game rooms on CnCNet)",
            [LocaleKey.Option_chkPlaySoundOnGameHosted] = "Play sound when a game is hosted",
            [LocaleKey.Option_chkNotifyOnUserListChange] = "Show player join / quit messages" + newLinePH + 
                                                           "on CnCNet lobby",

            [LocaleKey.Option_chkSkipLoginWindow] = "Skip login dialog",
            [LocaleKey.Option_chkPersistentMode] = "Stay connected outside of the CnCNet lobby",
            [LocaleKey.Option_chkConnectOnStartup] = "Connect automatically on client startup",
            [LocaleKey.Option_chkDiscordIntegration] = "Show detailed game info in Discord status",
            [LocaleKey.Option_chkAllowGameInvitesFromFriendsOnly] = "Only receive game invitations from friends",
            [LocaleKey.Option_lblFollowedGames] = "Show game rooms from the following games:",


            [LocaleKey.Option_tabUpdater] = "Updater",
            [LocaleKey.Option_lblDescription] = "To change download server priority, select a server from the list and" + newLinePH +
                                                "use the Move Up / Down buttons to change its priority.",
            [LocaleKey.Option_btnMoveUp] = "Move Up",
            [LocaleKey.Option_btnMoveDown] = "Move Down",
            [LocaleKey.Option_chkAutoCheck] = "Check for updates automatically",
            [LocaleKey.Option_btnForceUpdate] = "Force Update",
            [LocaleKey.Option_msgboxForceUpdateCaption] = "Force Update Confirmation",
            [LocaleKey.Option_msgboxForceUpdateDesc] = "WARNING: Force update will result in files being re-verified" + newLinePH +
                                                       "and re - downloaded.While this may fix problems with game" + newLinePH +
                                                       "files, this also may delete some custom modifications" + newLinePH +
                                                       "made to this installation. Use at your own risk!" + newLinePH + newLinePH +
                                                       "If you proceed, the options window will close and the" + newLinePH +
                                                       "client will proceed to checking for updates." + newLinePH + newLinePH +
                                                       "Do you really want to force update?" + newLinePH,

            [LocaleKey.Option_tabComponents] = "Components",
            [LocaleKey.Option_msgboxInstallCaption] = "Confirmation Required",
            [LocaleKey.Option_msgboxInstallDesc] = "To enable {0} the Client will download the necessary files to your game directory." + newLinePH + newLinePH +
                                                   "This will take an additional {1} of disk space, and the download may last" + newLinePH +
                                                   "from a few minutes to multiple hours depending on your Internet connection speed." + newLinePH + newLinePH +
                                                   "You will not be able to play during the download. Do you want to continue?",

            [LocaleKey.Option_msgboxDownloadFailCaption] = "Optional Component Download Failed",
            [LocaleKey.Option_msgboxDownloadFailDesc] = "Download of optional component {0} failed." + newLinePH +
                                                        "See client.log for details." + newLinePH + newLinePH +
                                                        "If this problem continues, please contact your mod's authors for support.",

            [LocaleKey.Option_msgboxDownloadCompleteCaption] = "Download Completed",
            [LocaleKey.Option_msgboxDownloadCompleteDesc] = "Download of optional component {0} completed succesfully.",

            [LocaleKey.HotKey_lblCategory] = "Category:",
            [LocaleKey.HotKey_lbCommand] = "Command",
            [LocaleKey.HotKey_lbShortcut] = "Shortcut",
            [LocaleKey.HotKey_lblCommandCaption] = "Command name",
            [LocaleKey.HotKey_lblDescription] = "Command description",
            [LocaleKey.HotKey_lblCurrentHotkey] = "Currently assigned hotkey:",
            [LocaleKey.HotKey_lblCurrentHotkeyValue] = "Current hotkey value",
            [LocaleKey.HotKey_lblNewHotkey] = "New hotkey:",
            [LocaleKey.HotKey_lblNewHotkeyValue] = "Press a key...",
            [LocaleKey.HotKey_lblCurrentlyAssignedTo] = "Currently assigned to:",
            [LocaleKey.HotKey_btnAssign] = "Assign Hotkey",
            [LocaleKey.HotKey_btnResetKey] = "Reset to Default",
            [LocaleKey.HotKey_lblDefaultHotkey] = "Default hotkey:",
            [LocaleKey.HotKey_btnResetAllKeys] = "Reset All Keys",
            [LocaleKey.HotKey_none] = "None",

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

            [LocaleKey.NotAvailable] = "Not Available",
            [LocaleKey.Install] = "Install",
            [LocaleKey.Downloading] = "Downloading",
            [LocaleKey.Update] = "Update",
            [LocaleKey.Uninstall] = "Uninstall",
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
            else if (File.Exists(ProgramConstants.GetLocalePath() + LocalizationManager.Default_Lang + ".ini"))
                GetINILocale(new CCIniFile(ProgramConstants.GetLocalePath() + LocalizationManager.Default_Lang + ".ini"));
        }

        /// <summary>
        /// Reads Locales from an INI file.
        /// </summary>
        protected virtual void GetINILocale(IniFile iniFile)
        {
            List<string> keys = iniFile.GetSectionKeys(LocalizationManager.LANG_KEY);

            if (keys != null)
            {
                foreach (string key in keys)
                {
                    if (Enum.TryParse(key, true, out LocaleKey localeKey))
                    {
                        locale[localeKey] = iniFile.GetStringValue(LocalizationManager.LANG_KEY, key, String.Empty);
                    }
                }
            }
        }

    }

}

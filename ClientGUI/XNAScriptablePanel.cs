using Microsoft.Xna.Framework;
using Rampastring.XNAUI.XNAControls;
using Rampastring.Tools;
using Rampastring.XNAUI;
using ClientCore;
using System.IO;
using System.Collections.Generic;
using System;

namespace ClientGUI
{
    /// <summary>
    /// An "extra panel" for modders that automatically
    /// changes its size to match the texture size.
    /// </summary>
    public class XNAScriptablePanel : XNAPanel
    {
        private const string GENERIC_WINDOW_INI = "GenericWindow.ini";
        private const string GENERIC_WINDOW_SECTION = "GenericWindow";

        public XNAScriptablePanel(WindowManager windowManager) : base(windowManager)
        {
            
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void ParseAttributeFromINI(IniFile iniFile, string key, string value)
        {
            if (key == "BackgroundTexture")
            {
                BackgroundTexture = AssetLoader.LoadTexture(value);

                if (new Point(Width, Height) == Point.Zero)
                {
                    ClientRectangle = new Rectangle(X, Y,
                        BackgroundTexture.Width, BackgroundTexture.Height);
                }

                return;
            }

            base.ParseAttributeFromINI(iniFile, key, value);
        }
        protected virtual void SetAttributesFromIni()
        {
            if (File.Exists(ProgramConstants.GetResourcePath() + Name + ".ini"))
                GetINIAttributes(new CCIniFile(ProgramConstants.GetResourcePath() + Name + ".ini"));
            else if (File.Exists(ProgramConstants.GetBaseResourcePath() + Name + ".ini"))
                GetINIAttributes(new CCIniFile(ProgramConstants.GetBaseResourcePath() + Name + ".ini"));
            else if (File.Exists(ProgramConstants.GetResourcePath() + GENERIC_WINDOW_INI))
                GetINIAttributes(new CCIniFile(ProgramConstants.GetResourcePath() + GENERIC_WINDOW_INI));
            else
                GetINIAttributes(new CCIniFile(ProgramConstants.GetBaseResourcePath() + GENERIC_WINDOW_INI));
        }

        /// <summary>
        /// Reads this window's attributes from an INI file.
        /// </summary>
        protected virtual void GetINIAttributes(IniFile iniFile)
        {

            List<string> keys = iniFile.GetSectionKeys(Name);

            if (keys != null)
            {
                foreach (string key in keys)
                    ParseAttributeFromINI(iniFile, key, iniFile.GetStringValue(Name, key, String.Empty));
            }
            else
            {
                keys = iniFile.GetSectionKeys(GENERIC_WINDOW_SECTION);

                if (keys != null)
                {
                    foreach (string key in keys)
                        ParseAttributeFromINI(iniFile, key, iniFile.GetStringValue(GENERIC_WINDOW_SECTION, key, String.Empty));
                }
            }

        }

    }
}

﻿using ClientCore;
using ClientCore.CnCNet5;
using ClientGUI;
using DTAConfig.OptionPanels;
using Microsoft.Xna.Framework;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using Updater;

namespace DTAConfig
{
    public class OptionsWindow : XNAWindow
    {
        public OptionsWindow(WindowManager windowManager, GameCollection gameCollection, XNAControl topBar) : base(windowManager)
        {
            this.gameCollection = gameCollection;
            this.topBar = topBar;
        }

        public event EventHandler OnForceUpdate;

        private XNAClientTabControl tabControl;

        private XNAOptionsPanel[] optionsPanels;
        private ComponentsPanel componentsPanel;

        private DisplayOptionsPanel displayOptionsPanel;
        private XNAControl topBar;

        private GameCollection gameCollection;

        public override void Initialize()
        {
            Name = "OptionsWindow";
            ClientRectangle = new Rectangle(0, 0, 576, 435);
            BackgroundTexture = AssetLoader.LoadTextureUncached("optionsbg.png");

            tabControl = new XNAClientTabControl(WindowManager);
            tabControl.Name = "tabControl";
            tabControl.ClientRectangle = new Rectangle(12, 12, 0, 23);
            tabControl.FontIndex = 1;
            tabControl.ClickSound = new EnhancedSoundEffect("button.wav");
            tabControl.AddTab(LocaleKey.Option_tabDisplay.Lang(), 92);
            tabControl.AddTab(LocaleKey.Option_tabAudio.Lang(), 92);
            tabControl.AddTab(LocaleKey.Option_tabGame.Lang(), 92);
            tabControl.AddTab(LocaleKey.Option_tabCnCNet.Lang(), 92);
            tabControl.AddTab(LocaleKey.Option_tabUpdater.Lang(), 92);
            tabControl.AddTab(LocaleKey.Option_tabComponents.Lang(), 92);
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            var btnCancel = new XNAClientButton(WindowManager);
            btnCancel.Name = "btnCancel";
            btnCancel.ClientRectangle = new Rectangle(Width - 104,
                Height - 35, 92, 23);
            btnCancel.Text = LocaleKey.Option_btnCancel.Lang();
            btnCancel.LeftClick += BtnBack_LeftClick;

            var btnSave = new XNAClientButton(WindowManager);
            btnSave.Name = "btnSave";
            btnSave.ClientRectangle = new Rectangle(12, btnCancel.Y, 92, 23);
            btnSave.Text = LocaleKey.Option_btnSave.Lang();
            btnSave.LeftClick += BtnSave_LeftClick;

            displayOptionsPanel = new DisplayOptionsPanel(WindowManager, UserINISettings.Instance);
            componentsPanel = new ComponentsPanel(WindowManager, UserINISettings.Instance);
            var updaterOptionsPanel = new UpdaterOptionsPanel(WindowManager, UserINISettings.Instance);
            updaterOptionsPanel.OnForceUpdate += (s, e) => { Disable(); OnForceUpdate?.Invoke(this, EventArgs.Empty); };

            optionsPanels = new XNAOptionsPanel[]
            {
                displayOptionsPanel,
                new AudioOptionsPanel(WindowManager, UserINISettings.Instance),
                new GameOptionsPanel(WindowManager, UserINISettings.Instance, topBar),
                new CnCNetOptionsPanel(WindowManager, UserINISettings.Instance, gameCollection),
                updaterOptionsPanel,
                componentsPanel
            };

            if (ClientConfiguration.Instance.ModMode || CUpdater.UPDATEMIRRORS == null || CUpdater.UPDATEMIRRORS.Count < 1)
            {
                tabControl.MakeUnselectable(4);
                tabControl.MakeUnselectable(5);
            }
            else if (CUpdater.CustomComponents == null || CUpdater.CustomComponents.Length < 1)
                tabControl.MakeUnselectable(5);

            foreach (var panel in optionsPanels)
            {
                AddChild(panel);
                panel.Load();
                panel.Disable();
            }

            optionsPanels[0].Enable();

            AddChild(tabControl);
            AddChild(btnCancel);
            AddChild(btnSave);

            base.Initialize();

            CenterOnParent();
        }

        /// <summary>
        /// Parses extra options defined by the modder
        /// from an INI file. Called from XNAWindow.SetAttributesFromINI.
        /// </summary>
        /// <param name="iniFile">The INI file.</param>
        protected override void GetINIAttributes(IniFile iniFile)
        {
            base.GetINIAttributes(iniFile);

            foreach (var panel in optionsPanels)
            {
                panel.ParseUserOptions(iniFile);
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var panel in optionsPanels)
            {
                panel.Disable();
            }

            optionsPanels[tabControl.SelectedTab].Enable();
        }

        private void BtnBack_LeftClick(object sender, EventArgs e)
        {
            if (CustomComponent.IsDownloadInProgress())
            {
                var msgBox = new XNAMessageBox(WindowManager, LocaleKey.Option_msgboxDownloadInProgressCaption.Lang(),
                    LocaleKey.Option_msgboxDownloadInProgressDesc.Lang(), XNAMessageBoxButtons.YesNo);
                msgBox.Show();
                msgBox.YesClickedAction = ExitDownloadCancelConfirmation_YesClicked;

                return;
            }

            WindowManager.SoundPlayer.SetVolume(Convert.ToSingle(UserINISettings.Instance.ClientVolume));
            Disable();
        }

        private void ExitDownloadCancelConfirmation_YesClicked(XNAMessageBox messageBox)
        {
            componentsPanel.CancelAllDownloads();
            WindowManager.SoundPlayer.SetVolume(Convert.ToSingle(UserINISettings.Instance.ClientVolume));
            Disable();
        }

        private void BtnSave_LeftClick(object sender, EventArgs e)
        {
            if (CustomComponent.IsDownloadInProgress())
            {
                var msgBox = new XNAMessageBox(WindowManager, LocaleKey.Option_msgboxDownloadInProgressCaption.Lang(),
                    LocaleKey.Option_msgboxDownloadInProgressDesc.Lang(), XNAMessageBoxButtons.YesNo);
                msgBox.Show();
                msgBox.YesClickedAction = SaveDownloadCancelConfirmation_YesClicked;

                return;
            }

            SaveSettings();
        }

        private void SaveDownloadCancelConfirmation_YesClicked(XNAMessageBox messageBox)
        {
            componentsPanel.CancelAllDownloads();

            SaveSettings();
        }

        private void SaveSettings()
        {
            bool restartRequired = false;

            try
            {
                foreach (var panel in optionsPanels)
                {
                    restartRequired = panel.Save() || restartRequired;
                }

                UserINISettings.Instance.SaveSettings();
            }
            catch (Exception ex)
            {
                Logger.Log("Saving settings failed! Error message: " + ex.Message);
                XNAMessageBox.Show(WindowManager, LocaleKey.Option_msgboxSaveFailCaption.Lang(),
                    LocaleKey.Option_msgboxSaveFailDesc.Lang(ex.Message)); 
            }

            Disable();

            if (restartRequired)
            {
                var msgBox = new XNAMessageBox(WindowManager, LocaleKey.Option_msgboxRestartCaption.Lang(),
                    LocaleKey.Option_msgboxRestartDesc.Lang(), XNAMessageBoxButtons.YesNo);
                msgBox.Show();
                msgBox.YesClickedAction = RestartMsgBox_YesClicked;
            }
        }

        private void RestartMsgBox_YesClicked(XNAMessageBox messageBox)
        {
            WindowManager.RestartGame();
        }

        public void RefreshSettings()
        {
            foreach (var panel in optionsPanels)
            {
                panel.Load();
                panel.Save();
            }

            UserINISettings.Instance.SaveSettings();
        }

        public void Open()
        {
            foreach (var panel in optionsPanels)
                panel.Load();

            componentsPanel.Open();

            Enable();
        }

        public void ToggleMainMenuOnlyOptions(bool enable)
        {
            foreach (var panel in optionsPanels)
            {
                panel.ToggleMainMenuOnlyOptions(enable);
            }
        }

        public void SwitchToCustomComponentsPanel()
        {
            foreach (var panel in optionsPanels)
            {
                panel.Disable();
            }

            tabControl.SelectedTab = 5;
        }

        public void InstallCustomComponent(int id)
        {
            componentsPanel.InstallComponent(id);
        }

        public void PostInit()
        {
#if !YR
            displayOptionsPanel.PostInit();
#endif
        }
    }
}

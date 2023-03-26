using ClientCore;
using ClientGUI;
using Microsoft.Xna.Framework;
using Rampastring.Tools;
using Rampastring.XNAUI;
using Rampastring.XNAUI.XNAControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Updater;

namespace DTAConfig.OptionPanels
{
    class ComponentsPanel : XNAOptionsPanel
    {
        public ComponentsPanel(WindowManager windowManager, UserINISettings iniSettings)
            : base(windowManager, iniSettings)
        {
        }

        List<XNAClientButton> installationButtons = new List<XNAClientButton>();

        bool downloadCancelled = false;

        public override void Initialize()
        {
            base.Initialize();

            Name = "ComponentsPanel";

            int componentIndex = 0;

            if (CUpdater.CustomComponents == null)
                return;

            foreach (CustomComponent c in CUpdater.CustomComponents)
            {
                string buttonText = LocaleKey.NotAvailable.Lang();

                if (File.Exists(ProgramConstants.GamePath + c.LocalPath))
                {
                    buttonText = LocaleKey.Uninstall.Lang();

                    if (c.LocalIdentifier != c.RemoteIdentifier)
                        buttonText = LocaleKey.Update.Lang();
                }
                else
                {
                    if (!string.IsNullOrEmpty(c.RemoteIdentifier))
                    {
                        buttonText = LocaleKey.Install.Lang();
                    }
                }

                var btn = new XNAClientButton(WindowManager);
                btn.Name = "btn" + c.ININame;
                btn.ClientRectangle = new Rectangle(Width - 145,
                    12 + componentIndex * 35, 133, 23);
                btn.Text = buttonText;
                btn.Tag = c;
                btn.LeftClick += Btn_LeftClick;

                var lbl = new XNALabel(WindowManager);
                lbl.Name = "lbl" + c.ININame;
                lbl.ClientRectangle = new Rectangle(12, btn.Y + 2, 0, 0);
                lbl.Text = c.GUIName;

                AddChild(btn);
                AddChild(lbl);

                installationButtons.Add(btn);

                componentIndex++;
            }
        }

        public override void Load()
        {
            base.Load();

            int componentIndex = 0;
            bool buttonEnabled = false;

            if (CUpdater.CustomComponents == null)
                return;

            foreach (CustomComponent c in CUpdater.CustomComponents)
            {
                string buttonText = LocaleKey.NotAvailable.Lang();

                if (File.Exists(ProgramConstants.GamePath + c.LocalPath))
                {
                    buttonText = LocaleKey.Uninstall.Lang();
                    buttonEnabled = true;

                    if (c.LocalIdentifier != c.RemoteIdentifier)
                        buttonText = LocaleKey.Update.Lang() + " (" + GetSizeString(c.RemoteSize) + ")";
                }
                else
                {
                    if (!string.IsNullOrEmpty(c.RemoteIdentifier))
                    {
                        buttonText = LocaleKey.Install.Lang() + " (" + GetSizeString(c.RemoteSize) + ")";
                        buttonEnabled = true;
                    }
                }

                installationButtons[componentIndex].Text = buttonText;
                installationButtons[componentIndex].AllowClick = buttonEnabled;

                componentIndex++;
            }
        }

        public override bool Save()
        {
            base.Save();

            return false;
        }

        private void Btn_LeftClick(object sender, EventArgs e)
        {
            var btn = (XNAClientButton)sender;

            var cc = (CustomComponent)btn.Tag;

            if (cc.IsBeingDownloaded)
                return;

            if (File.Exists(ProgramConstants.GamePath + cc.LocalPath))
            {
                if (cc.LocalIdentifier == cc.RemoteIdentifier)
                {
                    File.Delete(ProgramConstants.GamePath + cc.LocalPath);
                    btn.Text = LocaleKey.Install.Lang();
                    return;
                }

                btn.AllowClick = false;

                cc.DownloadFinished += cc_DownloadFinished;
                cc.DownloadProgressChanged += cc_DownloadProgressChanged;
                Thread thread = new Thread(cc.DownloadComponent);
                thread.Start();
            }
            else
            {
                var msgBox = new XNAMessageBox(WindowManager, LocaleKey.Option_msgboxInstallCaption.Lang(),
                    LocaleKey.Option_msgboxInstallDesc.Lang(cc.GUIName, GetSizeString(cc.RemoteSize)),
                    XNAMessageBoxButtons.YesNo);
                msgBox.Tag = btn;

                msgBox.Show();
                msgBox.YesClickedAction = MsgBox_YesClicked;
            }
        }

        private void MsgBox_YesClicked(XNAMessageBox messageBox)
        {
            var btn = (XNAClientButton)messageBox.Tag;
            btn.AllowClick = false;

            var cc = (CustomComponent)btn.Tag;

            cc.DownloadFinished += cc_DownloadFinished;
            cc.DownloadProgressChanged += cc_DownloadProgressChanged;
            Thread thread = new Thread(cc.DownloadComponent);
            thread.Start();
        }

        public void InstallComponent(int id)
        {
            var btn = installationButtons[id];
            btn.AllowClick = false;

            var cc = (CustomComponent)btn.Tag;

            cc.DownloadFinished += cc_DownloadFinished;
            cc.DownloadProgressChanged += cc_DownloadProgressChanged;
            Thread thread = new Thread(cc.DownloadComponent);
            thread.Start();
        }

        /// <summary>
        /// Called whenever a custom component download's progress is changed.
        /// </summary>
        /// <param name="c">The CustomComponent object.</param>
        /// <param name="percentage">The current download progress percentage.</param>
        private void cc_DownloadProgressChanged(CustomComponent c, int percentage)
        {
            WindowManager.AddCallback(new Action<CustomComponent, int>(HandleDownloadProgressChanged), c, percentage);
        }

        private void HandleDownloadProgressChanged(CustomComponent cc, int percentage)
        {
            percentage = Math.Min(percentage, 100);

            var btn = installationButtons.Find(b => object.ReferenceEquals(b.Tag, cc));
            btn.Text = LocaleKey.Downloading.Lang() + ".. " + percentage + "%";
        }

        /// <summary>
        /// Called whenever a custom component download is finished.
        /// </summary>
        /// <param name="c">The CustomComponent object.</param>
        /// <param name="success">True if the download succeeded, otherwise false.</param>
        private void cc_DownloadFinished(CustomComponent c, bool success)
        {
            WindowManager.AddCallback(new Action<CustomComponent, bool>(HandleDownloadFinished), c, success);
        }

        private void HandleDownloadFinished(CustomComponent cc, bool success)
        {
            cc.DownloadFinished -= cc_DownloadFinished;
            cc.DownloadProgressChanged -= cc_DownloadProgressChanged;

            var btn = installationButtons.Find(b => object.ReferenceEquals(b.Tag, cc));
            btn.AllowClick = true;

            if (!success)
            {
                if (!downloadCancelled)
                {
                    XNAMessageBox.Show(WindowManager, LocaleKey.Option_msgboxDownloadFailCaption.Lang(),
                        LocaleKey.Option_msgboxDownloadFailDesc.Lang(cc.GUIName));
                }

                btn.Text = LocaleKey.Install.Lang() + " (" + GetSizeString(cc.RemoteSize) + ")";

                if (File.Exists(ProgramConstants.GamePath + cc.LocalPath))
                    btn.Text = LocaleKey.Update.Lang() + " (" + GetSizeString(cc.RemoteSize) + ")";
            }
            else
            {
                XNAMessageBox.Show(WindowManager, LocaleKey.Option_msgboxDownloadCompleteCaption.Lang(),
                    LocaleKey.Option_msgboxDownloadCompleteDesc.Lang(cc.GUIName));
                btn.Text = LocaleKey.Uninstall.Lang();
            }
        }

        public void CancelAllDownloads()
        {
            Logger.Log("Cancelling all downloads.");

            downloadCancelled = true;

            if (CUpdater.CustomComponents == null)
                return;

            foreach (CustomComponent cc in CUpdater.CustomComponents)
            {
                if (cc.IsBeingDownloaded)
                    cc.StopDownload();
            }
        }

        public void Open()
        {
            downloadCancelled = false;
        }

        private string GetSizeString(long size)
        {
            if (size < 1048576)
            {
                return (size / 1024) + " KB";
            }
            else
            {
                return (size / 1048576) + " MB";
            }
        }
    }
}

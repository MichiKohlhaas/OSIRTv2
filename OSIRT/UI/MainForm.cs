﻿using ImageMagick;
using Ionic.Zip;
using Jacksonsoft;
using mshtml;
using OSIRT.Browser;
using OSIRT.Helpers;
using OSIRT.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSIRT
{
    public partial class MainForm : Form
    {

   
        public MainForm()
        {
            InitializeComponent();
            FormClosing += MainForm_FormClosing;

        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: re-enabled this when we can handle an open case
            //WaitWindow.Show(ClosingOperations, "Running OSIRT closing operations... Please wait");
        }

        private void ClosingOperations(object sender, WaitWindowEventArgs e)
        {
            //clear IE cache if required

            //zip container
            //TODO: make sure a case has actually been opened first
            e.Window.Message = "Archiving container.";
            using (ZipFile zip = new ZipFile())
            {
                //zip.Password = "123456";
                //zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                zip.AddDirectory(Constants.ContainerLocation, Constants.CaseContainerName);
                zip.Save(Path.Combine(Constants.CasePath, Constants.CaseContainerName + ".osr"));
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            FirstLoadPanel firstLoadPanel = new FirstLoadPanel();
            firstLoadPanel.NewCase_Click += firstLoadPanel_NewCase_Click;
            firstLoadPanel.LoadOldCase_Click += FirstLoadPanel_LoadOldCase_Click;
            Controls.Add(firstLoadPanel);
        }

        private void FirstLoadPanel_LoadOldCase_Click(object sender, EventArgs e)
        {
            string filenameWithPath = "";
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "OSR Files|*.osr";
                DialogResult result = openFileDialog.ShowDialog();
                if (result != DialogResult.OK)
                    return;

                filenameWithPath = openFileDialog.FileName;
            }

            Controls.Clear();
            LoadExistingCasePanel exisitingCasePanel = new LoadExistingCasePanel(new FileInfo(filenameWithPath));
            Controls.Add(exisitingCasePanel);
            exisitingCasePanel.PasswordCheckClick += ExisitingCasePanel_PasswordCheckClick;

        }

        private void ExisitingCasePanel_PasswordCheckClick(object sender, EventArgs e)
        {
            ShowBrowserPanel();
        }


        void firstLoadPanel_NewCase_Click(object sender, EventArgs e)
        {
            Controls.Clear();
            CaseDetailsPanel caseDetailsPanel = new CaseDetailsPanel();
            Controls.Add(caseDetailsPanel);
            caseDetailsPanel.NextClick += new EventHandler(caseDetailsPanel_NextClick);
        }

        protected void caseDetailsPanel_NextClick(object sender, EventArgs e)
        {
            ShowBrowserPanel();
        }

        private void ShowBrowserPanel()
        {
            Controls.Clear();
            BrowserPanel browserPanel = new BrowserPanel();
            Controls.Add(browserPanel);
            WindowState = FormWindowState.Maximized;
        }

    }
}

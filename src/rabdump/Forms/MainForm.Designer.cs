﻿namespace rabdump
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.generalPanel1 = new rabdump.GeneralPanel();
            this.tpDataSources = new System.Windows.Forms.TabPage();
            this.farmsPanel1 = new rabnet.FarmsPanel();
            this.tpArchiveJobs = new System.Windows.Forms.TabPage();
            this.archiveJobsPanel1 = new rabdump.ArchiveJobsPanel();
            this.tpInfo = new System.Windows.Forms.TabPage();
            this.scAbout = new System.Windows.Forms.SplitContainer();
            this.aboutPanel1 = new rabdump.AboutPanel();
            this.pbUpdate = new System.Windows.Forms.ProgressBar();
            this.btDloadUpdate = new System.Windows.Forms.Button();
            this.tbUpdateInfo = new System.Windows.Forms.TextBox();
            this.btCheckUpdate = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miShowMainForm = new System.Windows.Forms.ToolStripMenuItem();
            this.miRunRabnet = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.miJobStart = new System.Windows.Forms.ToolStripMenuItem();
            this.miRestore = new System.Windows.Forms.ToolStripMenuItem();
            this.miRemoteSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.miSendGlobRep = new System.Windows.Forms.ToolStripMenuItem();
            this.miUpdateKey = new System.Windows.Forms.ToolStripMenuItem();
            this.miCheckForUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button2 = new System.Windows.Forms.Button();
            this.btOK = new System.Windows.Forms.Button();
            this.tDumper = new System.Windows.Forms.Timer(this.components);
            this.tUpdater = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.tpDataSources.SuspendLayout();
            this.tpArchiveJobs.SuspendLayout();
            this.tpInfo.SuspendLayout();
            this.scAbout.Panel1.SuspendLayout();
            this.scAbout.Panel2.SuspendLayout();
            this.scAbout.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpGeneral);
            this.tabControl1.Controls.Add(this.tpDataSources);
            this.tabControl1.Controls.Add(this.tpArchiveJobs);
            this.tabControl1.Controls.Add(this.tpInfo);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(361, 370);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpGeneral
            // 
            this.tpGeneral.Controls.Add(this.generalPanel1);
            this.tpGeneral.Location = new System.Drawing.Point(4, 22);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tpGeneral.Size = new System.Drawing.Size(353, 344);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "Основное";
            // 
            // generalPanel1
            // 
            this.generalPanel1.Location = new System.Drawing.Point(6, 4);
            this.generalPanel1.MinimumSize = new System.Drawing.Size(340, 315);
            this.generalPanel1.Name = "generalPanel1";
            this.generalPanel1.Size = new System.Drawing.Size(340, 315);
            this.generalPanel1.TabIndex = 0;
            // 
            // tpDataSources
            // 
            this.tpDataSources.Controls.Add(this.farmsPanel1);
            this.tpDataSources.Location = new System.Drawing.Point(4, 22);
            this.tpDataSources.Name = "tpDataSources";
            this.tpDataSources.Size = new System.Drawing.Size(353, 344);
            this.tpDataSources.TabIndex = 1;
            this.tpDataSources.Text = "Фермы";
            // 
            // farmsPanel1
            // 
            this.farmsPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.farmsPanel1.Location = new System.Drawing.Point(0, 0);
            this.farmsPanel1.MinimumSize = new System.Drawing.Size(340, 315);
            this.farmsPanel1.Name = "farmsPanel1";
            this.farmsPanel1.Size = new System.Drawing.Size(340, 315);
            this.farmsPanel1.TabIndex = 0;
            // 
            // tpArchiveJobs
            // 
            this.tpArchiveJobs.Controls.Add(this.archiveJobsPanel1);
            this.tpArchiveJobs.Location = new System.Drawing.Point(4, 22);
            this.tpArchiveJobs.Name = "tpArchiveJobs";
            this.tpArchiveJobs.Size = new System.Drawing.Size(353, 344);
            this.tpArchiveJobs.TabIndex = 2;
            this.tpArchiveJobs.Text = "Расписания";
            // 
            // archiveJobsPanel1
            // 
            this.archiveJobsPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.archiveJobsPanel1.Location = new System.Drawing.Point(0, 0);
            this.archiveJobsPanel1.MinimumSize = new System.Drawing.Size(340, 315);
            this.archiveJobsPanel1.Name = "archiveJobsPanel1";
            this.archiveJobsPanel1.Size = new System.Drawing.Size(353, 344);
            this.archiveJobsPanel1.TabIndex = 0;
            // 
            // tpInfo
            // 
            this.tpInfo.Controls.Add(this.scAbout);
            this.tpInfo.Location = new System.Drawing.Point(4, 22);
            this.tpInfo.Name = "tpInfo";
            this.tpInfo.Size = new System.Drawing.Size(353, 344);
            this.tpInfo.TabIndex = 3;
            this.tpInfo.Text = "О программе";
            // 
            // scAbout
            // 
            this.scAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scAbout.Location = new System.Drawing.Point(0, 0);
            this.scAbout.Name = "scAbout";
            this.scAbout.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scAbout.Panel1
            // 
            this.scAbout.Panel1.Controls.Add(this.aboutPanel1);
            // 
            // scAbout.Panel2
            // 
            this.scAbout.Panel2.Controls.Add(this.pbUpdate);
            this.scAbout.Panel2.Controls.Add(this.btDloadUpdate);
            this.scAbout.Panel2.Controls.Add(this.tbUpdateInfo);
            this.scAbout.Panel2.Controls.Add(this.btCheckUpdate);
            this.scAbout.Size = new System.Drawing.Size(192, 74);
            this.scAbout.SplitterDistance = 40;
            this.scAbout.TabIndex = 0;
            // 
            // aboutPanel1
            // 
            this.aboutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutPanel1.Location = new System.Drawing.Point(0, 0);
            this.aboutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.aboutPanel1.MinimumSize = new System.Drawing.Size(340, 0);
            this.aboutPanel1.Name = "aboutPanel1";
            this.aboutPanel1.Size = new System.Drawing.Size(340, 40);
            this.aboutPanel1.TabIndex = 1;
            // 
            // pbUpdate
            // 
            this.pbUpdate.Location = new System.Drawing.Point(179, 3);
            this.pbUpdate.Name = "pbUpdate";
            this.pbUpdate.Size = new System.Drawing.Size(151, 23);
            this.pbUpdate.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbUpdate.TabIndex = 4;
            this.pbUpdate.Visible = false;
            // 
            // btDloadUpdate
            // 
            this.btDloadUpdate.Location = new System.Drawing.Point(179, 3);
            this.btDloadUpdate.Name = "btDloadUpdate";
            this.btDloadUpdate.Size = new System.Drawing.Size(151, 23);
            this.btDloadUpdate.TabIndex = 3;
            this.btDloadUpdate.Text = "Установить обновление";
            this.btDloadUpdate.UseVisualStyleBackColor = true;
            this.btDloadUpdate.Visible = false;
            this.btDloadUpdate.Click += new System.EventHandler(this.btDloadUpdate_Click);
            // 
            // tbUpdateInfo
            // 
            this.tbUpdateInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tbUpdateInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbUpdateInfo.Location = new System.Drawing.Point(8, 32);
            this.tbUpdateInfo.Multiline = true;
            this.tbUpdateInfo.Name = "tbUpdateInfo";
            this.tbUpdateInfo.Size = new System.Drawing.Size(337, 91);
            this.tbUpdateInfo.TabIndex = 2;
            this.tbUpdateInfo.Visible = false;
            // 
            // btCheckUpdate
            // 
            this.btCheckUpdate.Location = new System.Drawing.Point(22, 3);
            this.btCheckUpdate.Name = "btCheckUpdate";
            this.btCheckUpdate.Size = new System.Drawing.Size(151, 23);
            this.btCheckUpdate.TabIndex = 0;
            this.btCheckUpdate.Text = "Проверить обновление";
            this.btCheckUpdate.UseVisualStyleBackColor = true;
            this.btCheckUpdate.Click += new System.EventHandler(this.btCheckUpdate_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Резервные копии";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miShowMainForm,
            this.miRunRabnet,
            this.toolStripMenuItem3,
            this.miJobStart,
            this.miRestore,
            this.miRemoteSeparator,
            this.miSendGlobRep,
            this.miUpdateKey,
            this.miCheckForUpdate,
            this.toolStripSeparator2,
            this.exitMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 198);
            // 
            // miShowMainForm
            // 
            this.miShowMainForm.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.miShowMainForm.Name = "miShowMainForm";
            this.miShowMainForm.Size = new System.Drawing.Size(210, 22);
            this.miShowMainForm.Text = "Настройки";
            this.miShowMainForm.Click += new System.EventHandler(this.miShowMainForm_Click);
            // 
            // miRunRabnet
            // 
            this.miRunRabnet.Name = "miRunRabnet";
            this.miRunRabnet.Size = new System.Drawing.Size(210, 22);
            this.miRunRabnet.Text = "Запустить Miakro9.11";
            this.miRunRabnet.Click += new System.EventHandler(this.miRunRabnet_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(207, 6);
            // 
            // miJobStart
            // 
            this.miJobStart.Name = "miJobStart";
            this.miJobStart.Size = new System.Drawing.Size(210, 22);
            this.miJobStart.Text = "Резервировать";
            // 
            // miRestore
            // 
            this.miRestore.Name = "miRestore";
            this.miRestore.Size = new System.Drawing.Size(210, 22);
            this.miRestore.Text = "Восстановить";
            // 
            // miRemoteSeparator
            // 
            this.miRemoteSeparator.Name = "miRemoteSeparator";
            this.miRemoteSeparator.Size = new System.Drawing.Size(207, 6);
            // 
            // miSendGlobRep
            // 
            this.miSendGlobRep.Name = "miSendGlobRep";
            this.miSendGlobRep.Size = new System.Drawing.Size(210, 22);
            this.miSendGlobRep.Text = "Послать отчет на сервер";
            this.miSendGlobRep.Click += new System.EventHandler(this.miSendGlobRep_Click);
            // 
            // miUpdateKey
            // 
            this.miUpdateKey.Name = "miUpdateKey";
            this.miUpdateKey.Size = new System.Drawing.Size(210, 22);
            this.miUpdateKey.Text = "Обновить лицензию";
            this.miUpdateKey.Click += new System.EventHandler(this.miUpdateKey_Click);
            // 
            // miCheckForUpdate
            // 
            this.miCheckForUpdate.Name = "miCheckForUpdate";
            this.miCheckForUpdate.Size = new System.Drawing.Size(210, 22);
            this.miCheckForUpdate.Tag = "0";
            this.miCheckForUpdate.Text = "Проверить обновление";
            this.miCheckForUpdate.Click += new System.EventHandler(this.miCheckForUpdate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(207, 6);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(210, 22);
            this.exitMenuItem.Text = "Выход";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(193, 374);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btOK
            // 
            this.btOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(274, 374);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 2;
            this.btOK.Text = "Сохранить";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // tDumper
            // 
            this.tDumper.Interval = 60000;
            this.tDumper.Tick += new System.EventHandler(this.tDumper_Tick);
            // 
            // tUpdater
            // 
            this.tUpdater.Interval = 10800000;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(361, 409);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RabDump";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormNew_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormNew_FormClosed);
            this.Load += new System.EventHandler(this.MainFormNew_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.tpDataSources.ResumeLayout(false);
            this.tpArchiveJobs.ResumeLayout(false);
            this.tpInfo.ResumeLayout(false);
            this.scAbout.Panel1.ResumeLayout(false);
            this.scAbout.Panel2.ResumeLayout(false);
            this.scAbout.Panel2.PerformLayout();
            this.scAbout.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpGeneral;
        private System.Windows.Forms.TabPage tpDataSources;
        private System.Windows.Forms.TabPage tpArchiveJobs;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Timer tDumper;
        private System.Windows.Forms.Timer tUpdater;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miShowMainForm;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem miJobStart;
        private System.Windows.Forms.ToolStripMenuItem miRestore;
        private System.Windows.Forms.ToolStripSeparator miRemoteSeparator;
        private System.Windows.Forms.ToolStripMenuItem miSendGlobRep;
        private System.Windows.Forms.ToolStripMenuItem miUpdateKey;
        private System.Windows.Forms.ToolStripMenuItem miCheckForUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private GeneralPanel generalPanel1;
        private rabnet.FarmsPanel farmsPanel1;
        private ArchiveJobsPanel archiveJobsPanel1;
        private System.Windows.Forms.ToolStripMenuItem miRunRabnet;
        private System.Windows.Forms.TabPage tpInfo;
        private System.Windows.Forms.SplitContainer scAbout;
        private AboutPanel aboutPanel1;
        private System.Windows.Forms.TextBox tbUpdateInfo;
        private System.Windows.Forms.Button btCheckUpdate;
        private System.Windows.Forms.Button btDloadUpdate;
        private System.Windows.Forms.ProgressBar pbUpdate;
    }
}
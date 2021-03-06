﻿namespace rabnet.panels
{
    partial class RabbitsPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        protected override void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBreed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chWeight = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFlags = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAverAge = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chClass = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chAddress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miPassport = new System.Windows.Forms.ToolStripMenuItem();
            this.miIncome = new System.Windows.Forms.ToolStripMenuItem();
            this.miBon = new System.Windows.Forms.ToolStripMenuItem();
            this.miKill = new System.Windows.Forms.ToolStripMenuItem();
            this.miReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.miBoysOut = new System.Windows.Forms.ToolStripMenuItem();
            this.miPlaceExchange = new System.Windows.Forms.ToolStripMenuItem();
            this.misFemale = new System.Windows.Forms.ToolStripSeparator();
            this.miFucks = new System.Windows.Forms.ToolStripMenuItem();
            this.miLust = new System.Windows.Forms.ToolStripMenuItem();
            this.okrolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.proholostMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miCountKids = new System.Windows.Forms.ToolStripMenuItem();
            this.miYoungersOut = new System.Windows.Forms.ToolStripMenuItem();
            this.misPrint = new System.Windows.Forms.ToolStripSeparator();
            this.svidMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plemMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miRealize = new System.Windows.Forms.ToolStripMenuItem();
            this.miPlanReplace = new System.Windows.Forms.ToolStripMenuItem();
            this.misExtra = new System.Windows.Forms.ToolStripSeparator();
            this.miExport = new System.Windows.Forms.ToolStripMenuItem();
            this.miGenetic = new System.Windows.Forms.ToolStripMenuItem();
            this.misHidden = new System.Windows.Forms.ToolStripSeparator();
            this.показатьНомерToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tvGens = new rabnet.components.RabGenTreeView();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.actMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tvGens);
            this.splitContainer1.Size = new System.Drawing.Size(853, 550);
            this.splitContainer1.SplitterDistance = 709;
            this.splitContainer1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chSex,
            this.chAge,
            this.chBreed,
            this.chWeight,
            this.chStatus,
            this.chFlags,
            this.chCount,
            this.chAverAge,
            this.chRate,
            this.chClass,
            this.chAddress,
            this.chNotes});
            this.listView1.ContextMenuStrip = this.actMenu;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.OwnerDraw = true;
            this.listView1.Size = new System.Drawing.Size(703, 544);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView1_KeyDown);
            this.listView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDown);
            this.listView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseUp);
            // 
            // chName
            // 
            this.chName.Text = "Имя";
            this.chName.Width = 122;
            // 
            // chSex
            // 
            this.chSex.Text = "Пол";
            this.chSex.Width = 41;
            // 
            // chAge
            // 
            this.chAge.Text = "Возраст";
            this.chAge.Width = 47;
            // 
            // chBreed
            // 
            this.chBreed.Text = "Порода";
            this.chBreed.Width = 48;
            // 
            // chWeight
            // 
            this.chWeight.Text = "Вес";
            this.chWeight.Width = 38;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Статус";
            this.chStatus.Width = 50;
            // 
            // chFlags
            // 
            this.chFlags.Text = "Пометки";
            this.chFlags.Width = 35;
            // 
            // chCount
            // 
            this.chCount.Text = "N";
            this.chCount.Width = 31;
            // 
            // chAverAge
            // 
            this.chAverAge.Text = "СрВ";
            this.chAverAge.Width = 36;
            // 
            // chRate
            // 
            this.chRate.Text = "Рейтинг";
            this.chRate.Width = 46;
            // 
            // chClass
            // 
            this.chClass.Text = "Класс";
            this.chClass.Width = 41;
            // 
            // chAddress
            // 
            this.chAddress.Text = "Адрес";
            this.chAddress.Width = 90;
            // 
            // chNotes
            // 
            this.chNotes.Text = "Заметки";
            this.chNotes.Width = 64;
            // 
            // actMenu
            // 
            this.actMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miPassport,
            this.miIncome,
            this.miBon,
            this.miKill,
            this.miReplace,
            this.miBoysOut,
            this.miPlaceExchange,
            this.misFemale,
            this.miFucks,
            this.miLust,
            this.okrolMenuItem,
            this.proholostMenuItem,
            this.miCountKids,
            this.miYoungersOut,
            this.misPrint,
            this.svidMenuItem,
            this.plemMenuItem,
            this.miRealize,
            this.miPlanReplace,
            this.misExtra,
            this.miExport,
            this.miGenetic,
            this.misHidden,
            this.показатьНомерToolStripMenuItem});
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(237, 490);
            this.actMenu.Opening += new System.ComponentModel.CancelEventHandler(this.actMenu_Opening);
            // 
            // miPassport
            // 
            this.miPassport.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.miPassport.Name = "miPassport";
            this.miPassport.Size = new System.Drawing.Size(236, 22);
            this.miPassport.Text = "Паспорт";
            this.miPassport.Click += new System.EventHandler(this.miPassport_Click);
            // 
            // miIncome
            // 
            this.miIncome.Name = "miIncome";
            this.miIncome.Size = new System.Drawing.Size(236, 22);
            this.miIncome.Text = "Привоз";
            this.miIncome.Click += new System.EventHandler(this.miIncome_Click);
            // 
            // miBon
            // 
            this.miBon.Name = "miBon";
            this.miBon.Size = new System.Drawing.Size(236, 22);
            this.miBon.Text = "Бонитировка";
            this.miBon.Click += new System.EventHandler(this.miBon_Click);
            // 
            // miKill
            // 
            this.miKill.Name = "miKill";
            this.miKill.Size = new System.Drawing.Size(236, 22);
            this.miKill.Text = "Списание";
            this.miKill.Click += new System.EventHandler(this.miKill_Click);
            // 
            // miReplace
            // 
            this.miReplace.Name = "miReplace";
            this.miReplace.Size = new System.Drawing.Size(236, 22);
            this.miReplace.Text = "Пересадить";
            this.miReplace.Click += new System.EventHandler(this.miReplace_Click);
            // 
            // miBoysOut
            // 
            this.miBoysOut.Name = "miBoysOut";
            this.miBoysOut.Size = new System.Drawing.Size(236, 22);
            this.miBoysOut.Text = "Отсадить мальчиков";
            this.miBoysOut.Visible = false;
            this.miBoysOut.Click += new System.EventHandler(this.miBoysOut_Click);
            // 
            // miPlaceExchange
            // 
            this.miPlaceExchange.Name = "miPlaceExchange";
            this.miPlaceExchange.Size = new System.Drawing.Size(236, 22);
            this.miPlaceExchange.Text = "Жилобмен";
            this.miPlaceExchange.Click += new System.EventHandler(this.miPlaceGhange_Click);
            // 
            // misFemale
            // 
            this.misFemale.Name = "misFemale";
            this.misFemale.Size = new System.Drawing.Size(233, 6);
            // 
            // miFucks
            // 
            this.miFucks.Name = "miFucks";
            this.miFucks.Size = new System.Drawing.Size(236, 22);
            this.miFucks.Text = "Случка";
            this.miFucks.Click += new System.EventHandler(this.miFucks_Click);
            // 
            // miLust
            // 
            this.miLust.Name = "miLust";
            this.miLust.Size = new System.Drawing.Size(236, 22);
            this.miLust.Text = "Стимуляция крольчихи";
            this.miLust.Click += new System.EventHandler(this.miLust_Click);
            // 
            // okrolMenuItem
            // 
            this.okrolMenuItem.Name = "okrolMenuItem";
            this.okrolMenuItem.Size = new System.Drawing.Size(236, 22);
            this.okrolMenuItem.Text = "Принять окрол";
            this.okrolMenuItem.Click += new System.EventHandler(this.okrolMenuItem_Click);
            // 
            // proholostMenuItem
            // 
            this.proholostMenuItem.Name = "proholostMenuItem";
            this.proholostMenuItem.Size = new System.Drawing.Size(236, 22);
            this.proholostMenuItem.Text = "Прохолостание";
            this.proholostMenuItem.Click += new System.EventHandler(this.proholostMenuItem_Click);
            // 
            // miCountKids
            // 
            this.miCountKids.Name = "miCountKids";
            this.miCountKids.Size = new System.Drawing.Size(236, 22);
            this.miCountKids.Text = "Подсчет гнездовых";
            this.miCountKids.Click += new System.EventHandler(this.countKidsMenuItem_Click);
            // 
            // miYoungersOut
            // 
            this.miYoungersOut.Name = "miYoungersOut";
            this.miYoungersOut.Size = new System.Drawing.Size(236, 22);
            this.miYoungersOut.Text = "Отсадить молодняк";
            this.miYoungersOut.Click += new System.EventHandler(this.miYoungersOut_Click);
            // 
            // misPrint
            // 
            this.misPrint.Name = "misPrint";
            this.misPrint.Size = new System.Drawing.Size(233, 6);
            // 
            // svidMenuItem
            // 
            this.svidMenuItem.Name = "svidMenuItem";
            this.svidMenuItem.Size = new System.Drawing.Size(236, 22);
            this.svidMenuItem.Text = "Племенное свидетельство";
            this.svidMenuItem.Click += new System.EventHandler(this.svidMenuItem_Click);
            // 
            // plemMenuItem
            // 
            this.plemMenuItem.Name = "plemMenuItem";
            this.plemMenuItem.Size = new System.Drawing.Size(236, 22);
            this.plemMenuItem.Text = "Племенной список";
            this.plemMenuItem.Click += new System.EventHandler(this.plemMenuItem_Click);
            // 
            // miRealize
            // 
            this.miRealize.Name = "miRealize";
            this.miRealize.Size = new System.Drawing.Size(236, 22);
            this.miRealize.Text = "Кандидаты на реализацию";
            this.miRealize.Click += new System.EventHandler(this.miRealize_Click);
            // 
            // miPlanReplace
            // 
            this.miPlanReplace.Name = "miPlanReplace";
            this.miPlanReplace.Size = new System.Drawing.Size(236, 22);
            this.miPlanReplace.Text = "План пересадок";
            this.miPlanReplace.Click += new System.EventHandler(this.miPlanReplace_Click);
            // 
            // misExtra
            // 
            this.misExtra.Name = "misExtra";
            this.misExtra.Size = new System.Drawing.Size(233, 6);
            // 
            // miExport
            // 
            this.miExport.Name = "miExport";
            this.miExport.Size = new System.Drawing.Size(236, 22);
            this.miExport.Text = "Экспорт";
            this.miExport.Click += new System.EventHandler(this.miEPasportMake_Click);
            // 
            // miGenetic
            // 
            this.miGenetic.Name = "miGenetic";
            this.miGenetic.Size = new System.Drawing.Size(236, 22);
            this.miGenetic.Text = "Показать родословную";
            this.miGenetic.Click += new System.EventHandler(this.miGenetic_Click);
            // 
            // misHidden
            // 
            this.misHidden.Name = "misHidden";
            this.misHidden.Size = new System.Drawing.Size(233, 6);
            this.misHidden.Visible = false;
            // 
            // показатьНомерToolStripMenuItem
            // 
            this.показатьНомерToolStripMenuItem.Name = "показатьНомерToolStripMenuItem";
            this.показатьНомерToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
                        | System.Windows.Forms.Keys.Z)));
            this.показатьНомерToolStripMenuItem.Size = new System.Drawing.Size(236, 22);
            this.показатьНомерToolStripMenuItem.Text = "Показать номер";
            this.показатьНомерToolStripMenuItem.Visible = false;
            this.показатьНомерToolStripMenuItem.Click += new System.EventHandler(this.tsmiIDshow_Click);
            // 
            // tvGens
            // 
            this.tvGens.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tvGens.Location = new System.Drawing.Point(3, 3);
            this.tvGens.MaxNodesCount = 1;
            this.tvGens.Name = "tvGens";
            this.tvGens.ShowStateColors = true;
            this.tvGens.Size = new System.Drawing.Size(134, 544);
            this.tvGens.TabIndex = 1;
            // 
            // RabbitsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RabbitsPanel";
            this.Size = new System.Drawing.Size(853, 550);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.actMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chSex;
        private System.Windows.Forms.ColumnHeader chAge;
        private System.Windows.Forms.ColumnHeader chBreed;
        private System.Windows.Forms.ColumnHeader chWeight;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.ColumnHeader chFlags;
        private System.Windows.Forms.ColumnHeader chCount;
        private System.Windows.Forms.ColumnHeader chAverAge;
        private System.Windows.Forms.ColumnHeader chRate;
        private System.Windows.Forms.ColumnHeader chClass;
        private System.Windows.Forms.ColumnHeader chAddress;
        private System.Windows.Forms.ColumnHeader chNotes;
        private components.RabGenTreeView tvGens;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ToolStripMenuItem miPassport;
        private System.Windows.Forms.ToolStripMenuItem miIncome;
        private System.Windows.Forms.ToolStripMenuItem miBon;
        private System.Windows.Forms.ToolStripMenuItem miKill;
        private System.Windows.Forms.ToolStripMenuItem miReplace;
        private System.Windows.Forms.ToolStripMenuItem miBoysOut;
        private System.Windows.Forms.ToolStripMenuItem miPlaceExchange;
        private System.Windows.Forms.ToolStripSeparator misFemale;
        private System.Windows.Forms.ToolStripMenuItem miFucks;
        private System.Windows.Forms.ToolStripMenuItem miLust;
        private System.Windows.Forms.ToolStripMenuItem okrolMenuItem;
        private System.Windows.Forms.ToolStripMenuItem proholostMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miCountKids;
        private System.Windows.Forms.ToolStripMenuItem miYoungersOut;
        private System.Windows.Forms.ToolStripSeparator misPrint;
        private System.Windows.Forms.ToolStripMenuItem svidMenuItem;
        private System.Windows.Forms.ToolStripMenuItem plemMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miRealize;
        private System.Windows.Forms.ToolStripMenuItem miPlanReplace;
        private System.Windows.Forms.ToolStripSeparator misExtra;
        private System.Windows.Forms.ToolStripMenuItem miExport;
        private System.Windows.Forms.ToolStripMenuItem miGenetic;
        private System.Windows.Forms.ToolStripSeparator misHidden;
        private System.Windows.Forms.ToolStripMenuItem показатьНомерToolStripMenuItem;
    }
}

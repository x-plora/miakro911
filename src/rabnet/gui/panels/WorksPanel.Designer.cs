﻿namespace rabnet.panels
{
    partial class WorksPanel
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
            this.lvZooTech = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miOkrol = new System.Windows.Forms.ToolStripMenuItem();
            this.miNestOut = new System.Windows.Forms.ToolStripMenuItem();
            this.miCountKids = new System.Windows.Forms.ToolStripMenuItem();
            this.miCountKidsChanged = new System.Windows.Forms.ToolStripMenuItem();
            this.miPreOkrol = new System.Windows.Forms.ToolStripMenuItem();
            this.miBoysOut = new System.Windows.Forms.ToolStripMenuItem();
            this.miGirlsOut = new System.Windows.Forms.ToolStripMenuItem();
            this.miFuck = new System.Windows.Forms.ToolStripMenuItem();
            this.miLust = new System.Windows.Forms.ToolStripMenuItem();
            this.miVaccine = new System.Windows.Forms.ToolStripMenuItem();
            this.miNestSet = new System.Windows.Forms.ToolStripMenuItem();
            this.miBoysByOne = new System.Windows.Forms.ToolStripMenuItem();
            this.miSpermTake = new System.Windows.Forms.ToolStripMenuItem();
            this.miPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.lvLogs = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.actMenu.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvZooTech
            // 
            this.lvZooTech.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvZooTech.BackColor = System.Drawing.SystemColors.Window;
            this.lvZooTech.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader6});
            this.lvZooTech.ContextMenuStrip = this.actMenu;
            this.lvZooTech.FullRowSelect = true;
            this.lvZooTech.GridLines = true;
            this.lvZooTech.HideSelection = false;
            this.lvZooTech.Location = new System.Drawing.Point(3, 3);
            this.lvZooTech.Name = "lvZooTech";
            this.lvZooTech.OwnerDraw = true;
            this.lvZooTech.Size = new System.Drawing.Size(677, 266);
            this.lvZooTech.TabIndex = 0;
            this.lvZooTech.UseCompatibleStateImageBehavior = false;
            this.lvZooTech.View = System.Windows.Forms.View.Details;            
            this.lvZooTech.DoubleClick += new System.EventHandler(this.MenuItem_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "!";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Название работ";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Адрес";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Имя";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Возраст";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Порода";
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Доп.Инф.";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Партнеры";
            // 
            // actMenu
            // 
            this.actMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miOkrol,
            this.miNestOut,
            this.miCountKids,
            this.miCountKidsChanged,
            this.miPreOkrol,
            this.miBoysOut,
            this.miGirlsOut,
            this.miFuck,
            this.miLust,
            this.miVaccine,
            this.miNestSet,
            this.miBoysByOne,
            this.miSpermTake,
            this.miPrint});
            this.actMenu.Name = "actMenu";
            this.actMenu.Size = new System.Drawing.Size(265, 312);
            this.actMenu.Opening += new System.ComponentModel.CancelEventHandler(this.actMenu_Opening);
            // 
            // okrolMenuItem
            // 
            this.miOkrol.Name = "miOkrol";
            this.miOkrol.Size = new System.Drawing.Size(264, 22);
            this.miOkrol.Text = "Принять окрол";
            this.miOkrol.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // miNEST_OUT
            // 
            this.miNestOut.Name = "miNestOut";
            this.miNestOut.Size = new System.Drawing.Size(264, 22);
            this.miNestOut.Text = "Выдворение";
            this.miNestOut.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // countsMenuItem
            // 
            this.miCountKids.Name = "miCountKids";
            this.miCountKids.Size = new System.Drawing.Size(264, 22);
            this.miCountKids.Text = "Подсчет гнездовых/подсосных";
            this.miCountKids.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // countChangedMenuItem
            // 
            this.miCountKidsChanged.Name = "miCountKidsChanged";
            this.miCountKidsChanged.Size = new System.Drawing.Size(264, 22);
            this.miCountKidsChanged.Text = "Изменилось количество крольчат";
            this.miCountKidsChanged.Click += new System.EventHandler(this.countChangedMenuItem_Click);
            // 
            // preokrolMenuItem
            // 
            this.miPreOkrol.Name = "miPreOkrol";
            this.miPreOkrol.Size = new System.Drawing.Size(264, 22);
            this.miPreOkrol.Text = "Предокрольный осмотр";
            this.miPreOkrol.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // boysOutMenuItem
            // 
            this.miBoysOut.Name = "miBoysOut";
            this.miBoysOut.Size = new System.Drawing.Size(264, 22);
            this.miBoysOut.Text = "Отсадить мальчиков";
            this.miBoysOut.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // girlsOutMenuItem
            // 
            this.miGirlsOut.Name = "miGirlsOut";
            this.miGirlsOut.Size = new System.Drawing.Size(264, 22);
            this.miGirlsOut.Text = "Отсадить девочек";
            this.miGirlsOut.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // fuckMenuItem
            // 
            this.miFuck.Name = "miFuck";
            this.miFuck.Size = new System.Drawing.Size(264, 22);
            this.miFuck.Text = "Случить";
            this.miFuck.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // miLust
            // 
            this.miLust.Name = "miLust";
            this.miLust.Size = new System.Drawing.Size(264, 22);
            this.miLust.Text = "Стимуляция";
            this.miLust.Click += new System.EventHandler(this.miLust_Click);
            // 
            // vaccMenuItem
            // 
            this.miVaccine.Name = "miVaccine";
            this.miVaccine.Size = new System.Drawing.Size(264, 22);
            this.miVaccine.Text = "Привить";
            this.miVaccine.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // setNestMenuItem
            // 
            this.miNestSet.Name = "miNestSet";
            this.miNestSet.Size = new System.Drawing.Size(264, 22);
            this.miNestSet.Text = "Установить гнездовье";
            this.miNestSet.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // miBoysByOne
            // 
            this.miBoysByOne.Name = "miBoysByOne";
            this.miBoysByOne.Size = new System.Drawing.Size(264, 22);
            this.miBoysByOne.Text = "Рассадка по одному";
            this.miBoysByOne.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // miSpermTake
            // 
            this.miSpermTake.Name = "miSpermTake";
            this.miSpermTake.Size = new System.Drawing.Size(264, 22);
            this.miSpermTake.Text = "Забор спермы";
            this.miSpermTake.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // printMenuItem
            // 
            this.miPrint.Name = "miPrint";
            this.miPrint.Size = new System.Drawing.Size(264, 22);
            this.miPrint.Text = "Печать";
            this.miPrint.Click += new System.EventHandler(this.printMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvZooTech);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.lvLogs);
            this.splitContainer1.Size = new System.Drawing.Size(683, 459);
            this.splitContainer1.SplitterDistance = 272;
            this.splitContainer1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(683, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "События (Логи)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lvLogs
            // 
            this.lvLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader15,
            this.columnHeader14});
            this.lvLogs.ForeColor = System.Drawing.Color.Black;
            this.lvLogs.FullRowSelect = true;
            this.lvLogs.GridLines = true;
            this.lvLogs.HideSelection = false;
            this.lvLogs.Location = new System.Drawing.Point(3, 22);
            this.lvLogs.Name = "lvLogs";
            this.lvLogs.Size = new System.Drawing.Size(677, 158);
            this.lvLogs.TabIndex = 0;
            this.lvLogs.UseCompatibleStateImageBehavior = false;
            this.lvLogs.View = System.Windows.Forms.View.Details;            
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Дата";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Событие";
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Адрес";
            this.columnHeader13.Width = 75;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Параметры";
            this.columnHeader15.Width = 342;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Пользователь";
            // 
            // WorksPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "WorksPanel";
            this.Size = new System.Drawing.Size(689, 465);
            this.actMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvZooTech;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lvLogs;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ContextMenuStrip actMenu;
        private System.Windows.Forms.ToolStripMenuItem miOkrol;
        private System.Windows.Forms.ToolStripMenuItem miNestOut;
        private System.Windows.Forms.ToolStripMenuItem miCountKids;
        private System.Windows.Forms.ToolStripMenuItem miCountKidsChanged;
        private System.Windows.Forms.ToolStripMenuItem miPreOkrol;
        private System.Windows.Forms.ToolStripMenuItem miBoysOut;
        private System.Windows.Forms.ToolStripMenuItem miGirlsOut;
        private System.Windows.Forms.ToolStripMenuItem miFuck;
        private System.Windows.Forms.ToolStripMenuItem miVaccine;
        private System.Windows.Forms.ToolStripMenuItem miLust;
        private System.Windows.Forms.ToolStripMenuItem miNestSet;
        private System.Windows.Forms.ToolStripMenuItem miBoysByOne;        
        private System.Windows.Forms.ToolStripMenuItem miSpermTake;
        private System.Windows.Forms.ToolStripMenuItem miPrint;        
        private System.Windows.Forms.ColumnHeader columnHeader14;       
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label1;        
    }
}

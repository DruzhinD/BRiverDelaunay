namespace MeshExplorer
{
    partial class FormMain
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
            splitContainer = new System.Windows.Forms.SplitContainer();
            btnSmooth = new Controls.DarkButton();
            flatTabControl1 = new Controls.DarkTabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            meshControlView = new Views.MeshControlView();
            tabPage2 = new System.Windows.Forms.TabPage();
            statisticView = new Views.StatisticView();
            tabPage3 = new System.Windows.Forms.TabPage();
            aboutView = new Views.AboutView();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            menuFile = new System.Windows.Forms.ToolStripMenuItem();
            menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
            menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            menuFileExport = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            menuFileQuit = new System.Windows.Forms.ToolStripMenuItem();
            menuView = new System.Windows.Forms.ToolStripMenuItem();
            menuViewVoronoi = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            menuViewLog = new System.Windows.Forms.ToolStripMenuItem();
            menuTools = new System.Windows.Forms.ToolStripMenuItem();
            menuToolsGen = new System.Windows.Forms.ToolStripMenuItem();
            menuToolsCheck = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            menuToolsTopology = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            menuToolsRcm = new System.Windows.Forms.ToolStripMenuItem();
            btnMesh = new Controls.DarkButton();
            addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.SuspendLayout();
            flatTabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.BackColor = System.Drawing.Color.FromArgb(68, 68, 68);
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            splitContainer.IsSplitterFixed = true;
            splitContainer.Location = new System.Drawing.Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.BackColor = System.Drawing.Color.FromArgb(76, 76, 76);
            splitContainer.Panel1.Controls.Add(btnSmooth);
            splitContainer.Panel1.Controls.Add(flatTabControl1);
            splitContainer.Panel1.Controls.Add(menuStrip1);
            splitContainer.Panel1.Controls.Add(btnMesh);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.BackColor = System.Drawing.Color.Black;
            splitContainer.Size = new System.Drawing.Size(992, 623);
            splitContainer.SplitterDistance = 280;
            splitContainer.SplitterWidth = 1;
            splitContainer.TabIndex = 0;
            // 
            // btnSmooth
            // 
            btnSmooth.Enabled = false;
            btnSmooth.Location = new System.Drawing.Point(146, 44);
            btnSmooth.Name = "btnSmooth";
            btnSmooth.Size = new System.Drawing.Size(130, 23);
            btnSmooth.TabIndex = 12;
            btnSmooth.Text = "Smooth";
            btnSmooth.UseVisualStyleBackColor = true;
            btnSmooth.Click += btnSmooth_Click;
            // 
            // flatTabControl1
            // 
            flatTabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            flatTabControl1.Controls.Add(tabPage1);
            flatTabControl1.Controls.Add(tabPage2);
            flatTabControl1.Controls.Add(tabPage3);
            flatTabControl1.Location = new System.Drawing.Point(0, 73);
            flatTabControl1.Name = "flatTabControl1";
            flatTabControl1.SelectedIndex = 0;
            flatTabControl1.Size = new System.Drawing.Size(280, 538);
            flatTabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = System.Drawing.Color.DimGray;
            tabPage1.Controls.Add(meshControlView);
            tabPage1.ForeColor = System.Drawing.Color.DarkGray;
            tabPage1.Location = new System.Drawing.Point(4, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new System.Drawing.Size(272, 512);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Mesh Control";
            // 
            // meshControlView
            // 
            meshControlView.BackColor = System.Drawing.Color.DimGray;
            meshControlView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            meshControlView.ForeColor = System.Drawing.Color.DarkGray;
            meshControlView.Location = new System.Drawing.Point(0, 0);
            meshControlView.Name = "meshControlView";
            meshControlView.Size = new System.Drawing.Size(272, 509);
            meshControlView.TabIndex = 0;
            // 
            // tabPage2
            // 
            tabPage2.BackColor = System.Drawing.Color.DimGray;
            tabPage2.Controls.Add(statisticView);
            tabPage2.ForeColor = System.Drawing.Color.White;
            tabPage2.Location = new System.Drawing.Point(4, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(272, 509);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Statistic";
            // 
            // statisticView
            // 
            statisticView.BackColor = System.Drawing.Color.DimGray;
            statisticView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            statisticView.ForeColor = System.Drawing.Color.DarkGray;
            statisticView.Location = new System.Drawing.Point(0, 0);
            statisticView.Name = "statisticView";
            statisticView.Size = new System.Drawing.Size(272, 509);
            statisticView.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.BackColor = System.Drawing.Color.DimGray;
            tabPage3.Controls.Add(aboutView);
            tabPage3.Location = new System.Drawing.Point(4, 4);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new System.Windows.Forms.Padding(3);
            tabPage3.Size = new System.Drawing.Size(272, 509);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "About";
            // 
            // aboutView
            // 
            aboutView.BackColor = System.Drawing.Color.DimGray;
            aboutView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            aboutView.ForeColor = System.Drawing.Color.DarkGray;
            aboutView.Location = new System.Drawing.Point(0, 0);
            aboutView.Name = "aboutView";
            aboutView.Size = new System.Drawing.Size(272, 509);
            aboutView.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            menuStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            menuStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuFile, menuView, menuTools });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            menuStrip1.Size = new System.Drawing.Size(280, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            menuFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            menuFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { menuFileOpen, menuFileSave, toolStripSeparator3, menuFileExport, toolStripSeparator2, menuFileQuit, addToolStripMenuItem });
            menuFile.Name = "menuFile";
            menuFile.Size = new System.Drawing.Size(37, 24);
            menuFile.Text = "File";
            // 
            // menuFileOpen
            // 
            menuFileOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menuFileOpen.Name = "menuFileOpen";
            menuFileOpen.Size = new System.Drawing.Size(180, 22);
            menuFileOpen.Text = "Open";
            menuFileOpen.Click += menuFileOpen_Click;
            // 
            // menuFileSave
            // 
            menuFileSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            menuFileSave.Enabled = false;
            menuFileSave.Name = "menuFileSave";
            menuFileSave.Size = new System.Drawing.Size(180, 22);
            menuFileSave.Text = "Save";
            menuFileSave.Click += menuFileSave_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // menuFileExport
            // 
            menuFileExport.Enabled = false;
            menuFileExport.Name = "menuFileExport";
            menuFileExport.Size = new System.Drawing.Size(180, 22);
            menuFileExport.Text = "Export Image";
            menuFileExport.Click += menuFileExport_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // menuFileQuit
            // 
            menuFileQuit.Name = "menuFileQuit";
            menuFileQuit.Size = new System.Drawing.Size(180, 22);
            menuFileQuit.Text = "Quit";
            menuFileQuit.Click += menuFileQuit_Click;
            // 
            // menuView
            // 
            menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { menuViewVoronoi, toolStripSeparator1, menuViewLog });
            menuView.Name = "menuView";
            menuView.Size = new System.Drawing.Size(44, 24);
            menuView.Text = "View";
            // 
            // menuViewVoronoi
            // 
            menuViewVoronoi.Enabled = false;
            menuViewVoronoi.Name = "menuViewVoronoi";
            menuViewVoronoi.Size = new System.Drawing.Size(161, 22);
            menuViewVoronoi.Text = "Voronoi Diagram";
            menuViewVoronoi.Click += menuViewVoronoi_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(158, 6);
            // 
            // menuViewLog
            // 
            menuViewLog.Name = "menuViewLog";
            menuViewLog.Size = new System.Drawing.Size(161, 22);
            menuViewLog.Text = "Show Log";
            menuViewLog.Click += menuViewLog_Click;
            // 
            // menuTools
            // 
            menuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { menuToolsGen, menuToolsCheck, toolStripSeparator5, menuToolsTopology, toolStripSeparator4, menuToolsRcm });
            menuTools.Name = "menuTools";
            menuTools.Size = new System.Drawing.Size(46, 24);
            menuTools.Text = "Tools";
            // 
            // menuToolsGen
            // 
            menuToolsGen.Name = "menuToolsGen";
            menuToolsGen.Size = new System.Drawing.Size(195, 22);
            menuToolsGen.Text = "Input Generator";
            menuToolsGen.Click += menuToolsGenerator_Click;
            // 
            // menuToolsCheck
            // 
            menuToolsCheck.Enabled = false;
            menuToolsCheck.Name = "menuToolsCheck";
            menuToolsCheck.Size = new System.Drawing.Size(195, 22);
            menuToolsCheck.Text = "Check Mesh";
            menuToolsCheck.Click += menuToolsCheck_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
            // 
            // menuToolsTopology
            // 
            menuToolsTopology.Name = "menuToolsTopology";
            menuToolsTopology.Size = new System.Drawing.Size(195, 22);
            menuToolsTopology.Text = "Topology Explorer";
            menuToolsTopology.Click += menuToolsTopology_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(192, 6);
            // 
            // menuToolsRcm
            // 
            menuToolsRcm.Enabled = false;
            menuToolsRcm.Name = "menuToolsRcm";
            menuToolsRcm.Size = new System.Drawing.Size(195, 22);
            menuToolsRcm.Text = "Renumber nodes (RCM)";
            menuToolsRcm.Click += menuToolsRcm_Click;
            // 
            // btnMesh
            // 
            btnMesh.Enabled = false;
            btnMesh.Location = new System.Drawing.Point(4, 44);
            btnMesh.Name = "btnMesh";
            btnMesh.Size = new System.Drawing.Size(130, 23);
            btnMesh.TabIndex = 12;
            btnMesh.Text = "Triangulate";
            btnMesh.UseVisualStyleBackColor = true;
            btnMesh.Click += btnMesh_Click;
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += addToolStripMenuItem_Click;
            // 
            // FormMain
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(76, 76, 76);
            ClientSize = new System.Drawing.Size(992, 623);
            Controls.Add(splitContainer);
            Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            MinimumSize = new System.Drawing.Size(1000, 650);
            Name = "FormMain";
            Text = "Triangle.NET - Mesh Explorer";
            Load += Form1_Load;
            ResizeBegin += ResizeBeginHandler;
            ResizeEnd += ResizeEndHandler;
            DragDrop += frmDragDrop;
            DragOver += frmDragOver;
            KeyUp += Form1_KeyUp;
            Resize += ResizeHandler;
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            flatTabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
        private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private Controls.DarkTabControl flatTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.DarkButton btnSmooth;
        private Controls.DarkButton btnMesh;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem menuViewVoronoi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuViewLog;
        private System.Windows.Forms.ToolStripMenuItem menuTools;
        private System.Windows.Forms.ToolStripMenuItem menuToolsGen;
        private System.Windows.Forms.ToolStripMenuItem menuToolsCheck;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuFileQuit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuFileExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuToolsRcm;
        private Views.MeshControlView meshControlView;
        private Views.StatisticView statisticView;
        private Views.AboutView aboutView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuToolsTopology;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
    }
}


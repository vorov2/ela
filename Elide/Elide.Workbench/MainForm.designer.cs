using Elide.Forms;
namespace Elide.Workbench
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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mainSplit = new Elide.Forms.SplitContainerEx();
            this.toolDock = new Elide.Forms.SplitContainerEx();
            this.documentContainer = new Elide.Forms.DocumentContainer();
            this.docPanel = new System.Windows.Forms.Panel();
            this.toolWindow = new Elide.Forms.ToolWindow();
            this.outputsBar = new Elide.Forms.ActiveToolbar();
            this.topBorderPanel = new Elide.Forms.SingleBorderPanel();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel1.SuspendLayout();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolDock)).BeginInit();
            this.toolDock.Panel1.SuspendLayout();
            this.toolDock.Panel2.SuspendLayout();
            this.toolDock.SuspendLayout();
            this.documentContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(947, 24);
            this.mainMenu.TabIndex = 1;
            this.mainMenu.Text = "menuStrip1";
            // 
            // mainSplit
            // 
            this.mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainSplit.Location = new System.Drawing.Point(0, 29);
            this.mainSplit.Name = "mainSplit";
            this.mainSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.Controls.Add(this.toolDock);
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.Controls.Add(this.outputsBar);
            this.mainSplit.Panel2MinSize = 19;
            this.mainSplit.Size = new System.Drawing.Size(947, 604);
            this.mainSplit.SplitterDistance = 456;
            this.mainSplit.TabIndex = 2;
            // 
            // toolDock
            // 
            this.toolDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolDock.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.toolDock.Location = new System.Drawing.Point(0, 0);
            this.toolDock.Name = "toolDock";
            // 
            // toolDock.Panel1
            // 
            this.toolDock.Panel1.Controls.Add(this.documentContainer);
            // 
            // toolDock.Panel2
            // 
            this.toolDock.Panel2.Controls.Add(this.toolWindow);
            this.toolDock.Size = new System.Drawing.Size(947, 456);
            this.toolDock.SplitterDistance = 697;
            this.toolDock.TabIndex = 3;
            // 
            // documentContainer
            // 
            this.documentContainer.Column = 0;
            this.documentContainer.Controls.Add(this.docPanel);
            this.documentContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentContainer.InfoBarVisible = false;
            this.documentContainer.Line = 0;
            this.documentContainer.Location = new System.Drawing.Point(0, 0);
            this.documentContainer.Name = "documentContainer";
            this.documentContainer.Overtype = false;
            this.documentContainer.Padding = new System.Windows.Forms.Padding(1, 21, 1, 1);
            this.documentContainer.SelectedDocumentFunc = null;
            this.documentContainer.Size = new System.Drawing.Size(697, 456);
            this.documentContainer.TabIndex = 0;
            this.documentContainer.Text = "documentContainer1";
            this.documentContainer.UserDocumentSelect += new System.EventHandler<Elide.Forms.ObjectEventArgs>(this.documentContainer_UserDocumentSelect);
            this.documentContainer.MoreDocumentsRequested += new System.EventHandler(this.documentContainer_MoreDocumentsRequested);
            this.documentContainer.InfoBarUpdate += new System.EventHandler(this.docs_InfoBarUpdate);
            // 
            // docPanel
            // 
            this.docPanel.BackColor = System.Drawing.SystemColors.Control;
            this.docPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.docPanel.Location = new System.Drawing.Point(1, 21);
            this.docPanel.Name = "docPanel";
            this.docPanel.Size = new System.Drawing.Size(695, 434);
            this.docPanel.TabIndex = 0;
            // 
            // toolWindow
            // 
            this.toolWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolWindow.Location = new System.Drawing.Point(0, 0);
            this.toolWindow.Name = "toolWindow";
            this.toolWindow.SelectedIndex = -1;
            this.toolWindow.Size = new System.Drawing.Size(246, 456);
            this.toolWindow.TabIndex = 0;
            this.toolWindow.SelectedIndexChanged += new System.EventHandler<Elide.Forms.SwitchBarEventArgs>(this.toolWindow_SelectedIndexChanged);
            // 
            // outputsBar
            // 
            this.outputsBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.outputsBar.HighlightStatusString = false;
            this.outputsBar.Location = new System.Drawing.Point(0, 0);
            this.outputsBar.Name = "outputsBar";
            this.outputsBar.SelectedIndex = -1;
            this.outputsBar.Size = new System.Drawing.Size(947, 20);
            this.outputsBar.StatusImage = null;
            this.outputsBar.StatusString = "Ready";
            this.outputsBar.TabIndex = 0;
            this.outputsBar.SelectedIndexChanged += new System.EventHandler(this.outputsBar_SelectedIndexChanged);
            // 
            // topBorderPanel
            // 
            this.topBorderPanel.BottomBorder = false;
            this.topBorderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topBorderPanel.LeftBorder = true;
            this.topBorderPanel.Location = new System.Drawing.Point(0, 24);
            this.topBorderPanel.Name = "topBorderPanel";
            this.topBorderPanel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.topBorderPanel.RightBorder = true;
            this.topBorderPanel.Size = new System.Drawing.Size(947, 5);
            this.topBorderPanel.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 633);
            this.Controls.Add(this.mainSplit);
            this.Controls.Add(this.topBorderPanel);
            this.Controls.Add(this.mainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.MainMenuStrip = this.mainMenu;
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "MainForm";
            this.Text = "Elide";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.mainSplit.Panel1.ResumeLayout(false);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            this.toolDock.Panel1.ResumeLayout(false);
            this.toolDock.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolDock)).EndInit();
            this.toolDock.ResumeLayout(false);
            this.documentContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Elide.Forms.DocumentContainer documentContainer;
        private System.Windows.Forms.MenuStrip mainMenu;
        private SplitContainerEx mainSplit;
        private Elide.Forms.ActiveToolbar outputsBar;
        private System.Windows.Forms.Panel docPanel;
        private SplitContainerEx toolDock;
        private Elide.Forms.ToolWindow toolWindow;
        private Elide.Forms.SingleBorderPanel topBorderPanel;
    }
}


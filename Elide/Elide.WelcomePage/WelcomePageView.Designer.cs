namespace Elide.WelcomePage
{
    partial class WelcomePageView
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
        private void InitializeComponent()
        {
            this.panel = new System.Windows.Forms.Panel();
            this.dontShow = new System.Windows.Forms.CheckBox();
            this.thingsToDoLabel = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.Label();
            this.linkLayout1 = new System.Windows.Forms.FlowLayoutPanel();
            this.configure = new System.Windows.Forms.LinkLabel();
            this.codeSamples = new System.Windows.Forms.LinkLabel();
            this.newFile = new System.Windows.Forms.LinkLabel();
            this.linkLayout2 = new System.Windows.Forms.FlowLayoutPanel();
            this.docLink = new System.Windows.Forms.LinkLabel();
            this.codeSamplesLink = new System.Windows.Forms.LinkLabel();
            this.book = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.documentationLabel = new System.Windows.Forms.Label();
            this.linkLayout3 = new System.Windows.Forms.FlowLayoutPanel();
            this.homepageLink = new System.Windows.Forms.LinkLabel();
            this.rosettaLink = new System.Windows.Forms.LinkLabel();
            this.ohlohLink = new System.Windows.Forms.LinkLabel();
            this.groupLink = new System.Windows.Forms.LinkLabel();
            this.onlineConsoleLink = new System.Windows.Forms.LinkLabel();
            this.resourcesLabel = new System.Windows.Forms.Label();
            this.whatsNewLink = new System.Windows.Forms.LinkLabel();
            this.panel.SuspendLayout();
            this.linkLayout1.SuspendLayout();
            this.linkLayout2.SuspendLayout();
            this.linkLayout3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.dontShow);
            this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel.Location = new System.Drawing.Point(0, 700);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1037, 22);
            this.panel.TabIndex = 0;
            // 
            // dontShow
            // 
            this.dontShow.AutoSize = true;
            this.dontShow.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.dontShow.Location = new System.Drawing.Point(3, 3);
            this.dontShow.Name = "dontShow";
            this.dontShow.Size = new System.Drawing.Size(187, 17);
            this.dontShow.TabIndex = 0;
            this.dontShow.Text = "&Don\'t show this page next time";
            this.dontShow.UseVisualStyleBackColor = true;
            this.dontShow.CheckedChanged += new System.EventHandler(this.dontShow_CheckedChanged);
            // 
            // thingsToDoLabel
            // 
            this.thingsToDoLabel.AutoSize = true;
            this.thingsToDoLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.thingsToDoLabel.ForeColor = System.Drawing.Color.DimGray;
            this.thingsToDoLabel.Location = new System.Drawing.Point(19, 239);
            this.thingsToDoLabel.Name = "thingsToDoLabel";
            this.thingsToDoLabel.Size = new System.Drawing.Size(124, 21);
            this.thingsToDoLabel.TabIndex = 1;
            this.thingsToDoLabel.Text = "Getting started";
            // 
            // version
            // 
            this.version.AutoSize = true;
            this.version.BackColor = System.Drawing.Color.White;
            this.version.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.version.Location = new System.Drawing.Point(379, 150);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(79, 13);
            this.version.TabIndex = 2;
            this.version.Text = "Version: {0} {1}";
            // 
            // linkLayout1
            // 
            this.linkLayout1.Controls.Add(this.configure);
            this.linkLayout1.Controls.Add(this.codeSamples);
            this.linkLayout1.Controls.Add(this.newFile);
            this.linkLayout1.Location = new System.Drawing.Point(17, 263);
            this.linkLayout1.Name = "linkLayout1";
            this.linkLayout1.Size = new System.Drawing.Size(200, 257);
            this.linkLayout1.TabIndex = 4;
            // 
            // configure
            // 
            this.configure.AutoSize = true;
            this.configure.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.configure.Location = new System.Drawing.Point(3, 0);
            this.configure.Name = "configure";
            this.configure.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.configure.Size = new System.Drawing.Size(87, 18);
            this.configure.TabIndex = 2;
            this.configure.TabStop = true;
            this.configure.Text = "Configure Elide";
            this.configure.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.configure_LinkClicked);
            // 
            // codeSamples
            // 
            this.codeSamples.AutoSize = true;
            this.codeSamples.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.codeSamples.Location = new System.Drawing.Point(3, 18);
            this.codeSamples.Name = "codeSamples";
            this.codeSamples.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.codeSamples.Size = new System.Drawing.Size(165, 18);
            this.codeSamples.TabIndex = 0;
            this.codeSamples.TabStop = true;
            this.codeSamples.Text = "Browse code &samples directory";
            this.codeSamples.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.codeSamples_LinkClicked);
            // 
            // newFile
            // 
            this.newFile.AutoSize = true;
            this.newFile.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.newFile.Location = new System.Drawing.Point(3, 36);
            this.newFile.Name = "newFile";
            this.newFile.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.newFile.Size = new System.Drawing.Size(154, 18);
            this.newFile.TabIndex = 1;
            this.newFile.TabStop = true;
            this.newFile.Text = "Create your own script in Ela";
            this.newFile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.newFile_LinkClicked);
            // 
            // linkLayout2
            // 
            this.linkLayout2.Controls.Add(this.whatsNewLink);
            this.linkLayout2.Controls.Add(this.docLink);
            this.linkLayout2.Controls.Add(this.codeSamplesLink);
            this.linkLayout2.Controls.Add(this.book);
            this.linkLayout2.Location = new System.Drawing.Point(233, 263);
            this.linkLayout2.Name = "linkLayout2";
            this.linkLayout2.Size = new System.Drawing.Size(200, 257);
            this.linkLayout2.TabIndex = 6;
            // 
            // docLink
            // 
            this.docLink.AutoSize = true;
            this.docLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.docLink.Location = new System.Drawing.Point(3, 18);
            this.docLink.Name = "docLink";
            this.docLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.docLink.Size = new System.Drawing.Size(124, 18);
            this.docLink.TabIndex = 2;
            this.docLink.TabStop = true;
            this.docLink.Text = "Documentation Library";
            this.docLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.docsLink_LinkClicked);
            // 
            // codeSamplesLink
            // 
            this.codeSamplesLink.AutoSize = true;
            this.linkLayout2.SetFlowBreak(this.codeSamplesLink, true);
            this.codeSamplesLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.codeSamplesLink.Location = new System.Drawing.Point(3, 36);
            this.codeSamplesLink.Name = "codeSamplesLink";
            this.codeSamplesLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.codeSamplesLink.Size = new System.Drawing.Size(126, 18);
            this.codeSamplesLink.TabIndex = 1;
            this.codeSamplesLink.TabStop = true;
            this.codeSamplesLink.Text = "Code samples directory";
            this.codeSamplesLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.codeSamplesLink_LinkClicked);
            // 
            // book
            // 
            this.book.AutoSize = true;
            this.book.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.book.Location = new System.Drawing.Point(3, 54);
            this.book.Name = "book";
            this.book.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.book.Size = new System.Drawing.Size(86, 18);
            this.book.TabIndex = 3;
            this.book.TabStop = true;
            this.book.Text = "Book about Ela";
            this.book.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.book_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLayout3.SetFlowBreak(this.linkLabel1, true);
            this.linkLabel1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.linkLabel1.Location = new System.Drawing.Point(3, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.linkLabel1.Size = new System.Drawing.Size(103, 18);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "CodeProject article";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // documentationLabel
            // 
            this.documentationLabel.AutoSize = true;
            this.documentationLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.documentationLabel.ForeColor = System.Drawing.Color.DimGray;
            this.documentationLabel.Location = new System.Drawing.Point(235, 239);
            this.documentationLabel.Name = "documentationLabel";
            this.documentationLabel.Size = new System.Drawing.Size(130, 21);
            this.documentationLabel.TabIndex = 5;
            this.documentationLabel.Text = "Documentation";
            // 
            // linkLayout3
            // 
            this.linkLayout3.Controls.Add(this.linkLabel1);
            this.linkLayout3.Controls.Add(this.homepageLink);
            this.linkLayout3.Controls.Add(this.rosettaLink);
            this.linkLayout3.Controls.Add(this.ohlohLink);
            this.linkLayout3.Controls.Add(this.groupLink);
            this.linkLayout3.Controls.Add(this.onlineConsoleLink);
            this.linkLayout3.Location = new System.Drawing.Point(450, 263);
            this.linkLayout3.Name = "linkLayout3";
            this.linkLayout3.Size = new System.Drawing.Size(200, 257);
            this.linkLayout3.TabIndex = 8;
            // 
            // homepageLink
            // 
            this.homepageLink.AutoSize = true;
            this.homepageLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.homepageLink.Location = new System.Drawing.Point(3, 18);
            this.homepageLink.Name = "homepageLink";
            this.homepageLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.homepageLink.Size = new System.Drawing.Size(106, 18);
            this.homepageLink.TabIndex = 2;
            this.homepageLink.TabStop = true;
            this.homepageLink.Text = "Ela at Google Code";
            this.homepageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.homepageLink_LinkClicked);
            // 
            // rosettaLink
            // 
            this.rosettaLink.AutoSize = true;
            this.linkLayout3.SetFlowBreak(this.rosettaLink, true);
            this.rosettaLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.rosettaLink.Location = new System.Drawing.Point(3, 36);
            this.rosettaLink.Name = "rosettaLink";
            this.rosettaLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.rosettaLink.Size = new System.Drawing.Size(153, 18);
            this.rosettaLink.TabIndex = 0;
            this.rosettaLink.TabStop = true;
            this.rosettaLink.Text = "Ela in Rosetta code directory";
            this.rosettaLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.rosettaLink_LinkClicked);
            // 
            // ohlohLink
            // 
            this.ohlohLink.AutoSize = true;
            this.linkLayout3.SetFlowBreak(this.ohlohLink, true);
            this.ohlohLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.ohlohLink.Location = new System.Drawing.Point(3, 54);
            this.ohlohLink.Name = "ohlohLink";
            this.ohlohLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ohlohLink.Size = new System.Drawing.Size(71, 18);
            this.ohlohLink.TabIndex = 1;
            this.ohlohLink.TabStop = true;
            this.ohlohLink.Text = "Ela at Ohloh";
            this.ohlohLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ohlohLink_LinkClicked);
            // 
            // groupLink
            // 
            this.groupLink.AutoSize = true;
            this.linkLayout3.SetFlowBreak(this.groupLink, true);
            this.groupLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.groupLink.Location = new System.Drawing.Point(3, 72);
            this.groupLink.Name = "groupLink";
            this.groupLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.groupLink.Size = new System.Drawing.Size(114, 18);
            this.groupLink.TabIndex = 4;
            this.groupLink.TabStop = true;
            this.groupLink.Text = "Ela discussion group";
            this.groupLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.groupLink_LinkClicked);
            // 
            // onlineConsoleLink
            // 
            this.onlineConsoleLink.AutoSize = true;
            this.onlineConsoleLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.onlineConsoleLink.Location = new System.Drawing.Point(3, 90);
            this.onlineConsoleLink.Name = "onlineConsoleLink";
            this.onlineConsoleLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.onlineConsoleLink.Size = new System.Drawing.Size(143, 18);
            this.onlineConsoleLink.TabIndex = 3;
            this.onlineConsoleLink.TabStop = true;
            this.onlineConsoleLink.Text = "Online Interactive Console";
            this.onlineConsoleLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.onlineConsoleLink_LinkClicked);
            // 
            // resourcesLabel
            // 
            this.resourcesLabel.AutoSize = true;
            this.resourcesLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.resourcesLabel.ForeColor = System.Drawing.Color.DimGray;
            this.resourcesLabel.Location = new System.Drawing.Point(452, 239);
            this.resourcesLabel.Name = "resourcesLabel";
            this.resourcesLabel.Size = new System.Drawing.Size(135, 21);
            this.resourcesLabel.TabIndex = 7;
            this.resourcesLabel.Text = "Useful resources";
            // 
            // whatsNewLink
            // 
            this.whatsNewLink.AutoSize = true;
            this.whatsNewLink.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.whatsNewLink.Location = new System.Drawing.Point(3, 0);
            this.whatsNewLink.Name = "whatsNewLink";
            this.whatsNewLink.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.whatsNewLink.Size = new System.Drawing.Size(147, 18);
            this.whatsNewLink.TabIndex = 4;
            this.whatsNewLink.TabStop = true;
            this.whatsNewLink.Text = "What\'s new in this release?";
            this.whatsNewLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.whatsNewLink_LinkClicked);
            // 
            // WelcomePageView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.linkLayout3);
            this.Controls.Add(this.resourcesLabel);
            this.Controls.Add(this.linkLayout2);
            this.Controls.Add(this.documentationLabel);
            this.Controls.Add(this.linkLayout1);
            this.Controls.Add(this.version);
            this.Controls.Add(this.thingsToDoLabel);
            this.DoubleBuffered = true;
            this.Name = "WelcomePageView";
            this.Size = new System.Drawing.Size(1037, 722);
            this.Load += new System.EventHandler(this.WelcomePageView_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.linkLayout1.ResumeLayout(false);
            this.linkLayout1.PerformLayout();
            this.linkLayout2.ResumeLayout(false);
            this.linkLayout2.PerformLayout();
            this.linkLayout3.ResumeLayout(false);
            this.linkLayout3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.CheckBox dontShow;
        private System.Windows.Forms.Label thingsToDoLabel;
        private System.Windows.Forms.Label version;
        private System.Windows.Forms.FlowLayoutPanel linkLayout1;
        private System.Windows.Forms.LinkLabel codeSamples;
        private System.Windows.Forms.LinkLabel newFile;
        private System.Windows.Forms.LinkLabel configure;
        private System.Windows.Forms.FlowLayoutPanel linkLayout2;
        private System.Windows.Forms.LinkLabel docLink;
        private System.Windows.Forms.LinkLabel codeSamplesLink;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel book;
        private System.Windows.Forms.Label documentationLabel;
        private System.Windows.Forms.FlowLayoutPanel linkLayout3;
        private System.Windows.Forms.LinkLabel homepageLink;
        private System.Windows.Forms.LinkLabel rosettaLink;
        private System.Windows.Forms.LinkLabel ohlohLink;
        private System.Windows.Forms.LinkLabel groupLink;
        private System.Windows.Forms.LinkLabel onlineConsoleLink;
        private System.Windows.Forms.Label resourcesLabel;
        private System.Windows.Forms.LinkLabel whatsNewLink;
    }
}

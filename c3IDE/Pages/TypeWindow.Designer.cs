namespace c3IDE
{
    partial class TypeWindow
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
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config1 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config2 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config3 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config4 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.editTimeEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.editTimeTemplateEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.runTimeEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.runTimeTemplateEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeEditor)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeTemplateEditor)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runTimeEditor)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runTimeTemplateEditor)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl2);
            this.splitContainer1.Size = new System.Drawing.Size(1216, 723);
            this.splitContainer1.SplitterDistance = 626;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(626, 723);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.editTimeEditor);
            this.tabPage1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(618, 690);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Edit Time";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // editTimeEditor
            // 
            this.editTimeEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(98)))));
            this.editTimeEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            this.editTimeEditor.Configurator = config1;
            this.editTimeEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editTimeEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(166)))), ((int)(((byte)(50)))));
            this.editTimeEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            this.editTimeEditor.ContextMenuEnabled = false;
            this.editTimeEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            this.editTimeEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(236))))));
            this.editTimeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTimeEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            this.editTimeEditor.LikeVisualStudioSearch = true;
            this.editTimeEditor.LineNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.editTimeEditor.LineNumbersFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editTimeEditor.Location = new System.Drawing.Point(3, 3);
            this.editTimeEditor.MarkChangedLines = true;
            this.editTimeEditor.Name = "editTimeEditor";
            this.editTimeEditor.RenderRightToLeft = false;
            this.editTimeEditor.SaveOnClose = false;
            this.editTimeEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            this.editTimeEditor.ScrollVisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2016;
            this.editTimeEditor.SelectionTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.editTimeEditor.ShowHorizontalSplitters = false;
            this.editTimeEditor.ShowSelectionMargin = false;
            this.editTimeEditor.ShowVerticalSplitters = false;
            this.editTimeEditor.Size = new System.Drawing.Size(612, 684);
            this.editTimeEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.editTimeEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.editTimeEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.editTimeEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.editTimeEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.editTimeEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.editTimeEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.editTimeEditor.StatusBarSettings.TextPanel.Width = 214;
            this.editTimeEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Office2016Black;
            this.editTimeEditor.Style = Syncfusion.Windows.Forms.Edit.EditControlStyle.Office2016Black;
            this.editTimeEditor.TabIndex = 3;
            this.editTimeEditor.Text = "";
            this.editTimeEditor.UseXPStyle = false;
            this.editTimeEditor.UseXPStyleBorder = true;
            this.editTimeEditor.VisualColumn = 1;
            this.editTimeEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.editTimeEditor.RegisteringDefaultKeyBindings += new System.EventHandler(this.editor_RegisteringDefaultKeyBindings);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.editTimeTemplateEditor);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(618, 690);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Edit Type Template";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // editTimeTemplateEditor
            // 
            this.editTimeTemplateEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(98)))));
            this.editTimeTemplateEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            this.editTimeTemplateEditor.Configurator = config2;
            this.editTimeTemplateEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.editTimeTemplateEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(166)))), ((int)(((byte)(50)))));
            this.editTimeTemplateEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            this.editTimeTemplateEditor.ContextMenuEnabled = false;
            this.editTimeTemplateEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            this.editTimeTemplateEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(236))))));
            this.editTimeTemplateEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTimeTemplateEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            this.editTimeTemplateEditor.LikeVisualStudioSearch = true;
            this.editTimeTemplateEditor.LineNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.editTimeTemplateEditor.LineNumbersFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.editTimeTemplateEditor.Location = new System.Drawing.Point(3, 3);
            this.editTimeTemplateEditor.MarkChangedLines = true;
            this.editTimeTemplateEditor.Name = "editTimeTemplateEditor";
            this.editTimeTemplateEditor.RenderRightToLeft = false;
            this.editTimeTemplateEditor.SaveOnClose = false;
            this.editTimeTemplateEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            this.editTimeTemplateEditor.ScrollVisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2016;
            this.editTimeTemplateEditor.SelectionTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.editTimeTemplateEditor.ShowHorizontalSplitters = false;
            this.editTimeTemplateEditor.ShowSelectionMargin = false;
            this.editTimeTemplateEditor.ShowVerticalSplitters = false;
            this.editTimeTemplateEditor.Size = new System.Drawing.Size(612, 684);
            this.editTimeTemplateEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.editTimeTemplateEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.editTimeTemplateEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.editTimeTemplateEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.editTimeTemplateEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.editTimeTemplateEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.editTimeTemplateEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.editTimeTemplateEditor.StatusBarSettings.TextPanel.Width = 214;
            this.editTimeTemplateEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Office2016Black;
            this.editTimeTemplateEditor.Style = Syncfusion.Windows.Forms.Edit.EditControlStyle.Office2016Black;
            this.editTimeTemplateEditor.TabIndex = 3;
            this.editTimeTemplateEditor.Text = "";
            this.editTimeTemplateEditor.UseXPStyle = false;
            this.editTimeTemplateEditor.UseXPStyleBorder = true;
            this.editTimeTemplateEditor.VisualColumn = 1;
            this.editTimeTemplateEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.editTimeTemplateEditor.RegisteringDefaultKeyBindings += new System.EventHandler(this.editor_RegisteringDefaultKeyBindings);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(586, 723);
            this.tabControl2.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.runTimeEditor);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(578, 690);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Run Time";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // runTimeEditor
            // 
            this.runTimeEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(98)))));
            this.runTimeEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            this.runTimeEditor.Configurator = config3;
            this.runTimeEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.runTimeEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(166)))), ((int)(((byte)(50)))));
            this.runTimeEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            this.runTimeEditor.ContextMenuEnabled = false;
            this.runTimeEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            this.runTimeEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(236))))));
            this.runTimeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runTimeEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            this.runTimeEditor.LikeVisualStudioSearch = true;
            this.runTimeEditor.LineNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.runTimeEditor.LineNumbersFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.runTimeEditor.Location = new System.Drawing.Point(3, 3);
            this.runTimeEditor.MarkChangedLines = true;
            this.runTimeEditor.Name = "runTimeEditor";
            this.runTimeEditor.RenderRightToLeft = false;
            this.runTimeEditor.SaveOnClose = false;
            this.runTimeEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            this.runTimeEditor.ScrollVisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2016;
            this.runTimeEditor.SelectionTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.runTimeEditor.ShowHorizontalSplitters = false;
            this.runTimeEditor.ShowSelectionMargin = false;
            this.runTimeEditor.ShowVerticalSplitters = false;
            this.runTimeEditor.Size = new System.Drawing.Size(572, 684);
            this.runTimeEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.runTimeEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.runTimeEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.runTimeEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.runTimeEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.runTimeEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.runTimeEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.runTimeEditor.StatusBarSettings.TextPanel.Width = 214;
            this.runTimeEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Office2016Black;
            this.runTimeEditor.Style = Syncfusion.Windows.Forms.Edit.EditControlStyle.Office2016Black;
            this.runTimeEditor.TabIndex = 3;
            this.runTimeEditor.Text = "";
            this.runTimeEditor.UseXPStyle = false;
            this.runTimeEditor.UseXPStyleBorder = true;
            this.runTimeEditor.VisualColumn = 1;
            this.runTimeEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.runTimeEditor.RegisteringDefaultKeyBindings += new System.EventHandler(this.editor_RegisteringDefaultKeyBindings);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.runTimeTemplateEditor);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(578, 690);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Run Time Template";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // runTimeTemplateEditor
            // 
            this.runTimeTemplateEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(98)))));
            this.runTimeTemplateEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            this.runTimeTemplateEditor.Configurator = config4;
            this.runTimeTemplateEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.runTimeTemplateEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(166)))), ((int)(((byte)(50)))));
            this.runTimeTemplateEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            this.runTimeTemplateEditor.ContextMenuEnabled = false;
            this.runTimeTemplateEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            this.runTimeTemplateEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(236))))));
            this.runTimeTemplateEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runTimeTemplateEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            this.runTimeTemplateEditor.LikeVisualStudioSearch = true;
            this.runTimeTemplateEditor.LineNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.runTimeTemplateEditor.LineNumbersFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.runTimeTemplateEditor.Location = new System.Drawing.Point(3, 3);
            this.runTimeTemplateEditor.MarkChangedLines = true;
            this.runTimeTemplateEditor.Name = "runTimeTemplateEditor";
            this.runTimeTemplateEditor.RenderRightToLeft = false;
            this.runTimeTemplateEditor.SaveOnClose = false;
            this.runTimeTemplateEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            this.runTimeTemplateEditor.ScrollVisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2016;
            this.runTimeTemplateEditor.SelectionTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.runTimeTemplateEditor.ShowHorizontalSplitters = false;
            this.runTimeTemplateEditor.ShowSelectionMargin = false;
            this.runTimeTemplateEditor.ShowVerticalSplitters = false;
            this.runTimeTemplateEditor.Size = new System.Drawing.Size(572, 684);
            this.runTimeTemplateEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.runTimeTemplateEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.runTimeTemplateEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.runTimeTemplateEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.runTimeTemplateEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.runTimeTemplateEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.runTimeTemplateEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.runTimeTemplateEditor.StatusBarSettings.TextPanel.Width = 214;
            this.runTimeTemplateEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Office2016Black;
            this.runTimeTemplateEditor.Style = Syncfusion.Windows.Forms.Edit.EditControlStyle.Office2016Black;
            this.runTimeTemplateEditor.TabIndex = 3;
            this.runTimeTemplateEditor.Text = "";
            this.runTimeTemplateEditor.UseXPStyle = false;
            this.runTimeTemplateEditor.UseXPStyleBorder = true;
            this.runTimeTemplateEditor.VisualColumn = 1;
            this.runTimeTemplateEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.runTimeTemplateEditor.RegisteringDefaultKeyBindings += new System.EventHandler(this.editor_RegisteringDefaultKeyBindings);
            // 
            // TypeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TypeWindow";
            this.Size = new System.Drawing.Size(1216, 723);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editTimeEditor)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editTimeTemplateEditor)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.runTimeEditor)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.runTimeTemplateEditor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Syncfusion.Windows.Forms.Edit.EditControl editTimeEditor;
        private Syncfusion.Windows.Forms.Edit.EditControl editTimeTemplateEditor;
        private Syncfusion.Windows.Forms.Edit.EditControl runTimeEditor;
        private Syncfusion.Windows.Forms.Edit.EditControl runTimeTemplateEditor;
    }
}

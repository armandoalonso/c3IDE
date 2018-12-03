namespace c3IDE.Pages
{
    partial class AddPropertyWindow
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
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config1 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            Syncfusion.Windows.Forms.Edit.Implementation.Config.Config config2 = new Syncfusion.Windows.Forms.Edit.Implementation.Config.Config();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddPropertyWindow));
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyTemplateEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            this.propertySourceEditor = new Syncfusion.Windows.Forms.Edit.EditControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.propertyIdTextBox = new System.Windows.Forms.TextBox();
            this.propertyValueTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.propertyTypeDropDown = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.propertyNameTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.propertyDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.propertySaveButton = new System.Windows.Forms.Button();
            this.propertyCancelButton = new System.Windows.Forms.Button();
            this.propertyCompileButton = new System.Windows.Forms.Button();
            this.minMaxCheckBox = new System.Windows.Forms.CheckBox();
            this.dragSpeedCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.propertyTemplateEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertySourceEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(950, 19);
            this.HeaderPanel.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(23, 465);
            this.panel1.TabIndex = 3;
            // 
            // propertyTemplateEditor
            // 
            this.propertyTemplateEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(98)))));
            this.propertyTemplateEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            this.propertyTemplateEditor.Configurator = config1;
            this.propertyTemplateEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.propertyTemplateEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(166)))), ((int)(((byte)(50)))));
            this.propertyTemplateEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            this.propertyTemplateEditor.ContextMenuEnabled = false;
            this.propertyTemplateEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            this.propertyTemplateEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(236))))));
            this.propertyTemplateEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            this.propertyTemplateEditor.LikeVisualStudioSearch = true;
            this.propertyTemplateEditor.LineNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.propertyTemplateEditor.LineNumbersFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.propertyTemplateEditor.Location = new System.Drawing.Point(29, 222);
            this.propertyTemplateEditor.MarkChangedLines = true;
            this.propertyTemplateEditor.Name = "propertyTemplateEditor";
            this.propertyTemplateEditor.RenderRightToLeft = false;
            this.propertyTemplateEditor.SaveOnClose = false;
            this.propertyTemplateEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            this.propertyTemplateEditor.ScrollVisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2016;
            this.propertyTemplateEditor.SelectionTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.propertyTemplateEditor.ShowHorizontalSplitters = false;
            this.propertyTemplateEditor.ShowSelectionMargin = false;
            this.propertyTemplateEditor.ShowVerticalSplitters = false;
            this.propertyTemplateEditor.Size = new System.Drawing.Size(909, 91);
            this.propertyTemplateEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.propertyTemplateEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.propertyTemplateEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.propertyTemplateEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.propertyTemplateEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.propertyTemplateEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.propertyTemplateEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.propertyTemplateEditor.StatusBarSettings.TextPanel.Width = 214;
            this.propertyTemplateEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Office2016Black;
            this.propertyTemplateEditor.Style = Syncfusion.Windows.Forms.Edit.EditControlStyle.Office2016Black;
            this.propertyTemplateEditor.TabIndex = 4;
            this.propertyTemplateEditor.Text = "";
            this.propertyTemplateEditor.UseXPStyle = false;
            this.propertyTemplateEditor.UseXPStyleBorder = true;
            this.propertyTemplateEditor.VisualColumn = 1;
            this.propertyTemplateEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.propertyTemplateEditor.WordWrap = true;
            this.propertyTemplateEditor.WordWrapMode = Syncfusion.Windows.Forms.Edit.Enums.WordWrapMode.WordWrapMargin;
            this.propertyTemplateEditor.RegisteringDefaultKeyBindings += new System.EventHandler(this.editor_RegisteringDefaultKeyBindings);
            // 
            // propertySourceEditor
            // 
            this.propertySourceEditor.ChangedLinesMarkingLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(238)))), ((int)(((byte)(98)))));
            this.propertySourceEditor.CodeSnipptSize = new System.Drawing.Size(100, 100);
            this.propertySourceEditor.Configurator = config2;
            this.propertySourceEditor.ContextChoiceBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.propertySourceEditor.ContextChoiceBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(166)))), ((int)(((byte)(50)))));
            this.propertySourceEditor.ContextChoiceForeColor = System.Drawing.SystemColors.InfoText;
            this.propertySourceEditor.ContextMenuEnabled = false;
            this.propertySourceEditor.ContextPromptBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))));
            this.propertySourceEditor.ContextTooltipBackgroundBrush = new Syncfusion.Drawing.BrushInfo(System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(232)))), ((int)(((byte)(236))))));
            this.propertySourceEditor.IndicatorMarginBackColor = System.Drawing.Color.Empty;
            this.propertySourceEditor.LikeVisualStudioSearch = true;
            this.propertySourceEditor.LineNumbersColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.propertySourceEditor.LineNumbersFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.propertySourceEditor.Location = new System.Drawing.Point(29, 344);
            this.propertySourceEditor.MarkChangedLines = true;
            this.propertySourceEditor.Name = "propertySourceEditor";
            this.propertySourceEditor.RenderRightToLeft = false;
            this.propertySourceEditor.SaveOnClose = false;
            this.propertySourceEditor.ScrollPosition = new System.Drawing.Point(0, 0);
            this.propertySourceEditor.ScrollVisualStyle = Syncfusion.Windows.Forms.ScrollBarCustomDrawStyles.Office2016;
            this.propertySourceEditor.SelectionTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(173)))), ((int)(((byte)(214)))), ((int)(((byte)(255)))));
            this.propertySourceEditor.ShowHorizontalSplitters = false;
            this.propertySourceEditor.ShowSelectionMargin = false;
            this.propertySourceEditor.ShowVerticalSplitters = false;
            this.propertySourceEditor.Size = new System.Drawing.Size(909, 83);
            this.propertySourceEditor.StatusBarSettings.CoordsPanel.Width = 150;
            this.propertySourceEditor.StatusBarSettings.EncodingPanel.Width = 100;
            this.propertySourceEditor.StatusBarSettings.FileNamePanel.Width = 100;
            this.propertySourceEditor.StatusBarSettings.InsertPanel.Width = 33;
            this.propertySourceEditor.StatusBarSettings.Offcie2007ColorScheme = Syncfusion.Windows.Forms.Office2007Theme.Blue;
            this.propertySourceEditor.StatusBarSettings.Offcie2010ColorScheme = Syncfusion.Windows.Forms.Office2010Theme.Blue;
            this.propertySourceEditor.StatusBarSettings.StatusPanel.Width = 70;
            this.propertySourceEditor.StatusBarSettings.TextPanel.Width = 214;
            this.propertySourceEditor.StatusBarSettings.VisualStyle = Syncfusion.Windows.Forms.Tools.Controls.StatusBar.VisualStyle.Office2016Black;
            this.propertySourceEditor.Style = Syncfusion.Windows.Forms.Edit.EditControlStyle.Office2016Black;
            this.propertySourceEditor.TabIndex = 4;
            this.propertySourceEditor.Text = "";
            this.propertySourceEditor.UseXPStyle = false;
            this.propertySourceEditor.UseXPStyleBorder = true;
            this.propertySourceEditor.VisualColumn = 1;
            this.propertySourceEditor.VScrollMode = Syncfusion.Windows.Forms.Edit.ScrollMode.Immediate;
            this.propertySourceEditor.WordWrap = true;
            this.propertySourceEditor.WordWrapMode = Syncfusion.Windows.Forms.Edit.Enums.WordWrapMode.WordWrapMargin;
            this.propertySourceEditor.RegisteringDefaultKeyBindings += new System.EventHandler(this.editor_RegisteringDefaultKeyBindings);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(44, 199);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Property Template:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(44, 321);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Plugin Source:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(44, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Property ID:";
            // 
            // propertyIdTextBox
            // 
            this.propertyIdTextBox.Location = new System.Drawing.Point(44, 47);
            this.propertyIdTextBox.Name = "propertyIdTextBox";
            this.propertyIdTextBox.Size = new System.Drawing.Size(404, 26);
            this.propertyIdTextBox.TabIndex = 7;
            this.propertyIdTextBox.TextChanged += new System.EventHandler(this.propertyModel_Changed);
            // 
            // propertyValueTextBox
            // 
            this.propertyValueTextBox.Location = new System.Drawing.Point(44, 103);
            this.propertyValueTextBox.Name = "propertyValueTextBox";
            this.propertyValueTextBox.Size = new System.Drawing.Size(404, 26);
            this.propertyValueTextBox.TabIndex = 9;
            this.propertyValueTextBox.TextChanged += new System.EventHandler(this.propertyModel_Changed);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(44, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Property Type:";
            // 
            // propertyTypeDropDown
            // 
            this.propertyTypeDropDown.FormattingEnabled = true;
            this.propertyTypeDropDown.Items.AddRange(new object[] {
            "integer",
            "float",
            "percent",
            "text",
            "longtext",
            "check",
            "font",
            "combo",
            "color",
            "group",
            "link",
            "linkCallback",
            "info",
            "infoCallback"});
            this.propertyTypeDropDown.Location = new System.Drawing.Point(44, 159);
            this.propertyTypeDropDown.Name = "propertyTypeDropDown";
            this.propertyTypeDropDown.Size = new System.Drawing.Size(404, 28);
            this.propertyTypeDropDown.TabIndex = 10;
            this.propertyTypeDropDown.SelectedIndexChanged += new System.EventHandler(this.propertyTypeDropDown_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(44, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Property Value:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(454, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Property name:";
            // 
            // propertyNameTextBox
            // 
            this.propertyNameTextBox.Location = new System.Drawing.Point(454, 47);
            this.propertyNameTextBox.Name = "propertyNameTextBox";
            this.propertyNameTextBox.Size = new System.Drawing.Size(484, 26);
            this.propertyNameTextBox.TabIndex = 7;
            this.propertyNameTextBox.TextChanged += new System.EventHandler(this.propertyModel_Changed);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(454, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(161, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "Property Description:";
            // 
            // propertyDescriptionTextBox
            // 
            this.propertyDescriptionTextBox.Location = new System.Drawing.Point(454, 103);
            this.propertyDescriptionTextBox.Name = "propertyDescriptionTextBox";
            this.propertyDescriptionTextBox.Size = new System.Drawing.Size(484, 26);
            this.propertyDescriptionTextBox.TabIndex = 9;
            this.propertyDescriptionTextBox.TextChanged += new System.EventHandler(this.propertyModel_Changed);
            // 
            // propertySaveButton
            // 
            this.propertySaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertySaveButton.FlatAppearance.BorderSize = 0;
            this.propertySaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.propertySaveButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertySaveButton.ForeColor = System.Drawing.Color.Silver;
            this.propertySaveButton.Image = ((System.Drawing.Image)(resources.GetObject("propertySaveButton.Image")));
            this.propertySaveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.propertySaveButton.Location = new System.Drawing.Point(90, 433);
            this.propertySaveButton.Name = "propertySaveButton";
            this.propertySaveButton.Size = new System.Drawing.Size(37, 39);
            this.propertySaveButton.TabIndex = 11;
            this.propertySaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propertySaveButton.UseVisualStyleBackColor = true;
            this.propertySaveButton.Click += new System.EventHandler(this.propertySaveButton_Click);
            // 
            // propertyCancelButton
            // 
            this.propertyCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyCancelButton.FlatAppearance.BorderSize = 0;
            this.propertyCancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.propertyCancelButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyCancelButton.ForeColor = System.Drawing.Color.Silver;
            this.propertyCancelButton.Image = ((System.Drawing.Image)(resources.GetObject("propertyCancelButton.Image")));
            this.propertyCancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.propertyCancelButton.Location = new System.Drawing.Point(901, 433);
            this.propertyCancelButton.Name = "propertyCancelButton";
            this.propertyCancelButton.Size = new System.Drawing.Size(37, 39);
            this.propertyCancelButton.TabIndex = 11;
            this.propertyCancelButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propertyCancelButton.UseVisualStyleBackColor = true;
            this.propertyCancelButton.Click += new System.EventHandler(this.propertyCancelButton_Click);
            // 
            // propertyCompileButton
            // 
            this.propertyCompileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyCompileButton.FlatAppearance.BorderSize = 0;
            this.propertyCompileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.propertyCompileButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.propertyCompileButton.ForeColor = System.Drawing.Color.Silver;
            this.propertyCompileButton.Image = ((System.Drawing.Image)(resources.GetObject("propertyCompileButton.Image")));
            this.propertyCompileButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.propertyCompileButton.Location = new System.Drawing.Point(47, 433);
            this.propertyCompileButton.Name = "propertyCompileButton";
            this.propertyCompileButton.Size = new System.Drawing.Size(37, 39);
            this.propertyCompileButton.TabIndex = 11;
            this.propertyCompileButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.propertyCompileButton.UseVisualStyleBackColor = true;
            this.propertyCompileButton.Click += new System.EventHandler(this.propertyCompileButton_Click);
            // 
            // minMaxCheckBox
            // 
            this.minMaxCheckBox.AutoSize = true;
            this.minMaxCheckBox.Location = new System.Drawing.Point(466, 159);
            this.minMaxCheckBox.Name = "minMaxCheckBox";
            this.minMaxCheckBox.Size = new System.Drawing.Size(94, 24);
            this.minMaxCheckBox.TabIndex = 12;
            this.minMaxCheckBox.Text = "Min/Max";
            this.minMaxCheckBox.UseVisualStyleBackColor = true;
            this.minMaxCheckBox.CheckedChanged += new System.EventHandler(this.propCheckBox_CheckedChanged);
            // 
            // dragSpeedCheckBox
            // 
            this.dragSpeedCheckBox.AutoSize = true;
            this.dragSpeedCheckBox.Location = new System.Drawing.Point(566, 159);
            this.dragSpeedCheckBox.Name = "dragSpeedCheckBox";
            this.dragSpeedCheckBox.Size = new System.Drawing.Size(115, 24);
            this.dragSpeedCheckBox.TabIndex = 12;
            this.dragSpeedCheckBox.Text = "Drag Speed";
            this.dragSpeedCheckBox.UseVisualStyleBackColor = true;
            this.dragSpeedCheckBox.CheckedChanged += new System.EventHandler(this.propCheckBox_CheckedChanged);
            // 
            // AddPropertyWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 484);
            this.Controls.Add(this.dragSpeedCheckBox);
            this.Controls.Add(this.minMaxCheckBox);
            this.Controls.Add(this.propertyCancelButton);
            this.Controls.Add(this.propertyCompileButton);
            this.Controls.Add(this.propertySaveButton);
            this.Controls.Add(this.propertyTypeDropDown);
            this.Controls.Add(this.propertyDescriptionTextBox);
            this.Controls.Add(this.propertyValueTextBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.propertyNameTextBox);
            this.Controls.Add(this.propertyIdTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.propertySourceEditor);
            this.Controls.Add(this.propertyTemplateEditor);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.HeaderPanel);
            this.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPropertyWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Property Editor";
            ((System.ComponentModel.ISupportInitialize)(this.propertyTemplateEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.propertySourceEditor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Panel panel1;
        private Syncfusion.Windows.Forms.Edit.EditControl propertyTemplateEditor;
        private Syncfusion.Windows.Forms.Edit.EditControl propertySourceEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox propertyIdTextBox;
        private System.Windows.Forms.TextBox propertyValueTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox propertyTypeDropDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox propertyNameTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox propertyDescriptionTextBox;
        private System.Windows.Forms.Button propertySaveButton;
        private System.Windows.Forms.Button propertyCancelButton;
        private System.Windows.Forms.Button propertyCompileButton;
        private System.Windows.Forms.CheckBox minMaxCheckBox;
        private System.Windows.Forms.CheckBox dragSpeedCheckBox;
    }
}
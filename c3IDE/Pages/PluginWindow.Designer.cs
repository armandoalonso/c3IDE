namespace c3IDE
{
    partial class PluginWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginWindow));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pluginCategoryDropDown = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pluginVersionTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pluginAuthorTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pluginCompanyTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pluginNameTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.editTimeEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.runTimeEditor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.pluginDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.iconImage = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.propertiesListBox = new System.Windows.Forms.ListBox();
            this.addPropertyButton = new System.Windows.Forms.Button();
            this.deltePropertyButton = new System.Windows.Forms.Button();
            this.editPropertyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editTimeEditor)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runTimeEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconImage)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.editPropertyButton);
            this.splitContainer1.Panel1.Controls.Add(this.deltePropertyButton);
            this.splitContainer1.Panel1.Controls.Add(this.addPropertyButton);
            this.splitContainer1.Panel1.Controls.Add(this.propertiesListBox);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.iconImage);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.pluginDescriptionTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.pluginCategoryDropDown);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.pluginVersionTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.pluginAuthorTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.pluginCompanyTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.pluginNameTextbox);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1216, 723);
            this.splitContainer1.SplitterDistance = 320;
            this.splitContainer1.TabIndex = 0;
            // 
            // pluginCategoryDropDown
            // 
            this.pluginCategoryDropDown.FormattingEnabled = true;
            this.pluginCategoryDropDown.Items.AddRange(new object[] {
            "general",
            "form-controls",
            "data-and-storage",
            "input",
            "media",
            "monetisation",
            "platform-specific",
            "web ",
            "other"});
            this.pluginCategoryDropDown.Location = new System.Drawing.Point(8, 301);
            this.pluginCategoryDropDown.Name = "pluginCategoryDropDown";
            this.pluginCategoryDropDown.Size = new System.Drawing.Size(310, 21);
            this.pluginCategoryDropDown.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Catgory:";
            // 
            // pluginVersionTextBox
            // 
            this.pluginVersionTextBox.Location = new System.Drawing.Point(8, 179);
            this.pluginVersionTextBox.Name = "pluginVersionTextBox";
            this.pluginVersionTextBox.Size = new System.Drawing.Size(310, 20);
            this.pluginVersionTextBox.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Version:";
            // 
            // pluginAuthorTextBox
            // 
            this.pluginAuthorTextBox.Location = new System.Drawing.Point(8, 131);
            this.pluginAuthorTextBox.Name = "pluginAuthorTextBox";
            this.pluginAuthorTextBox.Size = new System.Drawing.Size(310, 20);
            this.pluginAuthorTextBox.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Author:";
            // 
            // pluginCompanyTextBox
            // 
            this.pluginCompanyTextBox.Location = new System.Drawing.Point(8, 83);
            this.pluginCompanyTextBox.Name = "pluginCompanyTextBox";
            this.pluginCompanyTextBox.Size = new System.Drawing.Size(310, 20);
            this.pluginCompanyTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Company:";
            // 
            // pluginNameTextbox
            // 
            this.pluginNameTextbox.Location = new System.Drawing.Point(8, 35);
            this.pluginNameTextbox.Name = "pluginNameTextbox";
            this.pluginNameTextbox.Size = new System.Drawing.Size(310, 20);
            this.pluginNameTextbox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Plugin Name:";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(892, 723);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.editTimeEditor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(884, 697);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "     Edit Time     ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // editTimeEditor
            // 
            this.editTimeEditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.editTimeEditor.AutoScrollMinSize = new System.Drawing.Size(179, 14);
            this.editTimeEditor.BackBrush = null;
            this.editTimeEditor.CharHeight = 14;
            this.editTimeEditor.CharWidth = 8;
            this.editTimeEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.editTimeEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.editTimeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editTimeEditor.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.editTimeEditor.IsReplaceMode = false;
            this.editTimeEditor.Location = new System.Drawing.Point(3, 3);
            this.editTimeEditor.Name = "editTimeEditor";
            this.editTimeEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.editTimeEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editTimeEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editTimeEditor.ServiceColors")));
            this.editTimeEditor.Size = new System.Drawing.Size(878, 691);
            this.editTimeEditor.TabIndex = 0;
            this.editTimeEditor.Text = "fastColoredTextBox1";
            this.editTimeEditor.Zoom = 100;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.runTimeEditor);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(884, 697);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "     Run Time     ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // runTimeEditor
            // 
            this.runTimeEditor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.runTimeEditor.AutoScrollMinSize = new System.Drawing.Size(154, 14);
            this.runTimeEditor.BackBrush = null;
            this.runTimeEditor.CharHeight = 14;
            this.runTimeEditor.CharWidth = 8;
            this.runTimeEditor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.runTimeEditor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.runTimeEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runTimeEditor.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.runTimeEditor.IsReplaceMode = false;
            this.runTimeEditor.Location = new System.Drawing.Point(3, 3);
            this.runTimeEditor.Name = "runTimeEditor";
            this.runTimeEditor.Paddings = new System.Windows.Forms.Padding(0);
            this.runTimeEditor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.runTimeEditor.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("runTimeEditor.ServiceColors")));
            this.runTimeEditor.Size = new System.Drawing.Size(878, 691);
            this.runTimeEditor.TabIndex = 0;
            this.runTimeEditor.Text = "fastColoredTextBox1";
            this.runTimeEditor.Zoom = 100;
            // 
            // pluginDescriptionTextBox
            // 
            this.pluginDescriptionTextBox.Location = new System.Drawing.Point(8, 227);
            this.pluginDescriptionTextBox.Multiline = true;
            this.pluginDescriptionTextBox.Name = "pluginDescriptionTextBox";
            this.pluginDescriptionTextBox.Size = new System.Drawing.Size(310, 46);
            this.pluginDescriptionTextBox.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(8, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 20);
            this.label6.TabIndex = 3;
            this.label6.Text = "Description:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 325);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 20);
            this.label7.TabIndex = 5;
            this.label7.Text = "Icon:";
            // 
            // iconImage
            // 
            this.iconImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconImage.Location = new System.Drawing.Point(8, 348);
            this.iconImage.Name = "iconImage";
            this.iconImage.Size = new System.Drawing.Size(125, 125);
            this.iconImage.TabIndex = 6;
            this.iconImage.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 476);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 20);
            this.label8.TabIndex = 7;
            this.label8.Text = "Properties:";
            // 
            // propertiesListBox
            // 
            this.propertiesListBox.FormattingEnabled = true;
            this.propertiesListBox.Location = new System.Drawing.Point(8, 499);
            this.propertiesListBox.Name = "propertiesListBox";
            this.propertiesListBox.Size = new System.Drawing.Size(309, 173);
            this.propertiesListBox.TabIndex = 8;
            // 
            // addPropertyButton
            // 
            this.addPropertyButton.Image = ((System.Drawing.Image)(resources.GetObject("addPropertyButton.Image")));
            this.addPropertyButton.Location = new System.Drawing.Point(8, 679);
            this.addPropertyButton.Name = "addPropertyButton";
            this.addPropertyButton.Size = new System.Drawing.Size(40, 40);
            this.addPropertyButton.TabIndex = 9;
            this.addPropertyButton.UseVisualStyleBackColor = true;
            this.addPropertyButton.Click += new System.EventHandler(this.addPropertyButton_Click);
            // 
            // deltePropertyButton
            // 
            this.deltePropertyButton.Image = ((System.Drawing.Image)(resources.GetObject("deltePropertyButton.Image")));
            this.deltePropertyButton.Location = new System.Drawing.Point(100, 679);
            this.deltePropertyButton.Name = "deltePropertyButton";
            this.deltePropertyButton.Size = new System.Drawing.Size(40, 40);
            this.deltePropertyButton.TabIndex = 9;
            this.deltePropertyButton.UseVisualStyleBackColor = true;
            this.deltePropertyButton.Click += new System.EventHandler(this.deltePropertyButton_Click);
            // 
            // editPropertyButton
            // 
            this.editPropertyButton.Image = ((System.Drawing.Image)(resources.GetObject("editPropertyButton.Image")));
            this.editPropertyButton.Location = new System.Drawing.Point(54, 679);
            this.editPropertyButton.Name = "editPropertyButton";
            this.editPropertyButton.Size = new System.Drawing.Size(40, 40);
            this.editPropertyButton.TabIndex = 10;
            this.editPropertyButton.UseVisualStyleBackColor = true;
            this.editPropertyButton.Click += new System.EventHandler(this.editPropertyButton_Click);
            // 
            // PluginWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PluginWindow";
            this.Size = new System.Drawing.Size(1216, 723);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editTimeEditor)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.runTimeEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox pluginNameTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private FastColoredTextBoxNS.FastColoredTextBox editTimeEditor;
        private System.Windows.Forms.TabPage tabPage2;
        private FastColoredTextBoxNS.FastColoredTextBox runTimeEditor;
        private System.Windows.Forms.ComboBox pluginCategoryDropDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox pluginVersionTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox pluginAuthorTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pluginCompanyTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox pluginDescriptionTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button deltePropertyButton;
        private System.Windows.Forms.Button addPropertyButton;
        private System.Windows.Forms.ListBox propertiesListBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox iconImage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button editPropertyButton;
    }
}

namespace c3IDE
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ActivePanel = new System.Windows.Forms.Panel();
            this.TestButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ExpressionButton = new System.Windows.Forms.Button();
            this.ConditionButton = new System.Windows.Forms.Button();
            this.ActionButton = new System.Windows.Forms.Button();
            this.InstanceButton = new System.Windows.Forms.Button();
            this.TypeButton = new System.Windows.Forms.Button();
            this.PluginButton = new System.Windows.Forms.Button();
            this.HomeButton = new System.Windows.Forms.Button();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.ExitButton = new System.Windows.Forms.Button();
            this.HeaderPanel = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.homeWindow = new c3IDE.Pages.HomeWindow();
            this.pluginWindow = new c3IDE.PluginWindow();
            this.typeWindow = new c3IDE.TypeWindow();
            this.instanceWindow = new c3IDE.Pages.InstanceWindow();
            this.actionsWindow = new c3IDE.Pages.ActionsWindow();
            this.conditionsWindow = new c3IDE.Pages.ConditionsWindow();
            this.expressionsWindow = new c3IDE.Pages.ExpressionsWindow();
            this.testWindow = new c3IDE.Pages.TestWindow();
            this.exportWindow = new c3IDE.Pages.ExportWindow();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(39)))), ((int)(((byte)(40)))));
            this.panel1.Controls.Add(this.ActivePanel);
            this.panel1.Controls.Add(this.TestButton);
            this.panel1.Controls.Add(this.ExportButton);
            this.panel1.Controls.Add(this.ExpressionButton);
            this.panel1.Controls.Add(this.ConditionButton);
            this.panel1.Controls.Add(this.ActionButton);
            this.panel1.Controls.Add(this.InstanceButton);
            this.panel1.Controls.Add(this.TypeButton);
            this.panel1.Controls.Add(this.PluginButton);
            this.panel1.Controls.Add(this.HomeButton);
            this.panel1.Controls.Add(this.TitleLabel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 780);
            this.panel1.TabIndex = 0;
            // 
            // ActivePanel
            // 
            this.ActivePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.ActivePanel.Location = new System.Drawing.Point(-2, 86);
            this.ActivePanel.Name = "ActivePanel";
            this.ActivePanel.Size = new System.Drawing.Size(10, 54);
            this.ActivePanel.TabIndex = 2;
            // 
            // TestButton
            // 
            this.TestButton.Enabled = false;
            this.TestButton.FlatAppearance.BorderSize = 0;
            this.TestButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TestButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestButton.ForeColor = System.Drawing.Color.DimGray;
            this.TestButton.Image = ((System.Drawing.Image)(resources.GetObject("TestButton.Image")));
            this.TestButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TestButton.Location = new System.Drawing.Point(31, 506);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(178, 54);
            this.TestButton.TabIndex = 2;
            this.TestButton.Text = "      Test";
            this.TestButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Enabled = false;
            this.ExportButton.FlatAppearance.BorderSize = 0;
            this.ExportButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExportButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportButton.ForeColor = System.Drawing.Color.DimGray;
            this.ExportButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportButton.Image")));
            this.ExportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExportButton.Location = new System.Drawing.Point(31, 566);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(178, 54);
            this.ExportButton.TabIndex = 2;
            this.ExportButton.Text = "      Export";
            this.ExportButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // ExpressionButton
            // 
            this.ExpressionButton.Enabled = false;
            this.ExpressionButton.FlatAppearance.BorderSize = 0;
            this.ExpressionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExpressionButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExpressionButton.ForeColor = System.Drawing.Color.DimGray;
            this.ExpressionButton.Image = ((System.Drawing.Image)(resources.GetObject("ExpressionButton.Image")));
            this.ExpressionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExpressionButton.Location = new System.Drawing.Point(31, 446);
            this.ExpressionButton.Name = "ExpressionButton";
            this.ExpressionButton.Size = new System.Drawing.Size(178, 54);
            this.ExpressionButton.TabIndex = 2;
            this.ExpressionButton.Text = "      Expressions";
            this.ExpressionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ExpressionButton.UseVisualStyleBackColor = true;
            this.ExpressionButton.Click += new System.EventHandler(this.ExpressionButton_Click);
            // 
            // ConditionButton
            // 
            this.ConditionButton.Enabled = false;
            this.ConditionButton.FlatAppearance.BorderSize = 0;
            this.ConditionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConditionButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConditionButton.ForeColor = System.Drawing.Color.DimGray;
            this.ConditionButton.Image = ((System.Drawing.Image)(resources.GetObject("ConditionButton.Image")));
            this.ConditionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ConditionButton.Location = new System.Drawing.Point(31, 386);
            this.ConditionButton.Name = "ConditionButton";
            this.ConditionButton.Size = new System.Drawing.Size(178, 54);
            this.ConditionButton.TabIndex = 2;
            this.ConditionButton.Text = "      Conditions";
            this.ConditionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ConditionButton.UseVisualStyleBackColor = true;
            this.ConditionButton.Click += new System.EventHandler(this.ConditionButton_Click);
            // 
            // ActionButton
            // 
            this.ActionButton.Enabled = false;
            this.ActionButton.FlatAppearance.BorderSize = 0;
            this.ActionButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ActionButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionButton.ForeColor = System.Drawing.Color.DimGray;
            this.ActionButton.Image = ((System.Drawing.Image)(resources.GetObject("ActionButton.Image")));
            this.ActionButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ActionButton.Location = new System.Drawing.Point(31, 326);
            this.ActionButton.Name = "ActionButton";
            this.ActionButton.Size = new System.Drawing.Size(178, 54);
            this.ActionButton.TabIndex = 2;
            this.ActionButton.Text = "      Actions";
            this.ActionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ActionButton.UseVisualStyleBackColor = true;
            this.ActionButton.Click += new System.EventHandler(this.ActionButton_Click);
            // 
            // InstanceButton
            // 
            this.InstanceButton.Enabled = false;
            this.InstanceButton.FlatAppearance.BorderSize = 0;
            this.InstanceButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InstanceButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstanceButton.ForeColor = System.Drawing.Color.DimGray;
            this.InstanceButton.Image = ((System.Drawing.Image)(resources.GetObject("InstanceButton.Image")));
            this.InstanceButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InstanceButton.Location = new System.Drawing.Point(31, 266);
            this.InstanceButton.Name = "InstanceButton";
            this.InstanceButton.Size = new System.Drawing.Size(178, 54);
            this.InstanceButton.TabIndex = 2;
            this.InstanceButton.Text = "      Instance";
            this.InstanceButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.InstanceButton.UseVisualStyleBackColor = true;
            this.InstanceButton.Click += new System.EventHandler(this.InstanceButton_Click);
            // 
            // TypeButton
            // 
            this.TypeButton.Enabled = false;
            this.TypeButton.FlatAppearance.BorderSize = 0;
            this.TypeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TypeButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TypeButton.ForeColor = System.Drawing.Color.DimGray;
            this.TypeButton.Image = ((System.Drawing.Image)(resources.GetObject("TypeButton.Image")));
            this.TypeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TypeButton.Location = new System.Drawing.Point(31, 206);
            this.TypeButton.Name = "TypeButton";
            this.TypeButton.Size = new System.Drawing.Size(178, 54);
            this.TypeButton.TabIndex = 2;
            this.TypeButton.Text = "      Type";
            this.TypeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.TypeButton.UseVisualStyleBackColor = true;
            this.TypeButton.Click += new System.EventHandler(this.TypeButton_Click);
            // 
            // PluginButton
            // 
            this.PluginButton.Enabled = false;
            this.PluginButton.FlatAppearance.BorderSize = 0;
            this.PluginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PluginButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PluginButton.ForeColor = System.Drawing.Color.DimGray;
            this.PluginButton.Image = ((System.Drawing.Image)(resources.GetObject("PluginButton.Image")));
            this.PluginButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PluginButton.Location = new System.Drawing.Point(31, 146);
            this.PluginButton.Name = "PluginButton";
            this.PluginButton.Size = new System.Drawing.Size(178, 54);
            this.PluginButton.TabIndex = 2;
            this.PluginButton.Text = "      Plugin";
            this.PluginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.PluginButton.UseVisualStyleBackColor = true;
            this.PluginButton.Click += new System.EventHandler(this.PluginButton_Click);
            // 
            // HomeButton
            // 
            this.HomeButton.FlatAppearance.BorderSize = 0;
            this.HomeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HomeButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HomeButton.ForeColor = System.Drawing.Color.White;
            this.HomeButton.Image = ((System.Drawing.Image)(resources.GetObject("HomeButton.Image")));
            this.HomeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.HomeButton.Location = new System.Drawing.Point(31, 86);
            this.HomeButton.Name = "HomeButton";
            this.HomeButton.Size = new System.Drawing.Size(178, 54);
            this.HomeButton.TabIndex = 2;
            this.HomeButton.Text = "      Home";
            this.HomeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.HomeButton.UseVisualStyleBackColor = true;
            this.HomeButton.Click += new System.EventHandler(this.HomeButton_Click);
            // 
            // TitleLabel
            // 
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Century Gothic", 24F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.TitleLabel.Location = new System.Drawing.Point(43, 9);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(103, 38);
            this.TitleLabel.TabIndex = 2;
            this.TitleLabel.Text = "c3IDE";
            // 
            // ExitButton
            // 
            this.ExitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitButton.BackColor = System.Drawing.Color.Transparent;
            this.ExitButton.FlatAppearance.BorderSize = 0;
            this.ExitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ExitButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExitButton.ForeColor = System.Drawing.Color.Silver;
            this.ExitButton.Image = ((System.Drawing.Image)(resources.GetObject("ExitButton.Image")));
            this.ExitButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ExitButton.Location = new System.Drawing.Point(1451, 16);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(37, 39);
            this.ExitButton.TabIndex = 2;
            this.ExitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // HeaderPanel
            // 
            this.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(178)))), ((int)(((byte)(8)))), ((int)(((byte)(55)))));
            this.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.HeaderPanel.Location = new System.Drawing.Point(212, 0);
            this.HeaderPanel.Name = "HeaderPanel";
            this.HeaderPanel.Size = new System.Drawing.Size(1288, 15);
            this.HeaderPanel.TabIndex = 1;
            this.HeaderPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HeaderPanel_MouseDown);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.FlatAppearance.BorderSize = 0;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.ForeColor = System.Drawing.Color.Silver;
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SaveButton.Location = new System.Drawing.Point(1404, 16);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(37, 39);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // homeWindow
            // 
            this.homeWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.homeWindow.Location = new System.Drawing.Point(212, 57);
            this.homeWindow.Name = "homeWindow";
            this.homeWindow.Size = new System.Drawing.Size(1288, 723);
            this.homeWindow.TabIndex = 11;
            // 
            // pluginWindow
            // 
            this.pluginWindow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pluginWindow.Location = new System.Drawing.Point(212, 57);
            this.pluginWindow.Name = "pluginWindow";
            this.pluginWindow.Size = new System.Drawing.Size(1288, 723);
            this.pluginWindow.TabIndex = 10;
            // 
            // typeWindow
            // 
            this.typeWindow.Location = new System.Drawing.Point(212, 57);
            this.typeWindow.Name = "typeWindow";
            this.typeWindow.Size = new System.Drawing.Size(1216, 723);
            this.typeWindow.TabIndex = 9;
            // 
            // instanceWindow
            // 
            this.instanceWindow.Location = new System.Drawing.Point(212, 57);
            this.instanceWindow.Name = "instanceWindow";
            this.instanceWindow.Size = new System.Drawing.Size(1216, 723);
            this.instanceWindow.TabIndex = 8;
            // 
            // actionsWindow
            // 
            this.actionsWindow.Location = new System.Drawing.Point(212, 57);
            this.actionsWindow.Name = "actionsWindow";
            this.actionsWindow.Size = new System.Drawing.Size(1216, 723);
            this.actionsWindow.TabIndex = 7;
            // 
            // conditionsWindow
            // 
            this.conditionsWindow.Location = new System.Drawing.Point(212, 57);
            this.conditionsWindow.Name = "conditionsWindow";
            this.conditionsWindow.Size = new System.Drawing.Size(1216, 723);
            this.conditionsWindow.TabIndex = 6;
            // 
            // expressionsWindow
            // 
            this.expressionsWindow.Location = new System.Drawing.Point(212, 57);
            this.expressionsWindow.Name = "expressionsWindow";
            this.expressionsWindow.Size = new System.Drawing.Size(1216, 723);
            this.expressionsWindow.TabIndex = 5;
            // 
            // testWindow
            // 
            this.testWindow.Location = new System.Drawing.Point(212, 57);
            this.testWindow.Name = "testWindow";
            this.testWindow.Size = new System.Drawing.Size(1216, 723);
            this.testWindow.TabIndex = 4;
            // 
            // exportWindow
            // 
            this.exportWindow.Location = new System.Drawing.Point(212, 57);
            this.exportWindow.Name = "exportWindow";
            this.exportWindow.Size = new System.Drawing.Size(1216, 723);
            this.exportWindow.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 780);
            this.Controls.Add(this.homeWindow);
            this.Controls.Add(this.pluginWindow);
            this.Controls.Add(this.typeWindow);
            this.Controls.Add(this.instanceWindow);
            this.Controls.Add(this.actionsWindow);
            this.Controls.Add(this.conditionsWindow);
            this.Controls.Add(this.expressionsWindow);
            this.Controls.Add(this.testWindow);
            this.Controls.Add(this.exportWindow);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.HeaderPanel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "c3IDE ";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel HeaderPanel;
        private System.Windows.Forms.Label TitleLabel;
        private System.Windows.Forms.Panel ActivePanel;
        private System.Windows.Forms.Button ExpressionButton;
        private System.Windows.Forms.Button ConditionButton;
        private System.Windows.Forms.Button ActionButton;
        private System.Windows.Forms.Button InstanceButton;
        private System.Windows.Forms.Button TypeButton;
        private System.Windows.Forms.Button PluginButton;
        private System.Windows.Forms.Button HomeButton;
        private System.Windows.Forms.Button ExitButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button TestButton;
        private Pages.ExportWindow exportWindow;
        private Pages.TestWindow testWindow;
        private Pages.ExpressionsWindow expressionsWindow;
        private Pages.ConditionsWindow conditionsWindow;
        private Pages.ActionsWindow actionsWindow;
        private Pages.InstanceWindow instanceWindow;
        private TypeWindow typeWindow;
        private PluginWindow pluginWindow;
        private Pages.HomeWindow homeWindow;
        private System.Windows.Forms.Button SaveButton;
    }
}


namespace c3IDE.Pages
{
    partial class HomeButton
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeButton));
            this.pluginButton = new System.Windows.Forms.Button();
            this.pluginNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pluginButton
            // 
            this.pluginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pluginButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.pluginButton.FlatAppearance.BorderSize = 2;
            this.pluginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pluginButton.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pluginButton.Image = ((System.Drawing.Image)(resources.GetObject("pluginButton.Image")));
            this.pluginButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.pluginButton.Location = new System.Drawing.Point(9, 8);
            this.pluginButton.Margin = new System.Windows.Forms.Padding(10);
            this.pluginButton.Name = "pluginButton";
            this.pluginButton.Size = new System.Drawing.Size(162, 169);
            this.pluginButton.TabIndex = 2;
            this.pluginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pluginButton.UseVisualStyleBackColor = true;
            this.pluginButton.Click += new System.EventHandler(this.pluginButton_Click);
            // 
            // pluginNameLabel
            // 
            this.pluginNameLabel.Font = new System.Drawing.Font("Century Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pluginNameLabel.Location = new System.Drawing.Point(5, 187);
            this.pluginNameLabel.Name = "pluginNameLabel";
            this.pluginNameLabel.Size = new System.Drawing.Size(166, 54);
            this.pluginNameLabel.TabIndex = 4;
            this.pluginNameLabel.Text = "Single Global Plugin";
            this.pluginNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pluginNameLabel.Click += new System.EventHandler(this.pluginButton_Click);
            // 
            // HomeButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pluginNameLabel);
            this.Controls.Add(this.pluginButton);
            this.Name = "HomeButton";
            this.Size = new System.Drawing.Size(181, 255);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button pluginButton;
        private System.Windows.Forms.Label pluginNameLabel;
    }
}

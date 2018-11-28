namespace c3IDE.Pages
{
    partial class HomeWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeWindow));
            this.homePluginContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.newSingleGlobalPluginButton = new System.Windows.Forms.Button();
            this.homePluginContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // homePluginContainer
            // 
            this.homePluginContainer.Controls.Add(this.newSingleGlobalPluginButton);
            this.homePluginContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homePluginContainer.Location = new System.Drawing.Point(0, 0);
            this.homePluginContainer.Name = "homePluginContainer";
            this.homePluginContainer.Size = new System.Drawing.Size(1216, 723);
            this.homePluginContainer.TabIndex = 0;
            // 
            // newSingleGlobalPluginButton
            // 
            this.newSingleGlobalPluginButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.newSingleGlobalPluginButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.newSingleGlobalPluginButton.FlatAppearance.BorderSize = 2;
            this.newSingleGlobalPluginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newSingleGlobalPluginButton.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newSingleGlobalPluginButton.Image = ((System.Drawing.Image)(resources.GetObject("newSingleGlobalPluginButton.Image")));
            this.newSingleGlobalPluginButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.newSingleGlobalPluginButton.Location = new System.Drawing.Point(10, 10);
            this.newSingleGlobalPluginButton.Margin = new System.Windows.Forms.Padding(10);
            this.newSingleGlobalPluginButton.Name = "newSingleGlobalPluginButton";
            this.newSingleGlobalPluginButton.Size = new System.Drawing.Size(162, 208);
            this.newSingleGlobalPluginButton.TabIndex = 0;
            this.newSingleGlobalPluginButton.Text = "Single Global Plugin";
            this.newSingleGlobalPluginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.newSingleGlobalPluginButton.UseVisualStyleBackColor = true;
            this.newSingleGlobalPluginButton.Click += new System.EventHandler(this.NewSingleGlobalPluginButton_Click);
            // 
            // HomeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.homePluginContainer);
            this.Name = "HomeWindow";
            this.Size = new System.Drawing.Size(1216, 723);
            this.homePluginContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel homePluginContainer;
        private System.Windows.Forms.Button newSingleGlobalPluginButton;
    }
}

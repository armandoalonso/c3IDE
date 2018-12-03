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
            this.homePluginContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.newSingleGlobalButton = new c3IDE.Pages.HomeButton();
            this.homePluginContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // homePluginContainer
            // 
            this.homePluginContainer.AutoScroll = true;
            this.homePluginContainer.Controls.Add(this.newSingleGlobalButton);
            this.homePluginContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.homePluginContainer.Location = new System.Drawing.Point(0, 0);
            this.homePluginContainer.Name = "homePluginContainer";
            this.homePluginContainer.Size = new System.Drawing.Size(1216, 723);
            this.homePluginContainer.TabIndex = 0;
            // 
            // newSingleGlobalButton
            // 
            this.newSingleGlobalButton.Location = new System.Drawing.Point(3, 3);
            this.newSingleGlobalButton.Name = "newSingleGlobalButton";
            this.newSingleGlobalButton.Size = new System.Drawing.Size(181, 255);
            this.newSingleGlobalButton.TabIndex = 1;
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
        private HomeButton newSingleGlobalButton;
    }
}

namespace c3IDE.Pages
{
    partial class OptionWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionWindow));
            this.deletePluginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deletePluginButton
            // 
            this.deletePluginButton.Enabled = false;
            this.deletePluginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deletePluginButton.Font = new System.Drawing.Font("Century Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deletePluginButton.ForeColor = System.Drawing.Color.DimGray;
            this.deletePluginButton.Image = ((System.Drawing.Image)(resources.GetObject("deletePluginButton.Image")));
            this.deletePluginButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.deletePluginButton.Location = new System.Drawing.Point(17, 26);
            this.deletePluginButton.Name = "deletePluginButton";
            this.deletePluginButton.Size = new System.Drawing.Size(194, 61);
            this.deletePluginButton.TabIndex = 3;
            this.deletePluginButton.Text = "      Delete Plugin";
            this.deletePluginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deletePluginButton.UseVisualStyleBackColor = true;
            // 
            // OptionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.deletePluginButton);
            this.Name = "OptionWindow";
            this.Size = new System.Drawing.Size(1216, 723);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button deletePluginButton;
    }
}

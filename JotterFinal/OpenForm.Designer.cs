namespace JotterFinal
{
    partial class OpenForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenForm));
            this.TitleLabel1 = new System.Windows.Forms.Label();
            this.ContinueButton = new System.Windows.Forms.Button();
            this.TitleLabel2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TitleLabel1
            // 
            this.TitleLabel1.AutoSize = true;
            this.TitleLabel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.TitleLabel1.Font = new System.Drawing.Font("Microsoft JhengHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.TitleLabel1.Location = new System.Drawing.Point(358, 153);
            this.TitleLabel1.Name = "TitleLabel1";
            this.TitleLabel1.Size = new System.Drawing.Size(160, 30);
            this.TitleLabel1.TabIndex = 0;
            this.TitleLabel1.Text = "Welcome To:\r\n";
            this.TitleLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ContinueButton
            // 
            this.ContinueButton.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ContinueButton.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ContinueButton.ForeColor = System.Drawing.Color.RoyalBlue;
            this.ContinueButton.Location = new System.Drawing.Point(372, 312);
            this.ContinueButton.Name = "ContinueButton";
            this.ContinueButton.Size = new System.Drawing.Size(115, 38);
            this.ContinueButton.TabIndex = 2;
            this.ContinueButton.Text = "Continue";
            this.ContinueButton.UseVisualStyleBackColor = false;
            this.ContinueButton.Click += new System.EventHandler(this.ContinueButton_Click);
            // 
            // TitleLabel2
            // 
            this.TitleLabel2.AutoSize = true;
            this.TitleLabel2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.TitleLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TitleLabel2.Font = new System.Drawing.Font("Microsoft YaHei UI", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel2.ForeColor = System.Drawing.Color.MediumBlue;
            this.TitleLabel2.Location = new System.Drawing.Point(317, 204);
            this.TitleLabel2.Name = "TitleLabel2";
            this.TitleLabel2.Size = new System.Drawing.Size(229, 88);
            this.TitleLabel2.TabIndex = 3;
            this.TitleLabel2.Text = "Jotter";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RoyalBlue;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(298, 186);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(266, 120);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // OpenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = global::JotterFinal.Properties.Resources.v602_nunoon_22_rippednotes;
            this.ClientSize = new System.Drawing.Size(871, 504);
            this.Controls.Add(this.TitleLabel2);
            this.Controls.Add(this.ContinueButton);
            this.Controls.Add(this.TitleLabel1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OpenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jotter";
            this.Load += new System.EventHandler(this.OpenForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleLabel1;
        private System.Windows.Forms.Button ContinueButton;
        private System.Windows.Forms.Label TitleLabel2;
        private System.Windows.Forms.Button button1;
    }
}


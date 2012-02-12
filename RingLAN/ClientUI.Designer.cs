namespace RingLAN {
    partial class ClientUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.RecievedMessagesBox = new System.Windows.Forms.TextBox();
            this.InputBox = new System.Windows.Forms.TextBox();
            this.SendButton = new System.Windows.Forms.Button();
            this.RecipientSelectBox = new System.Windows.Forms.ComboBox();
            this.LogOutButton = new System.Windows.Forms.Button();
            this.DebugModeCheck = new System.Windows.Forms.CheckBox();
            this.AttemptKickButton = new System.Windows.Forms.Button();
            this.BringDownTheSkyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RecievedMessagesBox
            // 
            this.RecievedMessagesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.RecievedMessagesBox.Enabled = false;
            this.RecievedMessagesBox.Location = new System.Drawing.Point(13, 38);
            this.RecievedMessagesBox.Multiline = true;
            this.RecievedMessagesBox.Name = "RecievedMessagesBox";
            this.RecievedMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RecievedMessagesBox.Size = new System.Drawing.Size(437, 355);
            this.RecievedMessagesBox.TabIndex = 0;
            this.RecievedMessagesBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RecievedMessagesBox_KeyDown);
            // 
            // InputBox
            // 
            this.InputBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.InputBox.Location = new System.Drawing.Point(96, 401);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(301, 20);
            this.InputBox.TabIndex = 1;
            this.InputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputBox_KeyDown);
            // 
            // SendButton
            // 
            this.SendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SendButton.Location = new System.Drawing.Point(403, 399);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(47, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Login";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // RecipientSelectBox
            // 
            this.RecipientSelectBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RecipientSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RecipientSelectBox.FormattingEnabled = true;
            this.RecipientSelectBox.Items.AddRange(new object[] {
            "All"});
            this.RecipientSelectBox.Location = new System.Drawing.Point(13, 400);
            this.RecipientSelectBox.Name = "RecipientSelectBox";
            this.RecipientSelectBox.Size = new System.Drawing.Size(77, 21);
            this.RecipientSelectBox.TabIndex = 3;
            // 
            // LogOutButton
            // 
            this.LogOutButton.Location = new System.Drawing.Point(12, 9);
            this.LogOutButton.Name = "LogOutButton";
            this.LogOutButton.Size = new System.Drawing.Size(53, 23);
            this.LogOutButton.TabIndex = 4;
            this.LogOutButton.Text = "Log Out";
            this.LogOutButton.UseVisualStyleBackColor = true;
            this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
            // 
            // DebugModeCheck
            // 
            this.DebugModeCheck.AutoSize = true;
            this.DebugModeCheck.Location = new System.Drawing.Point(334, 13);
            this.DebugModeCheck.Name = "DebugModeCheck";
            this.DebugModeCheck.Size = new System.Drawing.Size(116, 17);
            this.DebugModeCheck.TabIndex = 5;
            this.DebugModeCheck.Text = "Promiscuous Mode";
            this.DebugModeCheck.UseVisualStyleBackColor = true;
            this.DebugModeCheck.CheckedChanged += new System.EventHandler(this.DebugModeCheck_CheckedChanged);
            // 
            // AttemptKickButton
            // 
            this.AttemptKickButton.Location = new System.Drawing.Point(71, 9);
            this.AttemptKickButton.Name = "AttemptKickButton";
            this.AttemptKickButton.Size = new System.Drawing.Size(75, 23);
            this.AttemptKickButton.TabIndex = 6;
            this.AttemptKickButton.Text = "Kick Attempt";
            this.AttemptKickButton.UseVisualStyleBackColor = true;
            this.AttemptKickButton.Click += new System.EventHandler(this.AttemptKickButton_Click);
            // 
            // BringDownTheSkyButton
            // 
            this.BringDownTheSkyButton.Location = new System.Drawing.Point(152, 9);
            this.BringDownTheSkyButton.Name = "BringDownTheSkyButton";
            this.BringDownTheSkyButton.Size = new System.Drawing.Size(85, 23);
            this.BringDownTheSkyButton.TabIndex = 7;
            this.BringDownTheSkyButton.Text = "Stress Testing";
            this.BringDownTheSkyButton.UseVisualStyleBackColor = true;
            this.BringDownTheSkyButton.Click += new System.EventHandler(this.BringDownTheSkyButton_Click);
            // 
            // ClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 436);
            this.Controls.Add(this.BringDownTheSkyButton);
            this.Controls.Add(this.AttemptKickButton);
            this.Controls.Add(this.DebugModeCheck);
            this.Controls.Add(this.LogOutButton);
            this.Controls.Add(this.RecipientSelectBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.RecievedMessagesBox);
            this.Name = "ClientUI";
            this.Text = "LAN Ring UI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientUI_FormClosed);
            this.Load += new System.EventHandler(this.ClientUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RecievedMessagesBox;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ComboBox RecipientSelectBox;
        private System.Windows.Forms.Button LogOutButton;
        private System.Windows.Forms.CheckBox DebugModeCheck;
        private System.Windows.Forms.Button AttemptKickButton;
        private System.Windows.Forms.Button BringDownTheSkyButton;
    }
}
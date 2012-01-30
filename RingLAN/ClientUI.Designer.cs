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
            this.SuspendLayout();
            // 
            // RecievedMessagesBox
            // 
            this.RecievedMessagesBox.Location = new System.Drawing.Point(13, 13);
            this.RecievedMessagesBox.Multiline = true;
            this.RecievedMessagesBox.Name = "RecievedMessagesBox";
            this.RecievedMessagesBox.Size = new System.Drawing.Size(437, 383);
            this.RecievedMessagesBox.TabIndex = 0;
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(96, 401);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(301, 20);
            this.InputBox.TabIndex = 1;
            // 
            // SendButton
            // 
            this.SendButton.Location = new System.Drawing.Point(403, 399);
            this.SendButton.Name = "SendButton";
            this.SendButton.Size = new System.Drawing.Size(47, 23);
            this.SendButton.TabIndex = 2;
            this.SendButton.Text = "Send";
            this.SendButton.UseVisualStyleBackColor = true;
            this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
            // 
            // RecipientSelectBox
            // 
            this.RecipientSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RecipientSelectBox.FormattingEnabled = true;
            this.RecipientSelectBox.Location = new System.Drawing.Point(13, 400);
            this.RecipientSelectBox.Name = "RecipientSelectBox";
            this.RecipientSelectBox.Size = new System.Drawing.Size(77, 21);
            this.RecipientSelectBox.TabIndex = 3;
            // 
            // ClientUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 436);
            this.Controls.Add(this.RecipientSelectBox);
            this.Controls.Add(this.SendButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.RecievedMessagesBox);
            this.Name = "ClientUI";
            this.Text = "LAN Ring UI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox RecievedMessagesBox;
        private System.Windows.Forms.TextBox InputBox;
        private System.Windows.Forms.Button SendButton;
        private System.Windows.Forms.ComboBox RecipientSelectBox;
    }
}
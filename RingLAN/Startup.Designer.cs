namespace RingLAN {
    partial class Startup {
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
            this.VirtualLaunchButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.VirtualItemsSelect = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.PhysicalLaunchButton = new System.Windows.Forms.Button();
            this.PhysicalPortSelect = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.NoisePotential = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoisePotential)).BeginInit();
            this.SuspendLayout();
            // 
            // VirtualLaunchButton
            // 
            this.VirtualLaunchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VirtualLaunchButton.Location = new System.Drawing.Point(187, 76);
            this.VirtualLaunchButton.Name = "VirtualLaunchButton";
            this.VirtualLaunchButton.Size = new System.Drawing.Size(75, 23);
            this.VirtualLaunchButton.TabIndex = 0;
            this.VirtualLaunchButton.Text = "Launch";
            this.VirtualLaunchButton.UseVisualStyleBackColor = true;
            this.VirtualLaunchButton.Click += new System.EventHandler(this.VirtualLaunchButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.NoisePotential);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.VirtualItemsSelect);
            this.groupBox1.Controls.Add(this.VirtualLaunchButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(268, 105);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Virtual";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "A virtual communications ring.";
            // 
            // VirtualItemsSelect
            // 
            this.VirtualItemsSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VirtualItemsSelect.FormattingEnabled = true;
            this.VirtualItemsSelect.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.VirtualItemsSelect.Location = new System.Drawing.Point(6, 76);
            this.VirtualItemsSelect.Name = "VirtualItemsSelect";
            this.VirtualItemsSelect.Size = new System.Drawing.Size(102, 21);
            this.VirtualItemsSelect.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.PhysicalLaunchButton);
            this.groupBox2.Controls.Add(this.PhysicalPortSelect);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 123);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 115);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Physical";
            // 
            // PhysicalLaunchButton
            // 
            this.PhysicalLaunchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PhysicalLaunchButton.Location = new System.Drawing.Point(187, 88);
            this.PhysicalLaunchButton.Name = "PhysicalLaunchButton";
            this.PhysicalLaunchButton.Size = new System.Drawing.Size(75, 23);
            this.PhysicalLaunchButton.TabIndex = 2;
            this.PhysicalLaunchButton.Text = "Launch";
            this.PhysicalLaunchButton.UseVisualStyleBackColor = true;
            this.PhysicalLaunchButton.Click += new System.EventHandler(this.PhysicalLaunchButton_Click);
            // 
            // PhysicalPortSelect
            // 
            this.PhysicalPortSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PhysicalPortSelect.FormattingEnabled = true;
            this.PhysicalPortSelect.Location = new System.Drawing.Point(6, 72);
            this.PhysicalPortSelect.Name = "PhysicalPortSelect";
            this.PhysicalPortSelect.Size = new System.Drawing.Size(102, 21);
            this.PhysicalPortSelect.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "A physical ring using RS-232";
            // 
            // NoisePotential
            // 
            this.NoisePotential.Location = new System.Drawing.Point(6, 50);
            this.NoisePotential.Name = "NoisePotential";
            this.NoisePotential.Size = new System.Drawing.Size(102, 20);
            this.NoisePotential.TabIndex = 3;
            this.NoisePotential.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 250);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Startup";
            this.Text = "LAN Ring Messenger";
            this.Load += new System.EventHandler(this.Startup_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NoisePotential)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button VirtualLaunchButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox VirtualItemsSelect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button PhysicalLaunchButton;
        private System.Windows.Forms.ComboBox PhysicalPortSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NoisePotential;
    }
}
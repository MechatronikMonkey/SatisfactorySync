namespace V1
{
    partial class SatisfactorySync
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSync = new System.Windows.Forms.TabPage();
            this.btnDOWNLOAD = new System.Windows.Forms.Button();
            this.btnUPLOAD = new System.Windows.Forms.Button();
            this.txtLastDOWNdate = new System.Windows.Forms.TextBox();
            this.txtLastDOWNname = new System.Windows.Forms.TextBox();
            this.txtLastUPdate = new System.Windows.Forms.TextBox();
            this.txtLastUPname = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.button4 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPathToSave = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFTP = new System.Windows.Forms.TextBox();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.checkFile = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabSync.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSync);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 306);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSync
            // 
            this.tabSync.Controls.Add(this.btnDOWNLOAD);
            this.tabSync.Controls.Add(this.btnUPLOAD);
            this.tabSync.Controls.Add(this.txtLastDOWNdate);
            this.tabSync.Controls.Add(this.txtLastDOWNname);
            this.tabSync.Controls.Add(this.txtLastUPdate);
            this.tabSync.Controls.Add(this.txtLastUPname);
            this.tabSync.Controls.Add(this.pictureBox2);
            this.tabSync.Controls.Add(this.pictureBox1);
            this.tabSync.Controls.Add(this.label1);
            this.tabSync.Controls.Add(this.label2);
            this.tabSync.Controls.Add(this.label3);
            this.tabSync.Controls.Add(this.label4);
            this.tabSync.Location = new System.Drawing.Point(4, 22);
            this.tabSync.Name = "tabSync";
            this.tabSync.Padding = new System.Windows.Forms.Padding(3);
            this.tabSync.Size = new System.Drawing.Size(416, 280);
            this.tabSync.TabIndex = 0;
            this.tabSync.Text = "Sync";
            this.tabSync.UseVisualStyleBackColor = true;
            // 
            // btnDOWNLOAD
            // 
            this.btnDOWNLOAD.BackgroundImage = global::V1.Properties.Resources.arrowDOWN;
            this.btnDOWNLOAD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDOWNLOAD.Location = new System.Drawing.Point(232, 150);
            this.btnDOWNLOAD.Name = "btnDOWNLOAD";
            this.btnDOWNLOAD.Size = new System.Drawing.Size(120, 120);
            this.btnDOWNLOAD.TabIndex = 14;
            this.btnDOWNLOAD.UseVisualStyleBackColor = true;
            // 
            // btnUPLOAD
            // 
            this.btnUPLOAD.BackgroundImage = global::V1.Properties.Resources.arrowUP;
            this.btnUPLOAD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUPLOAD.Location = new System.Drawing.Point(58, 150);
            this.btnUPLOAD.Name = "btnUPLOAD";
            this.btnUPLOAD.Size = new System.Drawing.Size(120, 120);
            this.btnUPLOAD.TabIndex = 13;
            this.btnUPLOAD.UseVisualStyleBackColor = true;
            this.btnUPLOAD.Click += new System.EventHandler(this.btnUPLOAD_Click);
            // 
            // txtLastDOWNdate
            // 
            this.txtLastDOWNdate.Enabled = false;
            this.txtLastDOWNdate.Location = new System.Drawing.Point(205, 112);
            this.txtLastDOWNdate.Name = "txtLastDOWNdate";
            this.txtLastDOWNdate.Size = new System.Drawing.Size(172, 20);
            this.txtLastDOWNdate.TabIndex = 12;
            // 
            // txtLastDOWNname
            // 
            this.txtLastDOWNname.Enabled = false;
            this.txtLastDOWNname.Location = new System.Drawing.Point(204, 87);
            this.txtLastDOWNname.Name = "txtLastDOWNname";
            this.txtLastDOWNname.Size = new System.Drawing.Size(173, 20);
            this.txtLastDOWNname.TabIndex = 11;
            // 
            // txtLastUPdate
            // 
            this.txtLastUPdate.Enabled = false;
            this.txtLastUPdate.Location = new System.Drawing.Point(204, 50);
            this.txtLastUPdate.Name = "txtLastUPdate";
            this.txtLastUPdate.Size = new System.Drawing.Size(173, 20);
            this.txtLastUPdate.TabIndex = 10;
            // 
            // txtLastUPname
            // 
            this.txtLastUPname.Enabled = false;
            this.txtLastUPname.Location = new System.Drawing.Point(204, 25);
            this.txtLastUPname.Name = "txtLastUPname";
            this.txtLastUPname.Size = new System.Drawing.Size(173, 20);
            this.txtLastUPname.TabIndex = 9;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::V1.Properties.Resources.arrowDOWN;
            this.pictureBox2.Location = new System.Drawing.Point(38, 91);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(30, 30);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::V1.Properties.Resources.arrowUP;
            this.pictureBox1.Location = new System.Drawing.Point(38, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(30, 30);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Last DOWNloaded date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Last DOWNloaded from:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Last UPloaded date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Last UPloaded from:";
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.button4);
            this.tabSettings.Controls.Add(this.label10);
            this.tabSettings.Controls.Add(this.txtName);
            this.tabSettings.Controls.Add(this.label9);
            this.tabSettings.Controls.Add(this.txtFilename);
            this.tabSettings.Controls.Add(this.btnSave);
            this.tabSettings.Controls.Add(this.label8);
            this.tabSettings.Controls.Add(this.txtPathToSave);
            this.tabSettings.Controls.Add(this.label7);
            this.tabSettings.Controls.Add(this.txtPass);
            this.tabSettings.Controls.Add(this.label6);
            this.tabSettings.Controls.Add(this.txtUser);
            this.tabSettings.Controls.Add(this.label5);
            this.tabSettings.Controls.Add(this.txtFTP);
            this.tabSettings.Location = new System.Drawing.Point(4, 22);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(416, 280);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(361, 205);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(36, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "...";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Your name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(89, 24);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(162, 20);
            this.txtName.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "File";
            // 
            // txtFilename
            // 
            this.txtFilename.Location = new System.Drawing.Point(89, 146);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.Size = new System.Drawing.Size(162, 20);
            this.txtFilename.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(40, 246);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(331, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save and test connection";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 209);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "path to save game";
            // 
            // txtPathToSave
            // 
            this.txtPathToSave.Location = new System.Drawing.Point(122, 206);
            this.txtPathToSave.Name = "txtPathToSave";
            this.txtPathToSave.Size = new System.Drawing.Size(233, 20);
            this.txtPathToSave.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Pass";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(89, 120);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(162, 20);
            this.txtPass.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "User";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(89, 94);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(162, 20);
            this.txtUser.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "ftp address";
            // 
            // txtFTP
            // 
            this.txtFTP.Location = new System.Drawing.Point(89, 68);
            this.txtFTP.Name = "txtFTP";
            this.txtFTP.Size = new System.Drawing.Size(162, 20);
            this.txtFTP.TabIndex = 0;
            // 
            // txtStatus
            // 
            this.txtStatus.Enabled = false;
            this.txtStatus.Location = new System.Drawing.Point(12, 325);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(420, 20);
            this.txtStatus.TabIndex = 1;
            // 
            // checkFile
            // 
            this.checkFile.Enabled = true;
            this.checkFile.Interval = 2000;
            this.checkFile.Tick += new System.EventHandler(this.checkFile_Tick);
            // 
            // SatisfactorySync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 359);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.tabControl1);
            this.Name = "SatisfactorySync";
            this.Text = "Satisfactory Sync V1";
            this.Load += new System.EventHandler(this.SatisfactorySync_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSync.ResumeLayout(false);
            this.tabSync.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSync;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnUPLOAD;
        private System.Windows.Forms.TextBox txtLastDOWNdate;
        private System.Windows.Forms.TextBox txtLastDOWNname;
        private System.Windows.Forms.TextBox txtLastUPdate;
        private System.Windows.Forms.TextBox txtLastUPname;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnDOWNLOAD;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtFTP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPathToSave;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Timer checkFile;
    }
}


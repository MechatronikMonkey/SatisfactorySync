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
            this.btnImportSettings = new System.Windows.Forms.Button();
            this.btnExportSettings = new System.Windows.Forms.Button();
            this.btnListXMLs = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSelectSave = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSettingsFile = new System.Windows.Forms.TextBox();
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
            this.txtLocalPlaytime = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtLocalGameName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtServerPlaytime = new System.Windows.Forms.TextBox();
            this.txtServerGameName = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
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
            this.tabControl1.Size = new System.Drawing.Size(424, 490);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSync
            // 
            this.tabSync.Controls.Add(this.label19);
            this.tabSync.Controls.Add(this.txtServerPlaytime);
            this.tabSync.Controls.Add(this.txtServerGameName);
            this.tabSync.Controls.Add(this.label17);
            this.tabSync.Controls.Add(this.label18);
            this.tabSync.Controls.Add(this.txtLocalGameName);
            this.tabSync.Controls.Add(this.label16);
            this.tabSync.Controls.Add(this.txtLocalPlaytime);
            this.tabSync.Controls.Add(this.label15);
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
            this.tabSync.Size = new System.Drawing.Size(416, 464);
            this.tabSync.TabIndex = 0;
            this.tabSync.Text = "Sync";
            this.tabSync.UseVisualStyleBackColor = true;
            this.tabSync.Click += new System.EventHandler(this.tabSync_Click);
            // 
            // btnDOWNLOAD
            // 
            this.btnDOWNLOAD.BackgroundImage = global::SatisfactorySyncV0_2.Properties.Resources.arrowDOWN;
            this.btnDOWNLOAD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDOWNLOAD.Location = new System.Drawing.Point(234, 312);
            this.btnDOWNLOAD.Name = "btnDOWNLOAD";
            this.btnDOWNLOAD.Size = new System.Drawing.Size(120, 120);
            this.btnDOWNLOAD.TabIndex = 14;
            this.btnDOWNLOAD.UseVisualStyleBackColor = true;
            this.btnDOWNLOAD.Click += new System.EventHandler(this.btnDOWNLOAD_Click);
            // 
            // btnUPLOAD
            // 
            this.btnUPLOAD.BackgroundImage = global::SatisfactorySyncV0_2.Properties.Resources.arrowUP;
            this.btnUPLOAD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnUPLOAD.Location = new System.Drawing.Point(52, 312);
            this.btnUPLOAD.Name = "btnUPLOAD";
            this.btnUPLOAD.Size = new System.Drawing.Size(120, 120);
            this.btnUPLOAD.TabIndex = 13;
            this.btnUPLOAD.UseVisualStyleBackColor = true;
            this.btnUPLOAD.Click += new System.EventHandler(this.btnUPLOAD_Click);
            // 
            // txtLastDOWNdate
            // 
            this.txtLastDOWNdate.Enabled = false;
            this.txtLastDOWNdate.Location = new System.Drawing.Point(205, 240);
            this.txtLastDOWNdate.Name = "txtLastDOWNdate";
            this.txtLastDOWNdate.Size = new System.Drawing.Size(172, 20);
            this.txtLastDOWNdate.TabIndex = 12;
            // 
            // txtLastDOWNname
            // 
            this.txtLastDOWNname.Enabled = false;
            this.txtLastDOWNname.Location = new System.Drawing.Point(204, 215);
            this.txtLastDOWNname.Name = "txtLastDOWNname";
            this.txtLastDOWNname.Size = new System.Drawing.Size(173, 20);
            this.txtLastDOWNname.TabIndex = 11;
            // 
            // txtLastUPdate
            // 
            this.txtLastUPdate.Enabled = false;
            this.txtLastUPdate.Location = new System.Drawing.Point(204, 93);
            this.txtLastUPdate.Name = "txtLastUPdate";
            this.txtLastUPdate.Size = new System.Drawing.Size(173, 20);
            this.txtLastUPdate.TabIndex = 10;
            this.txtLastUPdate.TextChanged += new System.EventHandler(this.txtLastUPdate_TextChanged);
            // 
            // txtLastUPname
            // 
            this.txtLastUPname.Enabled = false;
            this.txtLastUPname.Location = new System.Drawing.Point(204, 68);
            this.txtLastUPname.Name = "txtLastUPname";
            this.txtLastUPname.Size = new System.Drawing.Size(173, 20);
            this.txtLastUPname.TabIndex = 9;
            this.txtLastUPname.TextChanged += new System.EventHandler(this.txtLastUPname_TextChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::SatisfactorySyncV0_2.Properties.Resources.arrowDOWN;
            this.pictureBox2.Location = new System.Drawing.Point(16, 185);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(49, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SatisfactorySyncV0_2.Properties.Resources.arrowUP;
            this.pictureBox1.Location = new System.Drawing.Point(16, 43);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(49, 46);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 240);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Last DOWNloaded date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 215);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Last DOWNloaded from:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(74, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Last UPloaded date:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Last UPloaded from:";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.btnImportSettings);
            this.tabSettings.Controls.Add(this.btnExportSettings);
            this.tabSettings.Controls.Add(this.btnListXMLs);
            this.tabSettings.Controls.Add(this.label14);
            this.tabSettings.Controls.Add(this.label13);
            this.tabSettings.Controls.Add(this.label12);
            this.tabSettings.Controls.Add(this.label11);
            this.tabSettings.Controls.Add(this.btnSelectSave);
            this.tabSettings.Controls.Add(this.label10);
            this.tabSettings.Controls.Add(this.txtName);
            this.tabSettings.Controls.Add(this.label9);
            this.tabSettings.Controls.Add(this.txtSettingsFile);
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
            this.tabSettings.Size = new System.Drawing.Size(416, 349);
            this.tabSettings.TabIndex = 1;
            this.tabSettings.Text = "Settings";
            this.tabSettings.UseVisualStyleBackColor = true;
            // 
            // btnImportSettings
            // 
            this.btnImportSettings.Location = new System.Drawing.Point(342, 249);
            this.btnImportSettings.Name = "btnImportSettings";
            this.btnImportSettings.Size = new System.Drawing.Size(54, 23);
            this.btnImportSettings.TabIndex = 20;
            this.btnImportSettings.Text = "import...";
            this.btnImportSettings.UseVisualStyleBackColor = true;
            this.btnImportSettings.Click += new System.EventHandler(this.btnImportSettings_Click);
            // 
            // btnExportSettings
            // 
            this.btnExportSettings.Location = new System.Drawing.Point(282, 249);
            this.btnExportSettings.Name = "btnExportSettings";
            this.btnExportSettings.Size = new System.Drawing.Size(54, 23);
            this.btnExportSettings.TabIndex = 19;
            this.btnExportSettings.Text = "export...";
            this.btnExportSettings.UseVisualStyleBackColor = true;
            this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
            // 
            // btnListXMLs
            // 
            this.btnListXMLs.Location = new System.Drawing.Point(256, 145);
            this.btnListXMLs.Name = "btnListXMLs";
            this.btnListXMLs.Size = new System.Drawing.Size(24, 23);
            this.btnListXMLs.TabIndex = 18;
            this.btnListXMLs.Text = "...";
            this.btnListXMLs.UseVisualStyleBackColor = true;
            this.btnListXMLs.Click += new System.EventHandler(this.btnListXMLs_Click);
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(97, 212);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(288, 23);
            this.label14.TabIndex = 17;
            this.label14.Text = "Leave empty if joining a game, pick file if starting an upload.\r\nWill be filled a" +
    "utomatically by downloading a file.";
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(287, 146);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(126, 34);
            this.label13.TabIndex = 16;
            this.label13.Text = "Your private settings file.  \r\nE.g. \"NameA_NameB.xml\"";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(287, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 12);
            this.label12.TabIndex = 15;
            this.label12.Text = "ftp://yourftpadress.de";
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(287, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(114, 31);
            this.label11.TabIndex = 14;
            this.label11.Text = "DONT change after uploading under that name!";
            // 
            // btnSelectSave
            // 
            this.btnSelectSave.Location = new System.Drawing.Point(360, 188);
            this.btnSelectSave.Name = "btnSelectSave";
            this.btnSelectSave.Size = new System.Drawing.Size(36, 23);
            this.btnSelectSave.TabIndex = 13;
            this.btnSelectSave.Text = "...";
            this.btnSelectSave.UseVisualStyleBackColor = true;
            this.btnSelectSave.Click += new System.EventHandler(this.btnSelectSave_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 27);
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
            this.label9.Location = new System.Drawing.Point(20, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Settings file";
            // 
            // txtSettingsFile
            // 
            this.txtSettingsFile.Location = new System.Drawing.Point(89, 146);
            this.txtSettingsFile.Name = "txtSettingsFile";
            this.txtSettingsFile.Size = new System.Drawing.Size(162, 20);
            this.txtSettingsFile.TabIndex = 9;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(23, 249);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(228, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Savegame";
            // 
            // txtPathToSave
            // 
            this.txtPathToSave.Location = new System.Drawing.Point(89, 189);
            this.txtPathToSave.Name = "txtPathToSave";
            this.txtPathToSave.Size = new System.Drawing.Size(265, 20);
            this.txtPathToSave.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 123);
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
            this.label6.Location = new System.Drawing.Point(20, 97);
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
            this.label5.Location = new System.Drawing.Point(20, 71);
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
            this.txtStatus.Location = new System.Drawing.Point(12, 508);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(420, 20);
            this.txtStatus.TabIndex = 1;
            // 
            // checkFile
            // 
            this.checkFile.Enabled = true;
            this.checkFile.Interval = 3000;
            this.checkFile.Tick += new System.EventHandler(this.checkFile_Tick);
            // 
            // txtLocalPlaytime
            // 
            this.txtLocalPlaytime.Enabled = false;
            this.txtLocalPlaytime.Location = new System.Drawing.Point(204, 42);
            this.txtLocalPlaytime.Name = "txtLocalPlaytime";
            this.txtLocalPlaytime.Size = new System.Drawing.Size(173, 20);
            this.txtLocalPlaytime.TabIndex = 16;
            this.txtLocalPlaytime.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(74, 45);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(78, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Local Playtime:";
            this.label15.Click += new System.EventHandler(this.label15_Click);
            // 
            // txtLocalGameName
            // 
            this.txtLocalGameName.Enabled = false;
            this.txtLocalGameName.Location = new System.Drawing.Point(204, 16);
            this.txtLocalGameName.Name = "txtLocalGameName";
            this.txtLocalGameName.Size = new System.Drawing.Size(173, 20);
            this.txtLocalGameName.TabIndex = 18;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(74, 19);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 13);
            this.label16.TabIndex = 17;
            this.label16.Text = "Local Game Name:";
            // 
            // txtServerPlaytime
            // 
            this.txtServerPlaytime.Enabled = false;
            this.txtServerPlaytime.Location = new System.Drawing.Point(205, 189);
            this.txtServerPlaytime.Name = "txtServerPlaytime";
            this.txtServerPlaytime.Size = new System.Drawing.Size(172, 20);
            this.txtServerPlaytime.TabIndex = 22;
            // 
            // txtServerGameName
            // 
            this.txtServerGameName.Enabled = false;
            this.txtServerGameName.Location = new System.Drawing.Point(204, 164);
            this.txtServerGameName.Name = "txtServerGameName";
            this.txtServerGameName.Size = new System.Drawing.Size(173, 20);
            this.txtServerGameName.TabIndex = 21;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(74, 189);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(83, 13);
            this.label17.TabIndex = 20;
            this.label17.Text = "Server Playtime:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(74, 164);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(103, 13);
            this.label18.TabIndex = 19;
            this.label18.Text = "Server Game Name:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(10, 129);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(397, 13);
            this.label19.TabIndex = 23;
            this.label19.Text = "_________________________________________________________________";
            this.label19.Click += new System.EventHandler(this.label19_Click);
            // 
            // SatisfactorySync
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 538);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.tabControl1);
            this.Name = "SatisfactorySync";
            this.Text = "Satisfactory Sync V0.2";
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
        private System.Windows.Forms.TextBox txtSettingsFile;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSelectSave;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Timer checkFile;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnListXMLs;
        private System.Windows.Forms.Button btnImportSettings;
        private System.Windows.Forms.Button btnExportSettings;
        private System.Windows.Forms.TextBox txtLocalPlaytime;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtLocalGameName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtServerPlaytime;
        private System.Windows.Forms.TextBox txtServerGameName;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
    }
}


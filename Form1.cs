using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using V1.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace V1
{
    public partial class SatisfactorySync : Form
    {
        public SatisfactorySync()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Erstelle ein OpenFileDialog-Objekt
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Verwende eine Umgebungsvariable wie %LOCALAPPDATA%
            string localAppDataPath = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%");

            // Setze den aufgelösten Pfad als InitialDirectory
            openFileDialog.InitialDirectory = localAppDataPath + "\\FactoryGame\\Saved\\SaveGames";

            // Optional: Filter setzen, um nur bestimmte Dateitypen anzuzeigen
            openFileDialog.Filter = "SAV-Dateien (*.sav)|*.sav|Alle Dateien (*.*)|*.*";

            // Überprüfe, ob der Benutzer eine Datei ausgewählt hat
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Hier kannst du den Pfad der ausgewählten Datei abrufen
                string filePath = openFileDialog.FileName;

                // Beispielsweise: Dateipfad in einem Textfeld anzeigen
                txtPathToSave.Text = filePath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SSSettings sSSettings = new SSSettings();

            
            sSSettings.name = txtName.Text;
            sSSettings.ftpAddress = txtFTP.Text;
            sSSettings.user = txtUser.Text;
            sSSettings.pass = txtPass.Text;
            sSSettings.file = txtSettingsFile.Text;
            sSSettings.pathToSave = txtPathToSave.Text;

            // Dateipfad zum Speichern der Settings festlegen
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SatisfactorySyncSettings.json");

            // Einstellungen in JSON serialisieren und in eine Datei schreiben
            string json = System.Text.Json.JsonSerializer.Serialize(sSSettings);
            File.WriteAllText(filePath, json);

            MessageBox.Show("Einstellungen wurden erfolgreich gespeichert.");
        }

        private void SatisfactorySync_Load(object sender, EventArgs e)
        {
            // Dateipfad der gespeicherten Settings festlegen
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SatisfactorySyncSettings.json");

            // JSON-Inhalt lesen und in ein AppSettings-Objekt deserialisieren
            string json = File.ReadAllText(filePath);
            SSSettings settings = System.Text.Json.JsonSerializer.Deserialize<SSSettings>(json);

            // TextBox-Werte mit den geladenen Properties füllen
            txtName.Text = settings.name;
            txtFTP.Text = settings.ftpAddress;
            txtUser.Text = settings.user;
            txtPass.Text = settings.pass;
            txtSettingsFile.Text = settings.file;
            txtPathToSave.Text = settings.pathToSave;

        }

        private void btnUPLOAD_Click(object sender, EventArgs e)
        {

            // 0. Vergleich der Datumswerte
            DateTime lastUpdateDate = DateTime.Parse(txtLastUPdate.Text);
            DateTime lastDownloadDate = DateTime.Parse(txtLastDOWNdate.Text);

            if (txtName.Text != txtLastDOWNname.Text)
            {
                // Warnung, anderer User im Spiel
                DialogResult result = MessageBox.Show(
                    "Jemand anderes hat das Spiel nach dir heruntergeladen! Der User " + txtLastDOWNname.Text + " hat das spiel parallel zu dir heruntergeladen. Bist du sicher, dass du das Spiel trotzdem hochladen möchtest?",
                    "Warnung",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    txtStatus.Text = "Uplad abgebrochen.";
                    return;
                }
            }

            string xmlFilePath = Path.Combine(Path.GetTempPath(), "updownstate.xml");
            string binaryFilePath = txtPathToSave.Text; // Absoluter Pfad zur bestehenden Binärdatei
            UP_DOWN_State settings;

            // Prüfen, ob die XML-Datei existiert
            if (File.Exists(xmlFilePath))
            {
                // XML-Datei lesen und bestehende Einstellungen laden
                XmlSerializer serializer = new XmlSerializer(typeof(UP_DOWN_State));
                using (FileStream fs = new FileStream(xmlFilePath, FileMode.Open))
                {
                    settings = (UP_DOWN_State)serializer.Deserialize(fs);
                }
            }
            else
            {
                // Neue Instanz erstellen, wenn die XML-Datei nicht existiert
                settings = new UP_DOWN_State();
            }

            // Update der gewünschten Properties
            settings.lastUPName = txtName.Text;
            settings.lastUPDate = DateTime.Now.ToString();

            // XML-Datei speichern
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UP_DOWN_State));
            using (FileStream fs = new FileStream(xmlFilePath, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, settings);
            }

            // FTP-Verbindungsinformationen
            string ftpServer = txtFTP.Text;
            string ftpUser = txtUser.Text;
            string ftpPass = txtPass.Text;

            // 1. Binärdatei vom Dateisystem auf FTP hochladen (ohne sie zu verändern)
            try
            {
                if (!File.Exists(binaryFilePath))
                {
                    MessageBox.Show("Das Savegame existiert nicht!");
                    return;
                }

                // Extrahiere den Dateinamen (mit Erweiterung) aus dem lokalen Pfad
                string binaryFileName = Path.GetFileName(binaryFilePath);

                // Binärdatei auf FTP hochladen
                string ftpBinaryPath = $"{ftpServer}/{binaryFileName}"; // Nutze den Dateinamen vom lokalen System
                FtpWebRequest binaryRequest = (FtpWebRequest)WebRequest.Create(ftpBinaryPath);
                binaryRequest.Method = WebRequestMethods.Ftp.UploadFile;
                binaryRequest.Credentials = new NetworkCredential(ftpUser, ftpPass);
                binaryRequest.UsePassive = true;

                byte[] binaryFileContents = File.ReadAllBytes(binaryFilePath); // Originale Binärdatei lesen
                binaryRequest.ContentLength = binaryFileContents.Length;

                using (Stream binaryRequestStream = binaryRequest.GetRequestStream())
                {
                    binaryRequestStream.Write(binaryFileContents, 0, binaryFileContents.Length);
                }

                using (FtpWebResponse binaryResponse = (FtpWebResponse)binaryRequest.GetResponse())
                {
                    MessageBox.Show($"Savegame erfolgreich hochgeladen: {binaryResponse.StatusDescription}");
                }

                txtStatus.Text = "Savegame erfolgreich hochgeladen.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fehler beim Hochladen des Savegames: " + ex.Message;
                return; // Abbrechen, falls der Upload fehlschlägt
            }

            // 2. XML-Datei hochladen
            try
            {
                string ftpFullPath = $"{ftpServer}/updownstate.xml"; // Pfad auf dem FTP-Server
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                request.UsePassive = true;

                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(xmlFilePath))
                {
                    fileContents = System.Text.Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    MessageBox.Show($"XML-Datei erfolgreich hochgeladen: {response.StatusDescription}");
                }

                txtStatus.Text = "state update pushed.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fehler beim Hochladen der XML-Datei: " + ex.Message;
            }
        }


        private void checkFile_Tick(object sender, EventArgs e)
        {
            // FTP-Verbindungsinformationen
            string ftpServer = txtFTP.Text; // Ersetze mit deinem FTP-Server
            string ftpUser = txtUser.Text; // Ersetze mit deinem Benutzernamen
            string ftpPass = txtPass.Text; // Ersetze mit deinem Passwort

            string ftpFullPath = $"{ftpServer}/" + txtSettingsFile.Text; // Pfad auf dem FTP-Server

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPass);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string xmlContent = reader.ReadToEnd();

                    // XML deserialisieren und Properties laden
                    XmlSerializer serializer = new XmlSerializer(typeof(UP_DOWN_State));
                    using (StringReader stringReader = new StringReader(xmlContent))
                    {
                        UP_DOWN_State settings = (UP_DOWN_State)serializer.Deserialize(stringReader);

                        // TextBoxen mit den geladenen Werten füllen
                        txtLastUPname.Text = settings.lastUPName;
                        txtLastUPdate.Text = settings.lastUPDate;
                        txtLastDOWNname.Text = settings.lastDOWNName;
                        txtLastDOWNdate.Text = settings.lastDOWNDate;

                    }
                }

                txtStatus.Text = "Status update erfolgreich...";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fehler FTP!" + ex.Message;
            }
        }

        // Methode zur Dateiauswahl (z.B. durch eine ComboBox oder ListBox)
        private string PromptForFileSelection(List<string> files)
        {
            using (Form form = new Form())
            {
                ListBox listBox = new ListBox { Dock = DockStyle.Fill };
                foreach (var file in files)
                {
                    listBox.Items.Add(file);
                }

                form.Controls.Add(listBox);
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(300, 200);

                System.Windows.Forms.Button selectButton = new System.Windows.Forms.Button { Text = "Auswählen", Dock = DockStyle.Bottom };
                form.Controls.Add(selectButton);

                string selectedFile = null;
                selectButton.Click += (sender, e) =>
                {
                    selectedFile = listBox.SelectedItem?.ToString();
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                form.ShowDialog();
                return selectedFile;
            }
        }

        private void btnDOWNLOAD_Click(object sender, EventArgs e)
        {

            // 0. Vergleich der Datumswerte
            DateTime lastUpdateDate = DateTime.Parse(txtLastUPdate.Text);
            DateTime lastDownloadDate = DateTime.Parse(txtLastDOWNdate.Text);

            if (lastDownloadDate > lastUpdateDate)
            {
                // Warnung, wenn das letzte Download-Datum jünger ist als das Upload-Datum
                DialogResult result = MessageBox.Show(
                    "Das letzte Download-Datum ist jünger als das Upload-Datum. Das könnte bedeuten, dass die Datei aktuell von Benutzer "+txtLastDOWNname.Text+" genutzt wird. Möchten Sie trotzdem fortfahren?",
                    "Warnung",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    txtStatus.Text = "Download abgebrochen.";
                    return;
                }
            }

            // 1. Liste der .sav-Dateien auf dem FTP-Server abrufen
            string ftpServer = txtFTP.Text;
            string ftpUser = txtUser.Text;
            string ftpPass = txtPass.Text;
            List<string> savFiles = new List<string>();

            try
            {
                FtpWebRequest listRequest = (FtpWebRequest)WebRequest.Create(ftpServer);
                listRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                listRequest.Credentials = new NetworkCredential(ftpUser, ftpPass);

                using (FtpWebResponse listResponse = (FtpWebResponse)listRequest.GetResponse())
                using (StreamReader reader = new StreamReader(listResponse.GetResponseStream()))
                {
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.EndsWith(".sav"))
                        {
                            savFiles.Add(line); // Alle .sav-Dateien sammeln
                        }
                    }
                }

                if (savFiles.Count == 0)
                {
                    MessageBox.Show("Keine .sav-Dateien auf dem Server gefunden.");
                    return;
                }

                // 2. Benutzer kann eine Datei auswählen
                string selectedFile = PromptForFileSelection(savFiles); // Methode zum Auswählen der Datei
                if (string.IsNullOrEmpty(selectedFile))
                {
                    MessageBox.Show("Keine Datei ausgewählt.");
                    return;
                }

                // 3. Zielpfad angeben lassen
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    FileName = selectedFile,
                    Filter = "SAV files (*.sav)|*.sav|All files (*.*)|*.*"
                };

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Kein Zielpfad angegeben.");
                    return;
                }

                string localFilePath = saveFileDialog.FileName;

                // 4. Datei vom FTP-Server herunterladen
                try
                {
                    string ftpFilePath = $"{ftpServer}/{selectedFile}";
                    FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(ftpFilePath);
                    downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    downloadRequest.Credentials = new NetworkCredential(ftpUser, ftpPass);

                    using (FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse())
                    using (Stream responseStream = downloadResponse.GetResponseStream())
                    using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        responseStream.CopyTo(fileStream);
                    }

                    MessageBox.Show("Savegame erfolgreich heruntergeladen.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Herunterladen des Savegames: " + ex.Message);
                    return;
                }

                // 5. XML-Datei mit neuen Properties aktualisieren
                string xmlFilePath = Path.Combine(Path.GetTempPath(), "updownstate.xml");
                UP_DOWN_State settings;

                if (File.Exists(xmlFilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(UP_DOWN_State));
                    using (FileStream fs = new FileStream(xmlFilePath, FileMode.Open))
                    {
                        settings = (UP_DOWN_State)serializer.Deserialize(fs);
                    }
                }
                else
                {
                    settings = new UP_DOWN_State();
                }

                // Properties aktualisieren (wie vorher beim Upload)
                settings.lastDOWNName = txtName.Text;
                settings.lastDOWNDate = DateTime.Now.ToString();

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UP_DOWN_State));
                using (FileStream fs = new FileStream(xmlFilePath, FileMode.Create))
                {
                    xmlSerializer.Serialize(fs, settings);
                }

                // 6. Aktualisierte XML-Datei auf den FTP-Server hochladen
                try
                {
                    string ftpXmlPath = $"{ftpServer}/updownstate.xml"; // Pfad auf dem FTP-Server
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpXmlPath);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                    request.UsePassive = true;

                    // Datei in einen Byte-Array lesen
                    byte[] fileContents;
                    using (StreamReader sourceStream = new StreamReader(xmlFilePath))
                    {
                        fileContents = System.Text.Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    }

                    request.ContentLength = fileContents.Length;

                    // Datei hochladen
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(fileContents, 0, fileContents.Length);
                    }

                    // Antwort vom Server abrufen
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    {
                        MessageBox.Show($"XML-Datei erfolgreich hochgeladen: {response.StatusDescription}");
                    }

                    txtStatus.Text = "state update pushed.";
                }
                catch (Exception ex)
                {
                    txtStatus.Text = "Fehler beim Hochladen der XML-Datei: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fehler: " + ex.Message;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }

    public class SSSettings
    {
        public string name { get; set; }
        public string ftpAddress { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string file { get; set; }
        public string pathToSave { get; set; }

    }

    public class UP_DOWN_State
    {
        public string lastUPName { get; set; }
        public string lastUPDate { get; set; }
        public string lastDOWNName { get; set; }
        public string lastDOWNDate { get; set; }
    }
}

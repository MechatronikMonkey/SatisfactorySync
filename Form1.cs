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
            sSSettings.file = txtFilename.Text;
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
            txtFilename.Text = settings.file;
            txtPathToSave.Text = settings.pathToSave;

        }

        private void btnUPLOAD_Click(object sender, EventArgs e)
        {
            string xmlFilePath = Path.Combine(Path.GetTempPath(), "updownstate.xml");
            UP_DOWN_State settings;

            // Prüfen, ob die Datei existiert
            if (File.Exists(xmlFilePath))
            {
                // Datei lesen und bestehende Einstellungen laden
                XmlSerializer serializer = new XmlSerializer(typeof(UP_DOWN_State));
                using (FileStream fs = new FileStream(xmlFilePath, FileMode.Open))
                {
                    settings = (UP_DOWN_State)serializer.Deserialize(fs);
                }
            }
            else
            {
                // Neue Instanz erstellen, wenn die Datei nicht existiert
                settings = new UP_DOWN_State();
            }

            // Update der gewünschten Properties
            settings.lastUPName = txtName.Text; // Zum Beispiel Property1 von textBox1
            settings.lastUPDate = DateTime.Now.ToString(); // Zum Beispiel Property2 von textBox2

            // XML-Datei speichern (entweder neu erstellen oder aktualisieren)
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UP_DOWN_State));
            using (FileStream fs = new FileStream(xmlFilePath, FileMode.Create))
            {
                xmlSerializer.Serialize(fs, settings);
            }

            // FTP-Verbindungsinformationen
            string ftpServer = txtFTP.Text; // Ersetze mit deinem FTP-Server
            string ftpUser = txtUser.Text; // Ersetze mit deinem Benutzernamen
            string ftpPass = txtPass.Text; // Ersetze mit deinem Passwort

            // Hochladen der XML-Datei
            string ftpFullPath = $"{ftpServer}/updownstate.xml"; // Pfad auf dem FTP-Server

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                request.ContentLength = new FileInfo(xmlFilePath).Length;

                // Datei in einen Byte-Array lesen
                byte[] fileContents;
                using (StreamReader sourceStream = new StreamReader(xmlFilePath))
                {
                    fileContents = System.Text.Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                }

                // Datei hochladen
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileContents, 0, fileContents.Length);
                }

                // Antwort vom Server abrufen
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    MessageBox.Show($"Upload erfolgreich: {response.StatusDescription}");
                }

                txtStatus.Text = "state update pushed.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fehler beim status update!" + ex.Message;
            }

            
        }

        private void checkFile_Tick(object sender, EventArgs e)
        {
            // FTP-Verbindungsinformationen
            string ftpServer = txtFTP.Text; // Ersetze mit deinem FTP-Server
            string ftpUser = txtUser.Text; // Ersetze mit deinem Benutzernamen
            string ftpPass = txtPass.Text; // Ersetze mit deinem Passwort

            string ftpFullPath = $"{ftpServer}/updownstate.xml"; // Pfad auf dem FTP-Server

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
                        // Hier kannst du die anderen Properties zuweisen, falls nötig
                        // textBox3.Text = settings.Property3;
                        // textBox4.Text = settings.Property4;
                    }
                }

                txtStatus.Text = "Status update erfolgreich...";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Fehler FTP!" + ex.Message;
            }
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

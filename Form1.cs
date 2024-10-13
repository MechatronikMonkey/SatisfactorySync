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
using SatisfactorySyncV0_2.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace V1
{
    public partial class SatisfactorySync : Form
    {
        public SatisfactorySync()
        {
            InitializeComponent();
        }

        private void btnSelectSave_Click(object sender, EventArgs e)
        {
            // Erstelle ein OpenFileDialog-Objekt
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Verwende eine Umgebungsvariable wie %LOCALAPPDATA%
            string localAppDataPath = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%");

            // Setze den aufgelösten Pfad als InitialDirectory
            openFileDialog.InitialDirectory = localAppDataPath + "\\FactoryGame\\Saved\\SaveGames";

            // Optional: Filter setzen, um nur bestimmte Dateitypen anzuzeigen
            openFileDialog.Filter = "SAV-Files (*.sav)|*.sav|All Files (*.*)|*.*";

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

            MessageBox.Show("Settings successfully saved.");
        }

        private void SatisfactorySync_Load(object sender, EventArgs e)
        {
            // Dateipfad der gespeicherten Settings festlegen
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SatisfactorySyncSettings.json");

            try
            {
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
            catch { }
        }

        private void btnUPLOAD_Click(object sender, EventArgs e)
        {
            bool isfirstsetup = false;

            // 0. Vergleich der Datumswerte
            try
            {
                DateTime lastDownloadDate = DateTime.Parse(txtLastDOWNdate.Text);
            }
            catch { }

            try
            {
                DateTime lastUpdateDate = DateTime.Parse(txtLastUPdate.Text);
            }
            catch 
            {
                //never uploaded before, looks like we have first setup
                isfirstsetup = true;
            }

            if (txtName.Text != txtLastDOWNname.Text && isfirstsetup == false)
            {
                // Warnung, anderer User im Spiel
                DialogResult result = MessageBox.Show(
                    "Someone else downloaded the game after you! The User \n" + txtLastDOWNname.Text + "\n has downloaded the game in parallel. Are you sure you still want to upload the game?",
                    "Warnung",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No)
                {
                    txtStatus.Text = "Upload canceled.";
                    return;
                }
            }

            string xmlFilePath = Path.Combine(Path.GetTempPath(), txtSettingsFile.Text);
            string binaryFilePath = txtPathToSave.Text; // Absoluter Pfad zur bestehenden Binärdatei

  
            // Neue Instanz erstellen, wenn die XML-Datei nicht existiert
            UP_DOWN_State settings = new UP_DOWN_State();
            

            // Update der gewünschten Properties
            settings.lastUPName = txtName.Text;
            settings.lastUPDate = DateTime.Now.ToString();
            settings.lastDOWNDate = txtLastDOWNdate.Text;
            settings.lastDOWNName = txtLastDOWNname.Text;
            

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
                    MessageBox.Show("The save game does not exist!");
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
                    MessageBox.Show($"Save game successfully uploaded: {binaryResponse.StatusDescription}");
                }

                txtStatus.Text = "Save game successfully uploaded.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Error uploading the save game:" + ex.Message;
                return; // Abbrechen, falls der Upload fehlschlägt
            }

            // 2. XML-Datei hochladen
            try
            {
                string ftpFullPath = $"{ftpServer}/" + txtSettingsFile.Text; // Pfad auf dem FTP-Server
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
                    MessageBox.Show($"XML file successfully updated: {response.StatusDescription}");
                }

                txtStatus.Text = "state update pushed.";
            }
            catch (Exception ex)
            {
                txtStatus.Text = "Error uploading the XML file: " + ex.Message;
            }
        }


        private void checkFile_Tick(object sender, EventArgs e)
        {
            // FTP-Verbindungsinformationen
            string ftpServer = txtFTP.Text; // Ersetze mit deinem FTP-Server
            string ftpUser = txtUser.Text; // Ersetze mit deinem Benutzernamen
            string ftpPass = txtPass.Text; // Ersetze mit deinem Passwort

            int startPosition = -1;
            List<byte> extractedBytes = new List<byte>(); // Vor dem if-Block deklariert
            byte[] saveGameBytes = null; // Vor dem if-Block deklariert
            int gameNameByteCount = 0;

            string ftpFullPath = $"{ftpServer}/{txtSettingsFile.Text}"; // Pfad auf dem FTP-Server

            try
            {
                string txtSavegameFile = Path.GetFileName(txtPathToSave.Text);
                string ftpSaveGamePath = $"{ftpServer}/{txtSavegameFile}"; // Pfad zur SaveGame-Datei

                // Anfrage für die Header-Bytes
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpFullPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                request.Headers.Add("Range", "bytes=0-4094"); // Range von 0 bis 4094 (4095 Bytes)

                // XML deserialisieren und Properties laden
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    string xmlContent = reader.ReadToEnd();
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

                // SaveGame-Datei herunterladen
                request = (FtpWebRequest)WebRequest.Create(ftpSaveGamePath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPass);

                using (FtpWebResponse saveGameResponse = (FtpWebResponse)request.GetResponse())
                using (Stream saveGameStream = saveGameResponse.GetResponseStream())
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    saveGameStream.CopyTo(memoryStream);
                    saveGameBytes = memoryStream.ToArray(); // Jetzt global verfügbar

                    // Nach "SessionDefinition=" suchen
                    string searchWord = "SessionDefinition=";
                    byte[] searchBytes = System.Text.Encoding.ASCII.GetBytes(searchWord);
                    int position = IndexOfBytes(saveGameBytes, searchBytes);

                    if (position != -1)
                    {
                        // Startposition nach dem '='-Zeichen
                        startPosition = position + searchBytes.Length;

                        // Bytes extrahieren bis zum ersten 0x00 Byte
                        for (int i = startPosition; i < saveGameBytes.Length; i++)
                        {
                            if (saveGameBytes[i] == 0x00)
                            {
                                break; // Beenden, wenn das erste 0x00 Byte gefunden wird
                            }
                            extractedBytes.Add(saveGameBytes[i]);
                        }

                        // Gefundene Bytes in ein Wort umwandeln.
                        txtServerSessDefinition.Text = System.Text.Encoding.ASCII.GetString(extractedBytes.ToArray());
                    }
                }

                // Weiter suchen, bis kein 0x00 Byte mehr vorhanden ist
                int gameNameStartPosition = -1;
                for (int i = startPosition + extractedBytes.Count; i < saveGameBytes.Length; i++)
                {
                    if (saveGameBytes[i] != 0x00)
                    {
                        gameNameStartPosition = i;
                        break; // Die erste Position, bei der das Byte kein 0x00 ist, ist der Anfang des GameNames
                    }
                }

                // Dieses Byte muss übersprungen werden, es ist vermutlich eine Checksumme.
                // Suche nun weiter bis wieder Text beginnt.
                for (int i = gameNameStartPosition + 1; i < saveGameBytes.Length; i++)
                {
                    if (saveGameBytes[i] != 0x00)
                    {
                        gameNameStartPosition = i;
                        break; // Die erste Position, bei der das Byte kein 0x00 ist, ist der Anfang des GameNames
                    }
                }

                if (gameNameStartPosition != -1)
                {
                    // Jetzt den GameName extrahieren, bis wieder ein 0x00 Byte gefunden wird
                    List<byte> gameNameBytes = new List<byte>();
                    for (int i = gameNameStartPosition; i < saveGameBytes.Length; i++)
                    {
                        if (saveGameBytes[i] == 0x00)
                        {
                            break; // Beenden, wenn ein 0x00 Byte gefunden wird (Ende des GameNames)
                        }
                        gameNameBytes.Add(saveGameBytes[i]);
                    }

                    // Gefundene Bytes in einen lesbaren String umwandeln (GameName)
                    txtServerGameName.Text = System.Text.Encoding.ASCII.GetString(gameNameBytes.ToArray());
                    gameNameByteCount = gameNameBytes.Count;
                }

                // Extrahiere die 4 Bytes nach dem 0x00-Byte (Zeitstempel)
                int timePosition = gameNameStartPosition + gameNameByteCount + 1; // 1 für das 0x00 Byte

                if (timePosition + 4 <= saveGameBytes.Length)
                {
                    byte[] timeBytes = new byte[4];
                    Array.Copy(saveGameBytes, timePosition, timeBytes, 0, 4);

                    // 32-Bit-Wert in Sekunden umwandeln
                    int durationInSeconds = BitConverter.ToInt32(timeBytes, 0);

                    // Umwandlung in Stunden, Minuten und Sekunden
                    TimeSpan duration = TimeSpan.FromSeconds(durationInSeconds);
                    string formattedDuration = $"{(int)duration.TotalHours:D2}H {duration.Minutes:D2}m {duration.Seconds:D2}s";

                    // Ausgabe der Dauer
                    txtServerPlaytime.Text = formattedDuration; // Ausgabe als Stunden:Minuten:Sekunden
                }

                txtStatus.Text = "Status update successful...";
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
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

                System.Windows.Forms.Button selectButton = new System.Windows.Forms.Button { Text = "Pick", Dock = DockStyle.Bottom };
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
            try
            {
                DateTime lastUpdateDate = DateTime.Parse(txtLastUPdate.Text);
                DateTime lastDownloadDate = DateTime.Parse(txtLastDOWNdate.Text);
            

                if (lastDownloadDate > lastUpdateDate)
                {
                    // Warnung, wenn das letzte Download-Datum jünger ist als das Upload-Datum
                    DialogResult result = MessageBox.Show(
                        "The last download date is more recent than the upload date. This could mean that the file is currently being used by the user \n" + txtLastDOWNname.Text+ "\n. Do you still want to proceed?",
                        "Warnung",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.No)
                    {
                        txtStatus.Text = "Download canceled.";
                        return;
                    }
                }
            }
            catch { }

            
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
                    MessageBox.Show("No .sav files found on the server.");
                    return;
                }

                string selectedFile;
                string localFilePath;

                if (txtPathToSave.Text == "") // Wenn kein Pfad für Savegame gesetzt, wähle eines aus.
                {
                    // 2. Benutzer kann eine Datei auswählen
                    selectedFile = PromptForFileSelection(savFiles); // Methode zum Auswählen der Datei
                    if (string.IsNullOrEmpty(selectedFile))
                    {
                        MessageBox.Show("No file selected.");
                        return;
                    }

                    // 3. Zielpfad angeben lassen
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        FileName = selectedFile,
                        Filter = "SAV files (*.sav)|*.sav|All files (*.*)|*.*"
                    };

                    // Verwende eine Umgebungsvariable wie %LOCALAPPDATA%
                    string localAppDataPath = Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%");

                    // Setze den aufgelösten Pfad als InitialDirectory
                    saveFileDialog.InitialDirectory = localAppDataPath + "\\FactoryGame\\Saved\\SaveGames";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        MessageBox.Show("No target path specified.");
                        return;
                    }

                    localFilePath = saveFileDialog.FileName;
                }
                else
                {
                    selectedFile = Path.GetFileName(txtPathToSave.Text);
                    localFilePath = txtPathToSave.Text;
                }

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

                    MessageBox.Show("Save game successfully downloaded.");
                    txtPathToSave.Text = localFilePath;

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

                    MessageBox.Show("Settings have been successfully saved.");

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error downloading the save game:" + ex.Message);
                    return;
                }

                // 5. XML-Datei mit neuen Properties aktualisieren
                string xmlFilePath = Path.Combine(Path.GetTempPath(), txtSettingsFile.Text);
                UP_DOWN_State settings = new UP_DOWN_State();
                

                // Properties aktualisieren (wie vorher beim Upload)
                settings.lastDOWNName = txtName.Text;
                settings.lastDOWNDate = DateTime.Now.ToString();
                settings.lastUPName = txtLastUPname.Text;
                settings.lastUPDate = txtLastUPdate.Text;

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UP_DOWN_State));
                using (FileStream fs = new FileStream(xmlFilePath, FileMode.Create))
                {
                    xmlSerializer.Serialize(fs, settings);
                }

                // 6. Aktualisierte XML-Datei auf den FTP-Server hochladen
                try
                {
                    string ftpXmlPath = $"{ftpServer}/" + txtSettingsFile.Text; // Pfad auf dem FTP-Server
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
                        MessageBox.Show($"XML file successfully updated: {response.StatusDescription}");
                    }

                    txtStatus.Text = "File successfully downloaded.";
                }
                catch (Exception ex)
                {
                    txtStatus.Text = "Error uploading the XML file: " + ex.Message;
                }
            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }

        private void btnListXMLs_Click(object sender, EventArgs e)
        {
            // 1. Verbindung zum FTP-Server herstellen und XML-Dateien abrufen
            string ftpServer = txtFTP.Text;
            string ftpUser = txtUser.Text;
            string ftpPass = txtPass.Text;
            List<string> xmlFiles = new List<string>();

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
                        if (line.EndsWith(".xml"))
                        {
                            xmlFiles.Add(line); // Alle .xml-Dateien sammeln
                        }
                    }
                }

                if (xmlFiles.Count == 0)
                {
                    MessageBox.Show("No .xml files found on the server!");
                    return;
                }

                // 2. Benutzer kann eine Datei auswählen
                string selectedFile = PromptForFileSelection(xmlFiles); // Methode zum Auswählen der Datei
                if (string.IsNullOrEmpty(selectedFile))
                {
                    MessageBox.Show("No file selected.");
                    return;
                }

                // 3. Ausgewählte Datei in ein Textfeld schreiben
                txtSettingsFile.Text = selectedFile; // Textfeld, um den Dateinamen zu schreiben
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            // Erstelle ein neues SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                FileName = "SatisfSyncSettings.xml", // Standarddateiname
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*" // Dateitypenfilter
            };

            // Zeige den Dialog an und prüfe, ob der Benutzer einen Pfad ausgewählt hat
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    // Exportiere die Settings in die XML-Datei
                    SSSettings settings = new SSSettings
                    {
                        name = txtName.Text,
                        ftpAddress = txtFTP.Text,
                        user = txtUser.Text,
                        pass = txtPass.Text,
                        file = txtSettingsFile.Text,
                    };

                    // XML-Datei serialisieren
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SSSettings));
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        xmlSerializer.Serialize(fs, settings);
                    }

                    // Erfolgsmeldung anzeigen
                    MessageBox.Show("Settings successfully exported.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting the settings: " + ex.Message);
                }
            }
            else
            {
                // Benutzer hat den Speichervorgang abgebrochen
                MessageBox.Show("Save canceled.");
            }
        }

        // Methode, um eine Eingabemaske für den Namen anzuzeigen
        private string PromptForNameInput(string defaultName)
        {
            using (Form form = new Form())
            {
                form.Text = "Enter your name:";

                Label label = new Label { Text = "Name:", Dock = DockStyle.Top };
                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox { Text = defaultName, Dock = DockStyle.Top };

                System.Windows.Forms.Button confirmButton = new System.Windows.Forms.Button { Text = "Confirm", Dock = DockStyle.Bottom };

                form.Controls.Add(label);
                form.Controls.Add(textBox);
                form.Controls.Add(confirmButton);
                form.StartPosition = FormStartPosition.CenterParent;
                form.Size = new Size(300, 150);

                string inputName = null;
                confirmButton.Click += (sender, e) =>
                {
                    inputName = textBox.Text;
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                };

                form.ShowDialog();
                return inputName;
            }
        }
        private void btnImportSettings_Click(object sender, EventArgs e)
        {
            // Erstelle ein OpenFileDialog, um eine XML-Datei auszuwählen
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*", // Dateitypenfilter
                Title = "Select the settings XML file."
            };

            // Zeige den Dialog an und prüfe, ob der Benutzer eine Datei ausgewählt hat
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;

                try
                {
                    // XML-Datei deserialisieren
                    SSSettings settings;
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SSSettings));
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        settings = (SSSettings)xmlSerializer.Deserialize(fs);
                    }

                    // Eingabemaske für den Namen anzeigen, mit dem Namen aus der XML-Datei als Standardwert
                    string inputName = PromptForNameInput(settings.name);
                    if (string.IsNullOrEmpty(inputName))
                    {
                        MessageBox.Show("Input canceled.");
                        return;
                    }

                    // Übernommene Werte in die Textfelder schreiben
                    txtName.Text = inputName; // Name aus der Eingabemaske
                    txtFTP.Text = settings.ftpAddress;
                    txtUser.Text = settings.user;
                    txtPass.Text = settings.pass;
                    txtSettingsFile.Text = settings.file;

                    MessageBox.Show("Settings successfully imported.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error importing the settings: " + ex.Message);
                }
            }
            else
            {
                // Benutzer hat den Öffnungsvorgang abgebrochen
                MessageBox.Show("Open canceled.");
            }
        }

        // Hilfsfunktion, um das Byte-Array zu durchsuchen
        int IndexOfBytes(byte[] haystack, byte[] needle)
        {
            // Randfallbehandlung: Falls das Suchmuster länger als das Byte-Array ist
            if (needle.Length == 0 || haystack.Length < needle.Length)
            {
                return -1;
            }

            // Schleife durch das Haupt-Array
            for (int i = 0; i <= haystack.Length - needle.Length; i++)
            {
                // Vergleich der Byte-Folge mit dem Suchmuster
                bool match = true;
                for (int j = 0; j < needle.Length; j++)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i; // Rückgabe des Index des ersten Vorkommens
                }
            }

         return -1; // Wenn das Muster nicht gefunden wird
        }

        void ExtractBytesUntilNull(byte[] sourceBytes, List<byte> targetBytes, int start)
        {
            for (int i = start; i < sourceBytes.Length; i++)
            {
                if (sourceBytes[i] == 0x00)
                {
                    break; // Beenden, wenn das erste 0x00 Byte gefunden wird
                }
                targetBytes.Add(sourceBytes[i]);
            }
        }

        int FindNextNonNullByte(byte[] bytes, int start)
        {
            for (int i = start; i < bytes.Length; i++)
            {
                if (bytes[i] != 0x00)
                {
                    return i; // Rückgabe der ersten Position, bei der das Byte kein 0x00 ist
                }
            }
            return -1; // Wenn kein non-null Byte gefunden wird
        }

        void ExtractAndDisplayDuration(byte[] saveGameBytes, int timePosition, System.Windows.Forms.TextBox txtPlaytime)
        {
            if (timePosition + 4 <= saveGameBytes.Length)
            {
                byte[] timeBytes = new byte[4];
                Array.Copy(saveGameBytes, timePosition, timeBytes, 0, 4);

                // 32-Bit-Wert in Sekunden umwandeln
                int durationInSeconds = BitConverter.ToInt32(timeBytes, 0);

                // Umwandlung in Stunden, Minuten und Sekunden
                TimeSpan duration = TimeSpan.FromSeconds(durationInSeconds);
                string formattedDuration = $"{(int)duration.TotalHours:D2}H {duration.Minutes:D2}m {duration.Seconds:D2}s";

                // Ausgabe der Dauer
                txtPlaytime.Text = formattedDuration; // Ausgabe als Stunden:Minuten:Sekunden
            }
        }

        private void tabSync_Click(object sender, EventArgs e)
        {

        }

        private void txtLastUPname_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtLastUPdate_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
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

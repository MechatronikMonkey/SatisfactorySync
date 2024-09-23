# SatisfactorySync v0.1.0 - alpha

This is the first version of my Satisfactory Savegame Sync Tool.

## Use cases
If you play satisfactory with lets say 3 people; A, B and C.
A starts the Server and you all play together. 

The next day, A has not time to play (and start the server for B and C).
But B and C want to continue the game without A.
B or C needs the save game to be able to load and continue the game.

SatisfactorySync helps doing that task with the help of an FTP Sever

### Why FTP?
Everyone can get a free FTP server nowadays. You do not need a complicated api key, cloud access or crap.
Just the FTP login credentials and you are good to go. Share the credentials with your frieds who play.
And do not hand them to others and you should be save.

## Installation
Unzip the downloaded Release into any folder you like.
![image](https://github.com/user-attachments/assets/6b1aeb6a-da5f-412e-940d-48dd7c6e26b9)

Best would be some space in your user path like C:\Users\yourUserName\SatisfactorySync

Enter folder and Run SatisfactorySyncV0_1.exe

## First Start
As the software is not signed, windows complains about that.
click "more information" to show an extra icon

![image](https://github.com/user-attachments/assets/2d91886b-d9a8-4597-add9-6dde925f5b6c)


Click "Run"

![image](https://github.com/user-attachments/assets/688d3f2b-766c-43fa-be1e-91aa4e81ca18)


After the first start, the software tries to connect to the ftp server.
This should fail as you have not entered the credentials.
![image](https://github.com/user-attachments/assets/89154c84-d415-4785-9e71-c6c04278024f)


This is all ok for now.
Form here there are **two cases**. You may download a game a friends already uploaded or you first upload a game.

## Download existing game ##

### Import Settings ###
If your friend has started a game, he should export his settings (see below) and provide the settings file to you.
Save it to your computer and open it with import... under Settings tab.

![image](https://github.com/user-attachments/assets/50051bad-0447-4ca2-8328-58ba1648c401)

After you selected the file you are prompted to enter your name. Lets assume we are player-B

![image](https://github.com/user-attachments/assets/c54a9961-346f-4159-a7c0-316857015492)

After successfull import of the proper settings you should see "Status update erfolgreich..." on the bottom of the app.

![image](https://github.com/user-attachments/assets/f0db2fef-b9aa-4509-b3bf-d9782cb95c69)

### Manually insert settings ###

If you enter your credentials manually, you first get an error message as long as no settings file is provided.
To make it easy for you to pick the correct settings file, you can use the picker.

![image](https://github.com/user-attachments/assets/b84d0cc7-b8a8-4cc3-a480-3ea938a6fa4c)

Pick the correct settings file to your game. You may need to talk to your frieds ;-)

![image](https://github.com/user-attachments/assets/2b5d3882-c300-4afd-8d17-b8bbc7244fae)

After picking the correct settings file you should see proper connection on the bottom of the app.

![image](https://github.com/user-attachments/assets/f0db2fef-b9aa-4509-b3bf-d9782cb95c69)

**DO NOT ENTER A SAVEGAME path if you are DOWNLOADING a save game for the first time!** 
You will be prompted on downloading the first time.

Press **Save**

## Start a new game
First of all get the ftp credentials of a free ftp server or your webspace ftp or the fpt of a friend or so.
Switch to settings tab and enter the credentials along with your name.

For this example we are player-A.

![image](https://github.com/user-attachments/assets/d322b952-db0d-45e0-ac99-8a06fe13cf4f)

1. Enter YOUR Name here
2. Enter FTP credentials
3. Enter a NEW NAME (must be unique on server) for a settings file. Give it a good name, mabe the players curently playing. 
   In this case we assume that Player A, B and C are playing.
   Must be .xml in the end! **Enter this manually**. DO NOT pick it with the picker if you create a new game!

4. Use the picker to select the save game you want to share with your frieds. It tries to throw you in the default save game path
   of satisfactory. However, this may fail. Your save game should be located C:\Users\yourUsername\AppData\Local\FactoryGame\Saved\SaveGames\YourSteamID\YourSaveGameName.sav

   ![image](https://github.com/user-attachments/assets/1c07f601-3c8b-48ca-8383-00a893222cb3)

   pick it and press open.

Now you have entered all you need to upload your first save game.

![image](https://github.com/user-attachments/assets/c5c59cbe-b333-4f45-b8f6-b5cedb977941)

Press SAVE and go back to the Sync Page for the first upload.

### First Upload ###
Switch to Sync Tab if not allready done.
As you see there is no name or date for up and downloads of this savegame to your ftp server.
This is because you never uploaded the savegame.

So klick the green upload arrow to first upload the save game to the server.
After clicking the upload button you should see **two** "226 Transfer complete" message, one for the sav game and one for the xml settings file.

![image](https://github.com/user-attachments/assets/fd937291-916f-4cfe-bbd8-abcf294e02d1)

After that is completed, you see "Status update erfolgreich..." in the bottom and the LastUploaded from and last UPloaded date filled.

![image](https://github.com/user-attachments/assets/8e651785-08c0-4cc7-8741-bc08ed7fd4e2)

## Export settings ###
After your first upload you should save the settings for your friends to make it easyier for them to setup.
Go to Settings tab and click export

![image](https://github.com/user-attachments/assets/03b5d373-6e48-47ee-a5ec-4b4c426c9a0a)

Save the xml file and share it with your friends by discord or email.





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
Best would be some space in your user path like C:\Users\yourUserName\SatisfactorySync
Run SatisfactorySyncV0_1.exe

## First Start

After the first start, the software tries to connect to the ftp server.
This should fail as you have not entered the credentials.
So form here there are two cases. You may download a game a friends already uploaded or you first upload a game.

### Download existing Game

Opitonal:
If your friend has started a game, he should export his settings to an XML file and provide this settings file to you.
On the settings page click "import..." to locate the file and import the settings. 
You get asked if you want to change the name to your own (what you normaly want).

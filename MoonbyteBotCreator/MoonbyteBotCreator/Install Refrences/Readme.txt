PythonInstall.ps1 automatically install's Python v3.6.7 64 bit in the background.
The python installer ask's the user for administrator privs, and after that it install's
Python to C:\Python36\

Also at the end of the file, it installs numpy, python SpeechRecognition lib and PyAudio.
This powershell script also sets enviorment vars directed to pip and python on the user's computer.
It's designed to install everything in the background and give the user the best experience if they arn't tech savy.

Hope you enjoy :)

Sorry for spelling and grammar errors, I put this togeather super quickly & I honestly don't care rn I gotta
get this out for my upcoming project on my browser. I will update it in the future!!

I recomend running PythonInstall.ps1 while you are installing your application in the background without a window,
this will give the user the best experience, and its kinda designed for that.

Haven't tested of SpeechRecognition and PyAudio installs automatically, but I've tested if it install's python
correctly and set the enviormental variables for the user, and it does!

you might get a couple of errors from this file, I don't intend to fix them because this should be ran in the background
and it doesn't seem like it causes any issues.
import paramiko
import os.path
import time
import sys
import re

userFilePath = input("\n# Enter user file path and name (e.g. D:\\my\\file.txt): ")

if os.path.isfile(userFilePath) == True:
    print("\n** Username/password file is valid. **\n")

else:
    print("\n** File {} does not exist. Please check and try again. **\n".format(userFilePath))
    sys.exit()   

cmdFilePath = input("\n# Enter commands file path and name (e.g. D:\\my\\file.txt): ")

if os.path.isfile(cmdFilePath) == True:
    print("\n** Command file is valid. **\n")

else:
    print("\n** File {} does not exist. Please check and try again. **\n".format(cmdFilePath))
    sys.exit()
    
def sshConnection(ip):
    
    global userFilePath
    global cmdFilePath
    
    #Creating SSH CONNECTION
    try:
        #Define SSH parameters
        userFile = open(userFilePath, 'r')
        
        #Starting from the beginning of the file
        userFile.seek(0)
        
        #Reading the username from the file
        userName = userFile.readlines()[0].split(',')[0].rstrip("\n")
        
        #Starting from the beginning of the file
        userFile.seek(0)
        
        #Reading the password from the file
        passWord = userFile.readlines()[0].split(',')[1].rstrip("\n")
        
        #Logging into device
        session = paramiko.SSHClient()
        
        #For testing purposes, this allows auto-accepting unknown host keys
        #Do not use in production! The default would be RejectPolicy
        session.set_missing_host_key_policy(paramiko.AutoAddPolicy())
        
        #Connect to the device using username and password          
        session.connect(ip.rstrip("\n"), username = userName, password = passWord)
        
        #Start an interactive shell session on the router
        connection = session.invoke_shell()	
        
        #Setting terminal length for entire output - disable pagination
        connection.send("enable\n")
        connection.send("terminal length 0\n")
        time.sleep(1)
        
        #Entering global config mode
        connection.send("\n")
        connection.send("configure terminal\n")
        time.sleep(1)
        
        #Open user selected file for reading
        cmdFile = open(cmdFilePath, 'r')
            
        #Starting from the beginning of the file
        cmdFile.seek(0)
        
        #Writing each line in the file to the device
        for eachLine in cmdFile.readlines():
            connection.send(eachLine + '\n')
            time.sleep(2)
        
        #Closing the user file
        userFile.close()
        
        #Closing the command file
        cmdFile.close()
        
        #Checking command output for IOS syntax errors
        routerOutput = connection.recv(65535)
        
        if re.search(b"% Invalid input", routerOutput):
            print("* There was at least one IOS syntax error on device {} :(".format(ip))
            
        else:
            print("\nDONE for device {} :)\n".format(ip))
            
        #Test for reading command output
        print(str(routerOutput) + "\n")
        #print(re.findall(r"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}",str(routerOutput))[1])
        
        #Closing the connection
        session.close()
     
    except paramiko.AuthenticationException:
        print("** Invalid username or password. **\n** Please check the username/password file or the device configuration. **")
        print("** Closing program... Bye! **")
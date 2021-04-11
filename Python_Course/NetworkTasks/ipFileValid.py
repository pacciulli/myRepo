import os.path
import sys

#Verify if the IP file exists and return the list of IPs
def ipFileValid():
    
    filePath = input("\n# Enter IP file path and name (e.g D:\\my\\IP\\file.txt): ")
    
    #Check if file exist or not
    if os.path.isfile(filePath):
        print("\n** IP file is valid **\n")
    
    else:
        print("\n** File {} does not exist. Please check and try again! **\n".format(filePath))
        sys.exit()
    
    ipFile = open(filePath, 'r')
    
    #To guarantee that the cursor is in the start of the file
    ipFile.seek(0)
    
    ipList = ipFile.readlines()
    
    for x in range(0,len(ipList)):
        ipList[x] = ipList[x].rstrip('\n')
    
    ipFile.close()
    
    return ipList
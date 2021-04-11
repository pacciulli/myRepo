import sys

#This function check if there is an invalid IP address in the list provided
#It looks into each octet from each IP from the list and search for invalid
#option such as boradcast IPs (255.x.x.x), local IPs (127.x.x.x), etc...
def ipAddrValid(ipList):
    
    for ip in ipList:
        octetList = ip.split(".")
        
        if (len(octetList) == 4) and (1 <= int(octetList[0]) <= 223) and \
            (int(octetList[0]) != 127) and \
            (int(octetList[0]) != 169 or int(octetList[1]) != 254) and \
            (0 <= int(octetList[1]) <= 255 and 0 <= int(octetList[2]) <= 255 and 0 <= int(octetList[3]) <= 255):
               
            continue
            
        else:
            print("\n** There is an invalid IP address in the file. Please check and try again**\n")
            sys.exit()
import sys
import subprocess
 
#Checking if each IP in the list is reachable (ping)
def ipReach(ipList):
 
    for ip in ipList:
        
        #This method execute a ping for IP and suppress the ansers from prompt
        #If return is 0 means that ping was successful
        pingReply = subprocess.call("ping %s /n 2" % (ip), \
                        stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        
        if pingReply == 0:
            print("\n** {} is reachable **\n".format(ip))
            continue
        
        else:
            print("\n** {} not reachable. Check connectivity and try again. **".format(ip))
            sys.exit()
#Importing the necessary modules
import sys

from ipFileValid import ipFileValid
from ipAddrValid import ipAddrValid
from ipReach import ipReach
from sshConnection import sshConnection
from createThreads import createThreads

#Saving the list of IP addresses in ip.txt to a variable
ipList = ipFileValid()

#Verifying the validity of each IP address in the list
try:
    ipAddrValid(ipList)
    
except KeyboardInterrupt:
    print("\n\n** Program aborted by user. Exiting...\n")
    sys.exit()

#Verifying the reachability of each IP address in the list
try:
    ipReach(ipList)
    
except KeyboardInterrupt:
    print("\n\n** Program aborted by user. Exiting...\n")
    sys.exit()

#Calling threads creation function for one or multiple SSH connections
createThreads(ipList, sshConnection)

#End of program
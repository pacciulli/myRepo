""" Iteractive Scientific Calculator

This code refers to section 18 of the course Python Complete Masterclass for
Beginners """

import math

#This class defines and execute all operations of the calculator
class operation(object):
    def Addition(self):
        num1 = float(input("\nNumber 1: "))
        num2 = float(input("Number 2: "))
        
        self.result = num1 + num2
        
        return self.result

    def Subtraction(self):
        num1 = float(input("\nNumber 1: "))
        num2 = float(input("Number 2: "))
        
        self.result = num1 - num2
        
        return self.result
    
    def Multiplication(self):
        num1 = float(input("\nNumber 1: "))
        num2 = float(input("Number 2: "))
        self.result = num1 * num2
        
        return self.result

    def Division(self):
        num1 = float(input("\nNumber 1: "))
        num2 = float(input("Number 2: "))
        
        #To prevent division by 0.0 (zero)
        while num2 == 0:
            print("\nCannot divide per 0.0!")
            num2 = float(input("\nInput Number 2 again: "))
        
        self.result = num1 / num2
        
        return self.result

    def Module(self):
        num1 = float(input("\nNumber 1: "))
        num2 = float(input("Number 2: "))
        
        self.result = num1 % num2
        
        return self.result

    def Power(self):
        #Receive numbers
        num1 = float(input("\nBase: "))
        num2 = float(input("Power: "))
        
        self.result = math.pow(num1, num2)
        
        return self.result

    def Square_root(self):
        #Receive numbers
        num1 = float(input("\nNumber: "))
        
        self.result = math.sqrt(num1)
        
        return self.result

    def Logareithm(self):
        #Receive numbers
        num1 = float(input("\nNumber: "))
        
        self.result = math.log2(num1)
        
        return self.result

    def Sine(self):
        #Receive numbers
        num1 = float(input("\nAngle(deg): "))
        
        self.result = math.sin(math.radians(num1))
        
        return self.result
    
    def Cossine(self):
        #Receive numbers
        num1 = float(input("\nAngle(deg): "))
        
        self.result = math.cos(math.radians(num1))
        
        return self.result

    def Tangent(self):
        #Receive numbers
        num1 = float(input("\nAngle(deg): "))
        
        self.result = math.tan(math.radians(num1))
        
        return self.result

    def Exit(self):
        print("\nThank you for using this calculator!")
        return "exit"

#Function to print main menu
def printMenu(menu):
    print("\nAvailable operations:")
    for x in range(1,len(menu)):
        print(str(x) + " - " + menu[str(x)])
    print("\n" + str(0) + " - " + menu[str(0)])

#Dictionary with possible options for menu
optionDict = {"1" : "Addition", "2" : "Subtraction",
        "3" : "Multiplication", "4" : "Division", "5" : "Module",
        "6" : "Power", "7" : "Square_root", "8" : "Logarithm",
        "9" : "Sine", "10" : "Cossine", "11" : "Tangent", "0" : "Exit"}

#Declaring object as operation class
operator = operation()

#Main loop
while True:
    
    printMenu(optionDict)
   
    #Receive input from user
    userChoice = input("\nSelect an operation: ")
    
    #Operation to be executed
    try:
        toExecute = getattr(operator, optionDict[userChoice])
    
        result = toExecute()
        if result == "exit":
            break
        else:
            print("Restult: %.4f" % result)
            back = input("\nReturn to main menu? (y/n): ")
            if back == "y":
                continue
            else:
                break
        
    except KeyError:
        print("\nOperation invalid! Please select a valid operation!")
        continue
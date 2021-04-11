import psycopg2

#Open text file
file = open("D:\GIT Repo\myRepo\Python_Course\Database\employees.txt", "r")

#Reading text file
data = []

for line in file.readlines():
    data.append(line.rstrip("\n").split("/ "))

#Closing file
file.close()

#Connect to database 
try:
    connection = psycopg2.connect(database="staff", user = "caue",
                                    password = "python",
                                    host = "127.0.0.1",
                                    port = "5432")
    
except psycopg2.Error as err:
    print("An error was generated!")
    
else:
    print("Connection to database was successful!")
    
#Initialize cursor
cursor = connection.cursor()

#Loop to insert data to database
try:
    for i in data:
        cursor.execute("insert into mystaff.employees " \
        "(id,first_name,last_name,department,phone,address,salary) " \
        "values (%s,%s,%s,%s,%s,%s,%s);",
        (i[0],i[1],i[2],i[3],i[4],i[5],i[6]))

except psycopg2.Error as err:
    print("An error was generated while inserting the records!")
    
else:
    print("Records inserted successfully!\n")

#commit database
connection.commit()
 
#close connection with database
connection.close()
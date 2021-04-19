import pandas
from sqlalchemy import create_engine

#Loading data from files
dataTxt = pandas.read_csv("D:\\GIT Repo\\myRepo\\Python_Course\\DataAnalysis\\my_employees.txt")
dataCsv = pandas.read_csv("D:\\GIT Repo\\myRepo\\Python_Course\\DataAnalysis\\my_employees.csv")
dataJson = pandas.read_json("D:\\GIT Repo\\myRepo\Python_Course\\DataAnalysis\\my_employees.json")
dataXlsx = pandas.read_excel("D:\\GIT Repo\myRepo\\Python_Course\\DataAnalysis\\my_employees.xlsx", sheet_name = 0)

#Loading data from SQL database
engine = create_engine('postgresql+psycopg2://caue:python@127.0.0.1:5432/staff')
dataSql = pandas.read_sql_table('employees', engine, schema = 'mystaff')

#Due to heading of SQL be all lowercase, it is necessary to rename the headings so all data headings
#be the same
dataSql.rename({"id": "ID", "first_name": "FirstName", "last_name": "LastName", "department": "Department",  
                "phone": "Phone", "address": "Address", "salary": "Salary"}, axis = 'columns', inplace = True)

#Writing data to new SQL table "allstaff" from files and old SQL database. Parameter "if_exists" check if
#there is already a table with that name and append to it.
dataTxt.to_sql('allstaff', engine, schema = 'mystaff', index = False)
dataCsv.to_sql('allstaff', engine, schema = 'mystaff', index = False, if_exists = 'append')
dataJson.to_sql('allstaff', engine, schema = 'mystaff', index = False, if_exists = 'append')
dataXlsx.to_sql('allstaff', engine, schema = 'mystaff', index = False, if_exists = 'append')
dataSql.to_sql('allstaff', engine, schema = 'mystaff', index = False, if_exists = 'append')

#Building HTML summary report
queryAll = pandas.read_sql_query('SELECT * FROM mystaff.allstaff', engine)
#print(queryAll)

queryCount = pandas.read_sql_query('SELECT COUNT (*) FROM mystaff.allstaff', engine)
totalEmployees = queryCount.iloc[0][0]

queryDept = pandas.read_sql_query('SELECT COUNT(DISTINCT "Department") FROM mystaff.allstaff', engine)
totalDepts = queryDept.iloc[0][0]
#print(totalDepts)

queryEPD = pandas.read_sql_query('SELECT "Department", COUNT("LastName") FROM mystaff.allstaff GROUP BY "Department"', engine)
#print(queryEPD)

queryEPD.set_index("Department", inplace = True)

logEmp = queryEPD.loc["Logistics", "count"] #Number of employees in Logistics
mkEmp = queryEPD.loc["Marketing", "count"] #Number of employees in Marketing
slsEmp = queryEPD.loc["Sales", "count"] #Number of employees in Sales
itEmp = queryEPD.loc["IT", "count"] #Number of employees in IT

salaryHigh = queryAll['Salary'].max() #Highest salary
salaryLow = queryAll['Salary'].min() #Lowest salary
salalaryAvg = queryAll['Salary'].mean() #Average salary

#Joining the above information in a nice format - HTML
summary = [["Total number of employees", int(totalEmployees)],
           ["Employees in Logistics", int(logEmp)],
           ["Employees in Marketing", int(mkEmp)],
           ["Employees in Sales", int(slsEmp)],
           ["Employees in IT", int(itEmp)],
           ["Highest salary", int(salaryHigh)],
           ["Lowest salary", int(salaryLow)],
           ["Salary average", int(salalaryAvg)]]
           
summaryHtml = pandas.DataFrame(summary, columns = ["Stats", "Value"])

print(summaryHtml)

#Writing the summary report to HTML format
with open("D:\\GIT Repo\\myRepo\\Python_Course\\DataAnalysis\\summary.html", "w") as f:
    summaryHtml.to_html(f, index = False, justify = 'center')
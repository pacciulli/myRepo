"""
Spreadsheet automation with Python.

Project related to section 19 from Python course
"""

#Importing modules
from openpyxl import Workbook
from openpyxl.styles import *
from openpyxl.worksheet.table import Table, TableStyleInfo

#Open text file
file = open("D:\GIT Repo\myRepo\Python_Course\SpreadSheet\employees.txt", "r")

#Reading text file
data = []

for line in file.readlines():
    data.append(line.rstrip("\n").split(";"))

#Closing file
file.close()

#Creating spreadsheet and selecting working sheet
workingBook = Workbook()

xlsx_path = "D:\GIT Repo\myRepo\Python_Course\SpreadSheet\employees.xlsx"
workingBook.save(xlsx_path)

workingSheet = workingBook["Sheet"]
workingSheet.title = "Employees Data"
workingSheet = workingBook["Employees Data"]

#Appenind data from text file to working sheet
for row in data:
    workingSheet.append(row)
    
#Creating a table for working sheet
table = Table(displayName = "Table", ref = "A1:G11")
style = TableStyleInfo(name = "TableStyleLight5", showRowStripes = True, showColumnStripes = True)
table.TableStyleInfo = style

#Apply style to the table
workingSheet.add_table(table)

#Creating font style for big salaries
fontStyle = Font(color = 'FF0000', bold = True, italic = True)

#Applying font style to the cells that salary > 55000
for cellNo in range(2, 12):
    if int(workingSheet["G%s" % (cellNo)].value) > 55000:
        workingSheet["G%s" % (cellNo)].font = fontStyle

#Saving workbook
workingBook.save(xlsx_path)

#Closing workbook
workingBook.close()

print("Spreadsheet created")
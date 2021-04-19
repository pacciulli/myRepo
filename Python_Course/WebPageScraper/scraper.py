import pandas
import requests
from bs4 import BeautifulSoup

#Getting the webpage
numPages = int(input("\nNumber of pages to scrap: "))

products = []

for i in range(0,numPages):
    pageIndex = str(i+1)
    webpage = requests.get("https://www.amazon.com.br/gp/bestsellers/books/ref=zg_bs_pg_" + pageIndex + "?ie=UTF8&pg=" + pageIndex)
    
    #Loading the content
    content = webpage.content
    
    #Parsing the content
    result = BeautifulSoup(content, 'html.parser')
    
    #Identifying the products on the page by the div tag and the class name
    productsPerPage = result.find_all("li", {"class": "zg-item-immersion"})
    
    for product in productsPerPage:
        products.append(product)

#Creating a list for each of the desired piece of information
names = []
prices = []

#Iterating over the list of products and extracting the necessary info
for item in products:
    names.append(item.a.div.img["alt"])
    prices.append(item.find("span", {"class":"p13n-sc-price"}).string)

data = list(zip(names, prices))

#Creating the Pandas dataframe
d = pandas.DataFrame(data, columns = ['Name', 'Price'])

#Writing the dataframe to a new Excel file
try:
    d.to_excel("D:\\GIT Repo\\myRepo\\Python_Course\\WebPageScraper\\Products.xlsx")

except:
    print("\nSomething went wrong! Please check your code.")

else:
    print("\nWeb data successfully written to Excel.")

finally:
    print("\nQuitting the program. Bye!")

#End of program
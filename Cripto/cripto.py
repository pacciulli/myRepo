# -*- coding: utf-8 -*-
"""
Created on Fri May  4 19:45:42 2018

@author: meyer
"""

import requests
import json
import time
from datetime import datetime

def retornaCotacaoMercBit():
    url = "https://www.mercadobitcoin.net/api/BTC/ticker/"
    headers = {'Content-Type': 'application/json'}
    response = requests.request("GET", url, headers=headers)

    p = lambda:None
    p.__dict__ = json.loads(response.text)

    return float(p.ticker['last'])

    
def retornaCotacaoBitTrade():
    url = "https://api.bitcointrade.com.br/v1/public/BTC/ticker/"
    headers = {'Content-Type': 'application/json'}
    response = requests.request("GET", url, headers=headers)
    
    p = lambda:None
    p.__dict__ = json.loads(response.text)
    
    return float(p.data['last'])

def retornaCotacaoDollar():
    #url = "https://www.alphavantage.co/query?function=CURRENCY_EXCHANGE_RATE&from_currency=USD&to_currency=BRL&apikey=30PB9W5BQWRSC4SW"
    url = "https://api.bitvalor.com/v1/ticker.json"
    headers = {'Content-Type': 'application/json'}
    response = requests.request("GET", url, headers=headers)

    p = lambda:None
    p.__dict__ = json.loads(response.text)
    
    #data = json.loads(response.text)
    
    return float(p.rates['USDCBRL'])
    #return float(data['Realtime Currency Exchange Rate']['5. Exchange Rate'])

    
def retornaCotacaoBinance():
    url = "https://api.binance.com/api/v1/ticker/24hr"
    headers = {'Content-Type': 'application/json'}
    querystring = {"symbol":"BTCUSDT"}    
    response = requests.request("GET", url, headers=headers, params=querystring)
    
    p = lambda:None
    p.__dict__ = json.loads(response.text)
    
    return float(p.lastPrice)
    
def retornaCotacaoCoinsbank():
     url = "https://coinsbank.com/api/public/ticker?pair=BTCUSD"
     headers = {'Content-Type': 'application/json'}
     response = requests.request("GET", url, headers=headers)

     p = lambda:None
     p.__dict__ = json.loads(response.text)
   
     return float(p.data['last'])
     
while True:
    try:
      a = retornaCotacaoMercBit()
    except:
      a = 0
    
    try:
      b = retornaCotacaoBitTrade()
    except:
      b = 0
    try:
      c = retornaCotacaoDollar()
    except:
      c = 0
    try:
      d = retornaCotacaoBinance()
    except:
      d = 0
    try:
      f = retornaCotacaoCoinsbank()
    except:
      f = 0
      
    e = c*d
    g = c*f

    print(datetime.now().strftime('%d/%m/%Y %H:%M:%S'))
    #print('Mercdo Bitcoin: R$',a)
    #print('Bitcoin Trade: R$',b)
    #print('Binance: R$',e, ' Dolar: R$',c)
    #print('CoinsBank: R$', g)
    
    fh = open("dataBase.csv","a")
    stringToSave = [datetime.now().strftime('%d/%m/%Y %H:%M:%S')," ", str(a)," ", str(b)," ", str(e)," ", str(g) ," ", str(c),"\n"]
    fh.writelines(stringToSave)
    fh.close()

    #time.sleep(57)
    time.sleep(56)
    
# -*- coding: utf-8 -*-
"""
Created on Sat Jun 15  10:21:00 2019

@author: meyer
"""

import requests
import json
import time
from datetime import datetime

def getPrice(symbol):
    url = "https://api.binance.com/api/v3/ticker/price"
    headers = {'Content-Type': 'application/json'}
    querystring = {"symbol":symbol}    
    response = requests.request("GET", url, headers=headers, params=querystring)
    
    p = json.loads(response.text)
    
    return float(p['price'])
     
while True:
    try:
      iotausdt = getPrice('IOTAUSDT')
    except:
      iotausdt = 0

    try:
      iotabtc = getPrice('IOTABTC')
    except:
      iotabtc = 0

    try:
      btcusdt = getPrice('BTCUSDT')
    except:
      btcusdt = 0
  
    fh = open("data.csv","a")
    stringToSave = [datetime.now().strftime('%d/%m/%Y %H:%M:%S')," ", str(iotabtc)," ", str(iotausdt)," ", str(btcusdt), "\n"]
    fh.writelines(stringToSave)
    fh.close()

    print(datetime.now().strftime('%d/%m/%Y %H:%M:%S'))

    time.sleep(58)
        
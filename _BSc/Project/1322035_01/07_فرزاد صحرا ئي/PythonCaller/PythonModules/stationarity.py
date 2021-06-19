import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import TimeSeries as ts
from statsmodels.tsa.stattools import adfuller
def test_stationarity(timeseries):
    
    #Determing rolling statistics
    rolmean = timeseries.rolling( window=12).mean()
    #rolmean = pd.rolling_mean(timeseries, window=12)
    #rolstd = pd.rolling_std(timeseries, window=12)
    rolstd = timeseries.rolling( window=12).std()

    #Plot rolling statistics:
    # fig = plt.figure(figsize=(12, 8))
    # orig = plt.plot(timeseries, color='blue',label='Original')
    # mean = plt.plot(rolmean, color='red', label='Rolling Mean')
    # std = plt.plot(rolstd, color='black', label = 'Rolling Std')
    # plt.legend(loc='best')
    # plt.title('Rolling Mean & Standard Deviation')
    # plt.show()
    
    #Perform Dickey-Fuller test:
    #print ('Results of Dickey-Fuller Test:')
    dftest = adfuller(timeseries, autolag='AIC')
    dfoutput = pd.Series(dftest[0:4], index=['Test Statistic','p-value','#Lags Used','Number of Observations Used'])
    outData={0:dftest[0],1:dftest[1],2:dftest[2],3:dftest[3],4:dftest[4]}
    counter = 5
    for key,value in dftest[4].items():
        dfoutput['Critical Value (%s)'%key] = value
        outData[counter]=value
        counter = counter + 1
    print (dfoutput)
    return outData

def checkdatastationarity(data):
    keys = list(data.keys())
    df = ts.ReadData(data)
    keys.remove('Dates')
    result = dict()
    for key in keys:        
        df[key] = df[key].astype('float32')
        outdata =  test_stationarity(df[key])
        if outdata[0] > outdata[7]:
            result[key] = False
        else:
            result[key] = True
    return result
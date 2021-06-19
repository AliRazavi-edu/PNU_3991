import sys
import io
import linecache
import pandas as pd
import numpy as np
import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import datetime
import warnings
import itertools
from dateutil.relativedelta import relativedelta
import statsmodels.api as sm
import statsmodels.tsa.api as smt
from statsmodels.tsa.stattools import acf
from statsmodels.tsa.stattools import pacf
from statsmodels.tsa.seasonal import seasonal_decompose
from statsmodels.tsa.arima_model import ARIMA
from statsmodels.tsa.arima_model import ARMA
import json
import uuid

_forcastSteps = 0
_perdictcount = 0
_p = 0
_q = 0
_d = 0
_s = 0

def main(data,forcastSteps,perdictcount,p,q,d,s,cType):
    
    global _forcastSteps
    global _perdictcount
    global _p
    global _q
    global _d
    global _s
    _forcastSteps = forcastSteps
    _perdictcount = perdictcount
    _p = p
    _q = q
    _d = d
    _s = s

    df = ReadData(data)
    PrepareData(df)
    PredictResult   = None
    forecast   = None
    aic=None
    hqic=None   
    cType  = cType.lower()

    if cType=="sarima":
        PredictResult,f,aic,hqic = Sarima(df)
#        forecast = dict(zip(f.index.format(), f))
        forecast = f    

    if cType == "arima":
        PredictResult,f,aic,hqic = Arima(df)
        forecast =f

    if cType == "arma":
        PredictResult,f,aic,hqic = Arma(df)
        forecast =f

    return dict(zip(PredictResult.index.format(), PredictResult)),forecast,aic,hqic 
    

def ReadData(data):
    #try:
        df = pd.DataFrame.from_dict(data)
        return df

    #except:
    #    PrintException()

def PrepareData(df):
    #try:

        #df.index = pd.to_datetime(df.Date)
        df.index = df.Date
        df.index.name = None
        df.columns = ['date', 'riders']
        df.riders = df.riders.astype('float32')

    #except:
    #    PrintException()

def Sarima(df):
    #try:
        mod = sm.tsa.SARIMAX(df.riders, order=(_p, _d, _q), seasonal_order=(1, 1, 0, _s), enforce_stationarity=False)
        results = mod.fit()
		
        aic = results.aic
        hqic = results.hqic
		
        bound = df.shape[0] - 1
        lowerbond = bound - _perdictcount
        predict = results.predict(start = lowerbond + 1, end = bound, dynamic = False)
        forecats = results.forecast(_forcastSteps)
        data = {}
        counter =0
        for k in forecats:
            counter +=1
            data[counter] =k		
        return predict, data,aic,hqic		    

    #except:
        #PrintException()

def Arima(df):
    #try:
        mod = ARIMA(df.riders, order=(_p, _d, _q))
        results = mod.fit()
		
        aic = results.aic
        hqic = results.hqic		
		
        bound = df.shape[0] - 1
        lowerbond = bound - _perdictcount
        predict = results.predict(start = lowerbond + 1, end = bound, dynamic = False)
        forecats = results.forecast(_forcastSteps)
        data = {}
        counter =0
        for k in forecats[0]:
            counter +=1
            data[counter] =k
				       
        return predict, data,aic,hqic

    #except:
        #PrintException()
def Arma(df):
    #try:
        mod = ARMA(df.riders, order=(_p,_q))
        results = mod.fit()
		
        aic = results.aic
        hqic = results.hqic				
		
        bound = df.shape[0] - 1
        lowerbond = bound - _perdictcount
        predict = results.predict(start = lowerbond + 1, end = bound, dynamic = False)
        forecats = results.forecast(_forcastSteps)
        counter = 0
        data = {}
        for k in forecats[0]:
           counter +=1
           data[counter] =k
		   
        return predict, data,aic,hqic		           

        

    #except:
        #PrintException()


def PlotAcf(data):
    df = pd.DataFrame.from_dict(data)
    df.riders = df.riders.astype('float32')
    fig = plt.figure(figsize=(12,8))
    ax1 = fig.add_subplot(211)
    data = df['riders'].iloc[1:]
    lag = df.shape[0] - 2
    fig = sm.graphics.tsa.plot_acf(data, lags=lag,alpha=.65, ax=ax1,zero=False)
    ax2 = fig.add_subplot(212)
    lag = lag / 2
    fig = sm.graphics.tsa.plot_pacf(data, lags=lag,alpha=.65, ax=ax2,method='ywm',zero=False)
    fileName = str(uuid.uuid4()) + ".png"
    plt.savefig("PacfPlot/" + fileName)
    return fileName

def Decompose(data):
    df = pd.DataFrame.from_dict(data)
    df.riders = df.riders.astype('float32')
    decomposition = seasonal_decompose(df.riders, freq=12)
    return decomposition



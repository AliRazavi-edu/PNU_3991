import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
import datetime
import warnings
import itertools
from dateutil.relativedelta import relativedelta
#import seaborn as sns
import statsmodels.api as sm  
import statsmodels.tsa.api as smt
from statsmodels.tsa.stattools import acf  
from statsmodels.tsa.stattools import pacf
from statsmodels.tsa.seasonal import seasonal_decompose
import stationarity as stationarity
from statsmodels.tsa.arima_model import ARIMA
from statsmodels.tsa.arima_model import ARMA
import sys

def main(): 
    df = ReadData()
    ShowData(df)
    #SuggestLags(df)
    #return
    # Decompose(df)
    # MakeStationary(df)
    PlotAcf(df)
    results,PredictResult,forecats = BuildModel(df)
    # Predict(df,results)
    x =  dict(zip(PredictResult.index.format(), PredictResult))
    return x
def ReadData():
    df = pd.read_csv('data.csv',index_col=0,sep=",",skiprows=0)
    # df.index.name=None
    # df.reset_index(inplace=True)
    upperBound = df.shape[0] -1
    df.drop(df.index[upperBound], inplace=True)
    return df

def ShowData(df):
    upperBound = df.shape[0]
    # start = datetime.datetime.strptime("1973-01-01", "%Y-%m-%d")
    # date_list = [start + relativedelta(months=x) for x in range(0,upperBound)]
    # df['index'] =date_list
    # df.set_index(['index'], inplace=True)
    df.index = pd.to_datetime(df.index)
    df.index.name=None


    df.columns= ['riders']
    # df['riders'] = df.riders.apply(lambda x: int(x)*100)
    df.riders=df.riders.astype('float32')
    df.riders.plot(figsize=(12,8), title= 'Monthly Ridership', fontsize=14)
    # plt.savefig('month_ridership.png', bbox_inches='tight')
    plt.show()

def Decompose(df):
    decomposition = seasonal_decompose(df.riders, freq=12)  
    fig = plt.figure()  
    fig = decomposition.plot()  
    fig.set_size_inches(15, 8)
    plt.show()


def MakeStationary(df):

    # enhance in coding necessary 
    global key
    key = 'riders'
    outdata =  stationarity.test_stationarity(df[key])
    if outdata[0] > outdata[5]:
       print("non stationary")
    else:
        return

    key = 'riders_log'
    df[key]= df.riders.apply(lambda x: np.log(x))  
    outdata = stationarity.test_stationarity( df[key])
    if outdata[0] > outdata[5]:
       print("non stationary")
    else:
        return

    key = 'first_difference'
    df[key] = df.riders - df.riders.shift(1)  
    outdata = stationarity.test_stationarity( df[key].dropna(inplace=False))

    if outdata[0] > outdata[5]:
       print("non stationary")
    else:
        return
    key = 'log_first_difference'
    df[key] = df.riders_log - df.riders_log.shift(1)  
    outdata = stationarity.test_stationarity(df[key].dropna(inplace=False))

    if outdata[0] > outdata[5]:
       print("non stationary")
    else:
        return
    key = 'log_first_difference'
    df[key] = df.riders - df.riders.shift(12)  
    outdata = stationarity.test_stationarity( df[key].dropna(inplace=False))

    if outdata[0] > outdata[5]:
       print("non stationary")
    else:
        return

    key = 'log_seasonal_difference'
    df[key] = df.riders_log - df.riders_log.shift(12)  
    outdata = stationarity.test_stationarity(df[key].dropna(inplace=False))
    if outdata[0] > outdata[5]:
      print("non stationary")
    else:
      return

    key = 'seasonal_first_difference'
    df[key] = df.first_difference - df.first_difference.shift(12)  
    outdata = stationarity.test_stationarity(df[key].dropna(inplace=False))
    if outdata[0] > outdata[5]:
      print("non stationary")
    else:
      return

def PlotAcf(df):
    fig = plt.figure(figsize=(12,8))
    ax1 = fig.add_subplot(211)
    # key1 = key
    data = df['riders'].iloc[1:]

    fig = sm.graphics.tsa.plot_acf(data, lags=40, ax=ax1)
    ax2 = fig.add_subplot(212)
    fig = sm.graphics.tsa.plot_pacf(data, lags=30, ax=ax2)
    plt.show()
def SuggestLags(df):
    # Define the p, d and q parameters to take any value between 0 and 2
    p = d = q = range(0, 3)

    # Generate all different combinations of p, q and q triplets
    pdq = list(itertools.product(p, d, q))

    # Generate all different combinations of seasonal p, q and q triplets
    seasonal_pdq = [(x[0], x[1], x[2], 0) for x in list(itertools.product(p, d, q))]
    warnings.filterwarnings("ignore") # specify to ignore warning messages
    minAic = 10000
    result = None
    min_param_seasonal=None
    min_param=None
    for param in pdq:
        for param_seasonal in seasonal_pdq:
            try:
                mod = sm.tsa.statespace.SARIMAX(df.riders,
                                                order=param,
                                                seasonal_order=param_seasonal,
                                                enforce_stationarity=False,
                                                enforce_invertibility=False)

                results = mod.fit()

                print('ARIMA{}x{}12 - AIC:{}'.format(param, param_seasonal, results.aic))
                if(minAic > results.aic):
                   minAic = results.aic
                   min_param_seasonal = param_seasonal
                   min_param = param
            except Exception as e:
                print(e)
                continue

    print('ARIMA{}x{}12 - AIC:{}'.format(min_param, min_param_seasonal, minAic))
def BuildModel(df):
    # max_lag = 30
    # mdl = smt.AR(df.riders).fit(maxlag=max_lag, ic='aic', trend='nc')
    # est_order = smt.AR(df.riders).select_order(maxlag=max_lag, ic='aic', trend='nc')

    # print('best estimated lag order = {}'.format(est_order))
    # print (mdl.params)

    
   
    mod = sm.tsa.SARIMAX( df.riders, order=(1,1,0), seasonal_order=(1,1,0,12),enforce_stationarity=False)
    #mod = ARMA( df.riders, order=(1,3))
    results = mod.fit()
    print (results.summary())

    
    bound = df.shape[0] - 1
    predict = results.predict(start=bound - 12, end=bound, dynamic=False)
   
    df['forecast']  = predict
    df[['riders', 'forecast']].plot(figsize=(12, 8))

# Check Forcasting with current Data 
    npredict =10
    fig, ax = plt.subplots(figsize=(12,6))
    npre = 12
    ax.set(title='Ridership', xlabel='Date', ylabel='Riders')
    ax.plot(df.index[-npredict-npre+1:], df.ix[-npredict-npre+1:, 'riders'], 'o', label='Observed')
    ax.plot(df.index[-npredict-npre+1:], df.ix[-npredict-npre+1:, 'forecast'], 'g', label='Dynamic forecast')
    legend = ax.legend(loc='lower right')
    legend.get_frame().set_facecolor('w')
    plt.show()
    forecats = results.forecast(10) 

    # plt.plot(forcats)
    
    return results,predict,forecats

# def Predict(df,results):
  

# For Cast Data 
    # start = datetime.datetime.strptime("2018-03-20", "%Y-%m-%d")
    # date_list = [start + relativedelta(months=x) for x in range(0,12)]
    # future = pd.DataFrame(index=date_list, columns= df.columns)
    # df = pd.concat([df, future])
  
    # plt.show()


main()
# data  = pd.read_csv("Report.csv", header=None)
# # a, b, c=stattools.acf(data,qstat=True)
# # print(a)
# # print(b)
# # print(c)

# plt.figure(figsize=(16,8))
# plt.title('ACF and PACF')
# plt.subplot(211)
# plot.plot_acf(data, ax = plt.gca(),lags=30)
# plt.show()
# data.index =pd.date_range(start='1/1/2016', periods=65, freq='M')
# print(data.index)
# data.columns = ['4Seporde']
# res = sm.tsa.ARMA(data, (18, 0)).fit()
# fig, ax = plt.subplots()
# ax = data.loc['2016-01-31':].plot(ax=ax)
# fig = res.plot_predict('2021-04-30', '2022-5-31', dynamic=True, ax=ax, plot_insample=False)
# plt.show()
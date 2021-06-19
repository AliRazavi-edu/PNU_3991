import numpy as np
import pandas
import statsmodels.api as sm
from statsmodels.tsa.api import VAR
import json
import pandas as pd
from statsmodels.compat.python import lrange

def forecast(data,lag,forcastStep):
    #with open('varModel.json') as f:
    #    data = json.load(f)

    mdata = prepareData(data)
    model = VAR(mdata)


    results = model.fit(lag)
    
    #lag_order = results.k_ar
    createForcastModel(results,forcastStep)

    fevd = results.fevd(forcastStep)
    
    fevModel = []
       

    for val in range(len(fevd.names)):
        for i in range(fevd.periods):
            for j in range(len(fevd.names)):
                fevItem = {}
                fevItem["ind"] = val
                fevItem["period"] = i
                fevItem["compindex"] = j
                fevItem["val"] = fevd.decomp[val][i][j]
                fevModel.append(fevItem)
    
    #irf = results.irf(forcastStep)
    #print(irf.orth_irfs)
    #print(irf.svar_irfs)
    #print(irf.irfs)
    
    mid, lower, upper = createForcastModel(results,forcastStep)
    print(fevd.summary())
    return mid, lower, upper,fevModel


    # fig = results.plot_forecast(forcastStep)
    # fig.savefig('forcast.png')
    # fig.show()
    

    # irf = results.irf(forcastStep)
    # fig =  irf.plot(orth=False)
    # fig.savefig('figIr.png')
    # fig.show()
        
    # fevd = results.fevd(forcastStep)
 

def prepareData(data):
    df =pd.read_json(json.dumps(data), orient='list')  #pd.read_json('varModel.json')
    df.index = pd.to_datetime(df.Dates)
    mdata = df[[c for c in df.columns if c not in {'Dates'}]]
    mdata.index = pd.to_datetime(df.Dates)
    return mdata

def createForcastModel(result,steps):
    alpha=0.05
    mid, lower, upper = result.forecast_interval(result.y[-result.k_ar:], steps,
                                                   alpha=alpha)
    return mid, lower, upper

def fit(data,maxlag):
    #with open('varModel.json') as f:
    #   data = json.load(f)
    mdata  = prepareData(data)
    equation = dict()
    equation["aic"] = []
    equation["BIC"]  =[]
    equation["hqic"] = []
    equation["min"] = []
    model = VAR(mdata)
    for x in range(0, maxlag):
        fitedModel  = model.fit(x + 1)
        equation["aic"].append(fitedModel.aic)  
        equation["BIC"].append(fitedModel.bic) 
        equation["hqic"].append(fitedModel.hqic)  
   
    minLag = model.fit(maxlags=maxlag,ic='bic')
    equation["min"].append(minLag.aic)  
    equation["min"].append(minLag.bic)  
    equation["min"].append(minLag.hqic)  

    return equation
if __name__ == "__main__" :
    with open('varModel.json') as f:
        data = json.load(f)
    forecast(data['Data'],2,12)

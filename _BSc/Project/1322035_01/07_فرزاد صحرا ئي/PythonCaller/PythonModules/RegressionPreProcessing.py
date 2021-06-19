import numpy as np
import pandas as pd
import json


def GetNewData(data,lag,functionType):
    dataArray = np.reshape(np.asarray(data),(len(data)))
    
    df = pd.DataFrame({'col':dataArray})
    
    switcher = {
        1:__GetSimpleDiff, # get simple differ with lag
        2:__GetLn,
        3:__GetGrowth
    }
    
    func = switcher.get(functionType,"Invalid Fuction Type !")
    
    out = func(df,lag) 
    
    out=np.reshape(out,out.shape[0])
    out[out == np.NINF] = 0
    out[out == np.inf] = 0
    out=np.nan_to_num(out)

    return out
    
def __GetSimpleDiff(data,lag):
    res = data - data.shift(lag)    
    return res.to_numpy()

def __GetLn(data,lag):
    res = data.apply(lambda x: np.log(x))#.replace(np.inf, 0, inplace=True)
    return res.to_numpy()

def __GetGrowth(data,lag):
    diff = __GetSimpleDiff (data,lag)
    diff=np.roll(diff,-lag)    
    res=diff/data  
    res=np.roll(res,lag)  
    return res

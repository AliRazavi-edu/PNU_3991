import regression as reg
import IndicatorPredictionMPL as mlp
import var
import stationarity
import sys
from flask import Flask, jsonify, request
import TimeSeries as ts
import json
import linecache
import RegressionPreProcessing as rpp
app = Flask(__name__)


@app.route('/')
def hello():
    return jsonify({"Message": "App Is running"})


@app.route('/ForeCast', methods=["Post"])
def ForeCast():
    # return jsonify({'ip': request.remote_addr}), 200
    try:

        perdictcount, forcastSteps, p, q, d, s, cType = GetParameters(request)
        data = json.loads(json.dumps(request.json['Data']))        
				  
        PredictResult,forecast,aic,hqic = ts.main(data, forcastSteps, perdictcount, p, q, d, s, cType)			
        
        result = {'PredictResult': PredictResult,
        'forecast': forecast,
        'aic': aic,
        'hqic':hqic,
        'Exception': ''}
		
        return jsonify(result)

    except:
        return jsonify({'PredictResult': '', 'forecast': '', 'Exception': GetException()})


def GetParameters(request):
    p = q = d = s = 0
    cType = "sarima"
    perdictcount = request.json['PredictCount']
    forcastSteps = request.json['ForcastStep']
    if 'p' in request.json:
        p = request.json['p']

    if 'q' in request.json:
        q = request.json['q']

    if 'd' in request.json:
        d = request.json['d']

    if 's' in request.json:
        s = request.json['s']
    if 'type' in request.json:
        cType = request.json['type']

    return perdictcount, forcastSteps, p, q, d, s, cType


@app.route('/Pacf', methods=["Post"])
def GetPlots():
    try:
        data = json.loads(json.dumps(request.json['Data']))
        fileName = ts.PlotAcf(data)
        return jsonify({"fileName": fileName})
    except:
        return jsonify({'PredictResult': '', 'forecast': '', 'Exception': GetException()})




@app.route('/decompose', methods=["Post"])
def GetDecompose():
    try:
        data = json.loads(json.dumps(request.json['Data']))
        decomposition = ts.Decompose(data)
        result = {"trend": dict(zip(decomposition.trend.index.format(), decomposition.trend.fillna('null'))), "resid": dict(zip(decomposition.resid.index.format(), decomposition.resid.fillna('null'))), "seasonal": dict(zip(decomposition.seasonal.index.format(), decomposition.seasonal.fillna('null'))), 'Exception': ''}
        return jsonify(result)
    except:
        return jsonify({'PredictResult': '', 'forecast': '', 'Exception': GetException()})
    


@app.route('/regression', methods=["Post"])
def GetRegression():
    data = json.loads(json.dumps(request.json['Data']))
    regressionResult = reg.reg_m(data["y"][0], data["x"])
    # regressionResult = reg.reg_m(reg.y,reg.x)
    print(regressionResult.params)
	
    
    coef = list(reversed(regressionResult.params.tolist()))
    temp = coef[0]
    coef = coef[1:]
    coef.append(temp)

    stdError = list(reversed(regressionResult.bse.tolist()))
    temp = stdError[0]
    stdError = stdError[1:]
    stdError.append(temp)
	
    t = list(reversed(regressionResult.tvalues.tolist()))
    temp = t[0]
    t = t[1:]
    t.append(temp)

    pvalues = list(reversed(regressionResult.pvalues.tolist()))
    temp = pvalues[0]
    pvalues = pvalues[1:]
    pvalues.append(temp)	
	
    return jsonify({"coef": coef,
                    "stdError": stdError,
                    "t": t,
                    "pvalues": pvalues,
                    "bic": regressionResult.bic,
                    "aic": regressionResult.aic})
	
	#return jsonify({"coef": regressionResult.params.tolist(),
                    #"stdError": regressionResult.bse.tolist(),
                    #"t": regressionResult.tvalues.tolist(),
                    #"pvalues": regressionResult.pvalues.tolist(),
                    #"bic": regressionResult.bic,
                    #"aic": regressionResult.aic})
   # return jsonify(regressionResult)


@app.route('/fitVar', methods=["Post"])
def fitVar():
    try:
        maxLag = request.json['maxLag']
        data = json.loads(json.dumps(request.json['Data']))
        result = var.fit(data, maxLag)
        return jsonify(result)

    except:
        return jsonify({'Exception': GetException()})


@app.route('/varForecast', methods=["Post"])
def varForecast():
    try:
        lag = request.json['lag']
        forcastStep = request.json['forcastStep']
        data = json.loads(json.dumps(request.json['Data']))
        mid, lower, upper, fevModel = var.forecast(data, lag, forcastStep)

        return jsonify({"mid": mid.tolist(), "lower": lower.tolist(), "upper": upper.tolist(), "fevd": fevModel})

    except:
        return jsonify({'Exception': GetException()})

@app.route('/checkStationary', methods=["Post"])
def CheckStationary():
    try:
        data = json.loads(json.dumps(request.json['Data']))
        result = stationarity.checkdatastationarity(data)
        return jsonify(result)
    except:
        return jsonify({'Exception': GetException()})


@app.route('/mlpForecast', methods=["Post"])
def mlpForecast():
    try:
        data = json.loads(json.dumps(request.json['data']))
        predictionIntervalCount = request.json['predictionIntervalCount']
        neuronCount = request.json['neuronCount']
        windowCount = request.json['windowCount']
        epoch = request.json['epoch']
        bachSize = request.json['bachSize']
        transformFunction = request.json['transformFunction']
        minMseTest = request.json['minMseTest']
        minMseTrain = request.json['minMseTrain']
		
		# call forecast function
        actualActivityTrend, trainTrend, testTrend, evaluationTrend, predictTrend, mseTest, mseTrain, mseEvaluation = mlp.forecast(data, predictionIntervalCount,neuronCount,windowCount,epoch,bachSize,transformFunction,minMseTest,minMseTrain)

        return jsonify({"actualActivityTrend": actualActivityTrend, "trainTrend": trainTrend, "testTrend": testTrend, "evaluationTrend": evaluationTrend,"predictTrend": predictTrend,"mseTest": str(mseTest),"mseTrain": str(mseTrain),"mseEvaluation": str(mseEvaluation)})

    except:
        return jsonify({'Exception': GetException()})    
    
@app.route('/regressionPreProcessing', methods=["Post"])
def regressionPreProcessing():
    try:
        data = json.loads(json.dumps(request.json['data']))
        lag = request.json['lag']
        funcType = request.json['funcType']
		
		# call PrePocessing function
        pre_processing_out = rpp.GetNewData(data,lag,funcType)  

        return jsonify({"data": pre_processing_out.tolist()})

    except:
        return jsonify({'Exception': GetException()})    


def GetException():
    exc_type, exc_obj, tb = sys.exc_info()
    f = tb.tb_frame
    lineno = tb.tb_lineno
    filename = f.f_code.co_filename
    linecache.checkcache(filename)
    line = linecache.getline(filename, lineno, f.f_globals)
    errorText = 'EXCEPTION IN ({}, LINE {} "{}"): {}'.format(
        filename, lineno, line.strip(), exc_obj)
    return errorText


if __name__ == '__main__':
    app.run(debug=True)
    # debug=True

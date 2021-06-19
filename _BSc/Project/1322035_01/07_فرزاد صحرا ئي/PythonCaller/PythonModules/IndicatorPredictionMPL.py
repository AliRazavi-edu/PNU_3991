# Import
import tensorflow as tf
import numpy as np
import pandas as pd
from sklearn.preprocessing import MinMaxScaler
import matplotlib.pyplot as plt
from scipy.ndimage.interpolation import shift
import json

    
def forecast(data,
             predict_intervalCounts,
             n_neurons_1,
             input_window_history_count,
             epochs,
             batch_size,
             transformFunction,
             threshold_mse_test,
             threshold_mse_train):

    data = np.reshape(np.asarray(data),(len(data),1))
	
    output_window_history = 0
    
    data_train_perception = 0.8
    data_test_perception = 0.1
    data_validation_perception = 0.1

    sigma = 1

    ### default settings
    #threshold_mse_test = 0.0003
    #threshold_mse_train = 0.0001
    #
    #predict_intervalCounts = 180
    #
    #batch_size =256
    #n_neurons_1 = 2048
    #epochs = 1

    ### Global variables
    prediction_result=[]
    mse_train = []
    mse_test = []


    ### Import data from sample file
    #data = pd.read_csv('Data/misData_4131_Daily_10_4Indicators.csv')

    ### Dimensions of dataset 
    #
    #
    n = data.shape[0]
    p = data.shape[1]
    
    ### min and max of original data
    maxValue = data.max()
    minValue = data.min()

    ### Make data a np.array
    #data = data.values

    ### Training, test and validation data
    train_start = 0
    train_end = int(np.floor(data_train_perception * n))
    test_start = train_end
    test_end = test_start + int(np.floor(data_test_perception * n))
    validation_start = test_end 
    validation_end = n

    ### normalize data between [-1,1]
    scaler = MinMaxScaler(feature_range=(-1, 1))
    scaler.fit(data)
    normalized_data = scaler.transform(data)


    ### Build dataset for Train and Test in X and Y format
    X_train, y_train = create_dataset(normalized_data, train_start, train_end,
                                            input_window_history_count,
                                            output_window_history)
    X_test, y_test = create_dataset(normalized_data, test_start, test_end,
                                        input_window_history_count,
                                        output_window_history)
    X_validation, y_validation = create_dataset(normalized_data, validation_start, validation_end,
                                        input_window_history_count,
                                        output_window_history)                                        



    ### Number of stocks in training data
    n_stocks = X_train.shape[1]

    ### Session
    net = tf.InteractiveSession()

    ### Placeholder
    X = tf.placeholder(dtype=tf.float32, shape=[None, n_stocks])
    Y = tf.placeholder(dtype=tf.float32, shape=[None])

    ### Initializers
    weight_initializer = tf.variance_scaling_initializer(mode="fan_avg", distribution="uniform", scale=sigma)
    bias_initializer = tf.zeros_initializer()

    ### Hidden weights
    W_hidden_1 = tf.Variable(weight_initializer([n_stocks, n_neurons_1]))
    bias_hidden_1 = tf.Variable(bias_initializer([n_neurons_1]))

    ### Output weights
    W_out = tf.Variable(weight_initializer([n_neurons_1, 1]))
    bias_out = tf.Variable(bias_initializer([1]))

    ### Hidden layer
    hidden_1 = tf.nn.relu(tf.add(tf.matmul(X, W_hidden_1), bias_hidden_1))

    ### Output layer (transpose!)
    out = tf.transpose(tf.add(tf.matmul(hidden_1, W_out), bias_out))

    ### Cost function
    mse = tf.reduce_mean(tf.squared_difference(out, Y))

    ### Optimizer
    opt = tf.train.AdamOptimizer(0.01).minimize(mse)

    ### Init
    net.run(tf.global_variables_initializer())

    ### Start Training Process     
    bach_count = len(y_train) // batch_size
    for e in range(epochs):
        
        # Shuffle training data
        # shuffle_indices = np.random.permutation(np.arange(len(y_train)))
        # X_train_shuffeled = X_train[shuffle_indices]
        # y_train_shuffeled = y_train[shuffle_indices]

        bach_shuffle_indices = np.random.permutation(np.arange(bach_count))
        for i in range(0, bach_count):
            start = bach_shuffle_indices[i] * batch_size
            batch_x = X_train[start:start + batch_size]
            batch_y = y_train[start:start + batch_size]
            ### Run optimizer with batch
            net.run(opt, feed_dict={X: batch_x, Y: batch_y})


        ### check mseTest and mseTrain for exit training Process
        mse_train.append(net.run(mse, feed_dict={X: X_train, Y: y_train}))
        mse_test.append(net.run(mse, feed_dict={X: X_test, Y: y_test}))
        print('MSE Train: ', mse_train[-1])
        print('MSE Test : ', mse_test[-1])
        if (np.average(mse_train) <= threshold_mse_train or np.average(mse_test) <= threshold_mse_test):
            break

       
    ### Pridict
    X_predict = X_validation[:1]
    X_predict[0] = X_validation[-1]

    for i in range(0,predict_intervalCounts):
        current_prediction = net.run(out, feed_dict={X: X_predict})
        prediction_result.append(current_prediction[0,0])
        shift(X_predict[0],-1,output=X_predict[0] ,cval=current_prediction[0][0])

    prediction_result = np.array(prediction_result)
    
   

    ### Prepare result
    train_final_result = net.run(out, feed_dict={X: X_train})
    test_result = net.run(out, feed_dict={X: X_test})
    validation_result = net.run(out, feed_dict={X: X_validation})

    ### get final mse
    result_mse_train= net.run(mse, feed_dict={X: X_train, Y: y_train})
    result_mse_test = net.run(mse, feed_dict={X: X_test, Y: y_test})
    result_mse_validation = net.run(mse, feed_dict={X: X_validation, Y: y_validation})

    all_data = np.concatenate((y_train,y_test,y_validation), axis =0)


    ### denormalize data
    scaler = MinMaxScaler(feature_range=(minValue, maxValue))

    # temp_all_data = np.reshape(all_data,(all_data.shape[0],1))
    # scaler.fit(temp_all_data)
    # result_all_data = scaler.transform(temp_all_data)


    temp_all_data = data
    scaler.fit(temp_all_data)
    result_all_data = scaler.transform(temp_all_data)

    temp_train_final_data = np.transpose(train_final_result)
    scaler.fit(temp_train_final_data)
    result_train_final_data = scaler.transform(temp_train_final_data)

    temp_test_final_data = np.reshape(y_test,(y_test.shape[0],1))
    scaler.fit(temp_test_final_data)
    result_test_final_data = scaler.transform(temp_test_final_data)

    temp_validation_final_data = np.reshape(y_validation,(y_validation.shape[0],1))
    scaler.fit(temp_validation_final_data)
    result_validation_final_data = scaler.transform(temp_validation_final_data)

    temp_prediction_final_data = np.reshape(prediction_result,(prediction_result.shape[0],1)) 
    scaler.fit(temp_prediction_final_data)
    result_prediction_final_data = scaler.transform(temp_prediction_final_data)


    result_all_data = np.reshape(result_all_data,(result_all_data.shape[0])).tolist()
    result_train_final_data = np.reshape(result_train_final_data,(result_train_final_data.shape[0])).tolist()
    result_test_final_data1 = np.reshape(result_test_final_data,(result_test_final_data.shape[0])).tolist(),
    result_validation_final_data = np.reshape(result_validation_final_data,(result_validation_final_data.shape[0])).tolist()
    result_prediction_final_data = np.reshape(result_prediction_final_data,(result_prediction_final_data.shape[0])).tolist()

    return result_all_data, result_train_final_data, np.reshape(result_test_final_data,(result_test_final_data.shape[0])).tolist(), result_validation_final_data, result_prediction_final_data,result_mse_train, result_mse_test, result_mse_validation


def create_dataset(dataset, start_index, end_index, history_size, target_size):
  data = []
  labels = []

  start_index = start_index + history_size
  if end_index is None:
    end_index = len(dataset) - target_size

  for i in range(start_index, end_index):
    indices = range(i-history_size, i)
    # Reshape data from (history_size,) to (history_size, 1)
    data.append(np.reshape(dataset[indices], (history_size, 1)))
    labels.append(dataset[i+target_size])

  data_Array=np.array(data)
  lable_Array=np.array(labels)

  return np.reshape(data_Array,(data_Array.shape[0],data_Array.shape[1])) , np.reshape(lable_Array,lable_Array.shape[0])

# if __name__=="__main__":

#   with open('JsonDataForNN.txt') as f:
#     string = f.read()
  
#   jsonData = json.loads(string)
#   print (pd.DataFrame(jsonData))

#   data = jsonData['data']
#   predictionIntervalCount = jsonData['predictionIntervalCount']
#   neuronCount = jsonData['neuronCount']
#   windowCount = jsonData['windowCount']
#   epoch = jsonData['epoch']
#   bachSize = jsonData['bachSize']
#   transformFunction = jsonData['transformFunction']
#   minMseTest = jsonData['minMseTest']
#   minMseTrain = jsonData['minMseTrain']

# # call forecast function
#   actualActivityTrend, trainTrend, testTrain, evaluationTrend, predictTrend, mseTest, mseTrain, mseEvaluation = forecast(data, predictionIntervalCount,neuronCount,windowCount,epoch,bachSize,transformFunction,minMseTest,minMseTrain)


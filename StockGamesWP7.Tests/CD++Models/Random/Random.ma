[Top]
components : Random@RandomType 
out : OutTime OutStockPrice 
in : InTime InStockPrice 
Link : InTime time_hour@Random
Link : InStockPrice stock_price@Random
Link : new_time_hour@Random OutTime
Link : new_stock_price@Random OutStockPrice

[Random]

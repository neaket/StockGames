[Top]
components : BrownianMotion@BrownianMotionType 
out : OutStockIndex OutStockPrice OutTime
in : InStockIndex InTime
Link : InTime time_hour@BrownianMotion
Link : InStockIndex stock_index@BrownianMotion
Link : relayed_stock_index@BrownianMotion OutStockIndex
Link : new_stock_price@BrownianMotion OutStockPrice
Link : new_time_hour@BrownianMotion OutTime

[BrownianMotion]

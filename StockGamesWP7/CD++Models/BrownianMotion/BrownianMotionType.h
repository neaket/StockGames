#ifndef __BrownianMotionType_H
#define __BrownianMotionType_H

#include <list>
#include "stdlib.h"
#include "atomic.h"

//Random Model Class

class BrownianMotionType : public Atomic
{
public:
        //Constructor
        BrownianMotionType( const string &name = "BrownianMotion" );

        virtual string className() const ;

protected:
        Model &initFunction();
        Model &externalFunction( const ExternalMessage & );
        Model &internalFunction( const InternalMessage & );
        Model &outputFunction( const CollectMessage & );

private:
        const Port &stock_index;
        const Port &time_hour;

        Port &relayed_stock_index;
        Port &new_time_hour;
        Port &new_stock_price;

        Time time;

        Value outIndex;
        Value outPrice;
        Value outTime;
};

inline
string BrownianMotionType::className() const
{
        return "BrownianMotion";
}

#endif

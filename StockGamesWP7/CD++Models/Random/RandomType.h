#ifndef __RandomType_H
#define __RandomType_H

#include <list>
#include "stdlib.h"
#include "atomic.h"

//Random Model Class

class RandomType : public Atomic
{
public:
	//Constructor
	RandomType( const string &name = "RandomType" );

	virtual string className() const ;

protected:
	Model &initFunction();
	Model &externalFunction( const ExternalMessage & );
	Model &internalFunction( const InternalMessage & );
	Model &outputFunction( const CollectMessage & );

private:

	const Port &time_hour;
	const Port &stock_price;

	Port &new_time_hour;
	Port &new_stock_price;

	Time time;

	Value outTime;

	Value stockPrice;
	Value outPrice;
};

inline
string RandomType::className() const
{
	return "RandomType";
}

#endif

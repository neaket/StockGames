/*******************************************************************
*
*  DESCRIPTION: Sawtooth Stock Price Model Header File
*
*  AUTHOR: Andrew Jeffery
*
*  EMAIL: jeffery.andrew@gmail.com
*
*  DATE: 31 October 2012
*
*******************************************************************/

#ifndef __SawtoothType_H
#define __SawtoothType_H

#include <list>
#include "atomic.h"     // class Atomic

// Sawtooth Model class

class SawtoothType : public Atomic
{
public:
	// Constructor
	SawtoothType( const string &name = "SawtoothType" );

	virtual string className() const ;
protected:
	Model &initFunction();
	Model &externalFunction( const ExternalMessage & );
	Model &internalFunction( const InternalMessage & );
	Model &outputFunction( const CollectMessage & );

private:

	// input ports
	const Port &time_hours;
	const Port &stock_price;

	// output port
	Port &new_stock_price;
	Port &new_time_hours;

	// value of the time variable
	Time time;

	// value of the time variable converted for output.
	Value outTime;

	// value of the price variable
	Value stockPrice;
	Value outPrice;

};	// class SawtoothType

// ** inline ** //
inline
string SawtoothType::className() const
{
	return "SawtoothType" ;
}

#endif   //__SawtoothType_H

/*******************************************************************
*
*  DESCRIPTION: Sawtooth Stock Atomic Model
*
*  This is the implementation of the price of a stock.  Its purpose
*  is to increment or decrement the stock price by one unit based
*  on whether the current price is an even or odd value.  The
*  model has two inputs and two outputs defined as follows:
*
*  <time_hours>: This input provides the hour of time.
*  It is an integer whose value should be between 0 and
*  23.  However this rule is not enforced by the model.
*
*  <stock_price>: This input provides the current value fo the
*  stock. This value is a decimal with two decimal positions since it
*  represents a dollar value. The value should be greater then of equal
*  to zero, this rule is not enforced by the model.
*
*  <new_stock_price>: This output provides the new stock price by adding
*  or subtracting one unit from the input value.  It is a variable of
*  type double.
*
*  <new_time_hours>:
*
*  AUTHOR: Andrew Jeffery
*
*  EMAIL: jeffery.andrew@gmail.com
*
*  DATE: 31 October 2012
*
*******************************************************************/

/** include files **/
#include "SawtoothType.h"  // class TimeAdder
#include "message.h"    // class ExternalMessage, InternalMessage
#include "mainsimu.h"   // MainSimulator::Instance().getParameter( ... )
#include "distri.h"        // class Distribution

/** public functions **/

/*******************************************************************
* Function Name: TimeAdder constructor
* Description: This routine constructs the TimeAdder model
********************************************************************/
SawtoothType::SawtoothType( const string &name )
: Atomic( name )
, time_hours( addInputPort( "time_hours" ) )
, stock_price( addInputPort( "stock_price" ) )
, new_time_hours( addOutputPort( "new_time_hours" ) )
, new_stock_price( addOutputPort( "new_stock_price"))
{
}

/*******************************************************************
* Function Name: SawtoothType init function
* Description: This routine is invoked when simulation starts.
* This method does nothing.
********************************************************************/
Model &SawtoothType::initFunction()
{
    passivate();
	return *this ;
}

/*******************************************************************
* Function Name: externalFunction
* Description: This method handles external events coming from any
* of the two input ports.
********************************************************************/
Model &SawtoothType::externalFunction( const ExternalMessage &msg )
{
	if( msg.port() == time_hours )
	   {
	   this->time.hours( msg.value() );
	   }
	if( msg.port() == stock_price)
		{
		this->stockPrice = msg.value() ;
		}

	holdIn( passive, Time::Zero ); //TODO do we need this?

	return *this;
}

/*******************************************************************
* Function Name: internalFunction
* Description: The internal function simply increments the stock price if
* the current price is even, and decrement it if the price is odd. As well
* the time is increment by one hour for the model
********************************************************************/
Model &SawtoothType::internalFunction( const InternalMessage & )
{
	passivate();
	return *this ;

}

/*******************************************************************
* Function Name: outputFunction
* Description: This routines outputs the time to the <time_of_day>
* port.
********************************************************************/
Model &SawtoothType::outputFunction( const CollectMessage &msg )
{

	if( ( ((int)(stockPrice * 100))%2) == 0 )   //True when the number is even
		{
		outPrice = stockPrice + 1;
		}
	else
	{
		outPrice = stockPrice - 1;
	}

	this->time.hours(this->time.hours() + 1);

	// format the time for output.
	this->outTime = this->time.hours();

	sendOutput( msg.time(), new_time_hours, this->outTime );
	sendOutput( msg.time(), new_stock_price, this->outPrice);
	return *this ;
}

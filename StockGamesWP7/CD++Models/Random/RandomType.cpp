/*******************************************************************
*
*  DESCRIPTION: Random Stock Price Model
*
*
*  AUTHOR: Jonathon Panke
*
*  EMAIL: laxaues@gmail.com
*
*  DATE: 14 January 2013
*
*******************************************************************/

/** include files **/
#include "RandomType.h"
#include "message.h"    // class ExternalMessage, InternalMessage
#include "mainsimu.h"   // MainSimulator::Instance().getParameter( ... )
#include "distri.h"        // class Distribution

/** public functions **/

RandomType::RandomType( const string &name )
: Atomic( name )
, time_hour( addInputPort( "time_hour" ) )
, stock_price( addInputPort( "stock_price" ) )
, new_time_hour( addOutputPort( "new_time_hour" ) )
, new_stock_price( addOutputPort( "new_stock_price"))
{
}

Model &RandomType::initFunction()
{
	return *this ;
}

Model &RandomType::externalFunction( const ExternalMessage &msg )
{
	if( msg.port() == time_hour )
	   {
	   this->time.hours( msg.value() );
	   }
	if( msg.port() == stock_price)
		{
		this->stockPrice = msg.value() ;
		}

	holdIn( passive, Time::Zero );

	return *this;
}

Model &RandomType::internalFunction( const InternalMessage & )
{
	passivate();
	return *this ;

}

Model &RandomType::outputFunction( const CollectMessage &msg )
{

	int number_hours = 0;
	while( number_hours != 24 )
		{
		outPrice = 	(int)stockPrice + (rand() % 21) - 10;
		if( outPrice < 0) { outPrice = 0; }
		stockPrice = outPrice;
		number_hours++;

		this->time.hours(this->time.hours() + 1);
		this->outTime = this->time.hours();

		sendOutput( msg.time(), new_time_hour, this->outTime );
		sendOutput( msg.time(), new_stock_price, this->outPrice);
		}

	return *this ;
}

/*******************************************************************
*
*  DESCRIPTION: BrownianMotion Stock Price Model
*
*  This simulation model was created in the attempt to simulate a 
*  stock market environment. This model does this by utilizing
*  deterministic Brownian motion. The model is based on the equation
*  dS/dT = rS.
*
*  This model also simulates 24 hours of data per stock index that 
*  is given.
*
*  AUTHOR: Jonathon Panke
*
*  DATE: 20 February 2013
*
*******************************************************************/

/** include files **/
#include "BrownianMotionType.h"
#include "message.h"    // class ExternalMessage, InternalMessage
#include "mainsimu.h"   // MainSimulator::Instance().getParameter( ... )
#include "distri.h"        // class Distribution

/** public functions **/

BrownianMotionType::BrownianMotionType( const string &name )
: Atomic( name )
, stock_index( addInputPort ( "stock_index" ) )
, time_hour( addInputPort( "time_hour" ) )
, relayed_stock_index( addOutputPort( "relayed_stock_index" ) )
, new_time_hour( addOutputPort( "new_time_hour" ) )
, new_stock_price( addOutputPort( "new_stock_price"))
{
}

Model &BrownianMotionType::initFunction()
{
    return *this ;
}

Model &BrownianMotionType::externalFunction( const ExternalMessage &msg )
{
    if( msg.port() == time_hour )
    {
        this->time.hours( msg.value() );
    }
    if( msg.port() == stock_index)
    {
        this->outIndex = msg.value() ;
    }
    holdIn( passive, Time::Zero );
    return *this;
}

Model &BrownianMotionType::internalFunction( const InternalMessage & )
{
    passivate();
    return *this ;
}

Model &BrownianMotionType::outputFunction( const CollectMessage &msg )
{

    sendOutput( msg.time(), relayed_stock_index, this->outIndex );

    int number_hours = 0;
    double stockPrice = (((0)+ ( (double)rand() / RAND_MAX) )* 150) + 1;

    while( number_hours != 24 )
    {
        double rand_norm = (((-1) + ( (double)rand() / RAND_MAX) )* 2) + 1;
        double offset = stockPrice * rand_norm * 0.04166666667;
        outPrice = stockPrice - offset;
        stockPrice = outPrice;
        number_hours++;

        //this->time.hours(this->time.hours() + 1);
        //this->outTime = this->time.hours()

		sendOutput( msg.time() , new_stock_price, this->outPrice);
    }

    number_hours = 0;

    return *this ;
}

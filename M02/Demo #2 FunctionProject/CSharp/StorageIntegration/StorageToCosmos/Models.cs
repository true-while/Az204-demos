using System;
using System.Collections.Generic;
using System.Text;

namespace StorageToCosmos
{
    public class FlightRecord
    {
        public string FLIGHT_DATE { get; set; }
        public string DIRECTION { get; set; }
        public string AIRLINE_CODE { get; set; }
        public string AIRLINE_NAME { get; set; }
        public string FLIGHT_NUMBER { get; set; }
        public DateTime SCHEDULED_TIME { get; set; }
        public DateTime ACTUAL_TIME { get; set; }
        public string AIRPORT_CODE { get; set; }
        public string AIRPORT_CITY { get; set; }
        public string GATE { get; set; }
        public string BAGGAGE_CLAIM { get; set; }
        public string FLIGHT_STATUS { get; set; }
        public string REMARKS { get; set; }
        public DateTime FIDS_CREATE_DATE { get; set; }
        public DateTime ROW_ADD_DATE { get; set; }
        public string FLIGHT_KEY { get; set; }
        public string AIRCRAFT_REGISTRATION { get; set; }
        public string AIRCRAFT_SUBTYPECODE { get; set; }
    }

    public class FlightRecords
    {
        public List<FlightRecord> Flight_Record { get; set; }
    }
}

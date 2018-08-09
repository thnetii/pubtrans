using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using THNETII.Common;

namespace THNETII.PubTrans.AvinorFlydata.Model.Raw
{
    [DataContract]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportFeedFlight
    {
        private readonly DuplexConversionTuple<DateTime, long> dateOfOperation =
            ModelHelpers.GetUnixEpochConversion();
        private readonly DuplexConversionTuple<DateTime, long> scheduleTime =
            ModelHelpers.GetUnixEpochConversion();
        private readonly DuplexConversionTuple<string, CustomsType> customsType =
            ModelHelpers.GetCustomsTypeConversion();
        private readonly DuplexConversionTuple<string, FlightDirection> direction =
            ModelHelpers.GetFlightDirectionConversion();
        private readonly DuplexConversionTuple<string, IReadOnlyList<string>> via_airport =
            ModelHelpers.GetCommaSeparatedStringToTConversion(s => s, s => s, 7);
        private readonly DuplexConversionTuple<string, GateStatusCode> gateStatus =
            ModelHelpers.GetGateStatusCodeConversion();
        private readonly DuplexConversionTuple<string, BeltStatusCode> beltStatus =
            ModelHelpers.GetBeltStatusCodeConversion();
        private readonly DuplexConversionTuple<DateTime, long> beltStart =
            ModelHelpers.GetUnixEpochConversion();
        private readonly DuplexConversionTuple<DateTime, long> beltStop =
            ModelHelpers.GetUnixEpochConversion();
        private readonly DuplexConversionTuple<string, bool?> delayed =
            ModelHelpers.GetYesNoBooleanNullableConversion();

        [XmlAttribute("uniqueID")]
        [DataMember(Name = "uniqueID")]
        public uint UniqueId { get; set; }

        [DataMember(Name = "codeshareKey")]
        [XmlElement("codeshare_key")]
        public ulong CodeShareKey { get; set; }

        [DataMember(Name = "flightLegKey")]
        [XmlElement("flight_leg_key")]
        public ulong FlightLegKey { get; set; }

        /// <remarks>
        /// Consists of 3 parts:
        /// <list type="number">
        /// <item>Airline code (which can be 2-signs IATA code or 3-signs ICAO code).</item>
        /// <item>Flight number (maximum 4 signs).</item>
        /// <item>Suffix (maximum 1 sign).</item>
        /// </list>
        /// </remarks>
        [XmlElement("flight_id", DataType = "NMTOKEN")]
        [DataMember(Name = "flightId")]
        public string FlightId { get; set; }

        [XmlElement("airline", DataType = "NMTOKEN")]
        [DataMember(Name = "airline")]
        public string AirlineIata { get; set; }

        [XmlElement("airport")]
        [DataMember(Name = "airport")]
        public string AirportIata { get; set; }

        [XmlElement("date_of_operation"), IgnoreDataMember]
        public DateTime DateOfOperation
        {
            get => dateOfOperation.ConvertedValue == 0L
                ? ScheduleTime.Date
                : dateOfOperation.RawValue.Date;
            set => dateOfOperation.RawValue = value;
        }

        [XmlIgnore, DataMember(Name = "dateOfOperation")]
        public long DateOfOperationEpoch
        {
            get => dateOfOperation.ConvertedValue;
            set => dateOfOperation.ConvertedValue = value;
        }

        [XmlElement("dom_int", DataType = "NMTOKEN")]
        [DataMember(Name = "domInt")]
        public string CustomsTypeString
        {
            get => customsType.RawValue;
            set => customsType.RawValue = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public CustomsType CustomsType
        {
            get => customsType.ConvertedValue;
            set => customsType.ConvertedValue = value;
        }

        [IgnoreDataMember]
        [XmlElement("schedule_time")]
        public DateTime ScheduleTime
        {
            get => scheduleTime.RawValue;
            set => scheduleTime.RawValue = value;
        }

        [XmlIgnore]
        [DataMember(Name = "scheduleTime")]
        public long ScheduleTimeEpoch
        {
            get => scheduleTime.ConvertedValue;
            set => scheduleTime.ConvertedValue = value;
        }

        [XmlElement("arr_dep", DataType = "NMTOKEN")]
        [DataMember(Name = "arrDep")]
        public string DirectionString
        {
            get => direction.RawValue;
            set => direction.RawValue = value;
        }

        [XmlIgnore]
        public FlightDirection Direction
        {
            get => direction.ConvertedValue;
            set => direction.ConvertedValue = value;
        }

        [XmlElement("via_airport")]
        [DataMember(Name = "viaAirport")]
        public string ViaAirportString
        {
            get => via_airport.RawValue;
            set => via_airport.RawValue = value;
        }

        [XmlIgnore]
        public IReadOnlyList<string> ViaAirports
        {
            get => via_airport.ConvertedValue;
            set => via_airport.ConvertedValue = value;
        }

        [XmlElement("check_in")]
        [DataMember(Name = "checkIn")]
        public string CheckInCounter { get; set; }

        [XmlElement("stand")]
        [DataMember(Name = "stand")]
        public string Stand { get; set; }

        [XmlElement("gate")]
        [DataMember(Name = "gate")]
        public string Gate { get; set; }

        [XmlElement("gate_status")]
        [DataMember(Name = "gateStatus")]
        public string GateStatusCodeString
        {
            get => gateStatus.RawValue;
            set => gateStatus.RawValue = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public GateStatusCode GateStatusCode
        {
            get => gateStatus.ConvertedValue;
            set => gateStatus.ConvertedValue = value;
        }

        [XmlElement("status"), DataMember(Name = "status")]
        public AirportFlightInfo Status { get; set; }

        [XmlElement("belt"), DataMember(Name = "belt")]
        public string BaggageClaimBelt { get; set; }

        [XmlElement("belt_status"), DataMember(Name = "beltStatus")]
        public string BaggageClaimStatusCodeString
        {
            get => beltStatus.RawValue;
            set => beltStatus.RawValue = value;
        }

        [XmlIgnore, IgnoreDataMember]
        public BeltStatusCode BaggageClaimStatusCode
        {
            get => beltStatus.ConvertedValue;
            set => beltStatus.ConvertedValue = value;
        }

        [XmlElement("belt_start"), IgnoreDataMember]
        public DateTime BaggageClaimStart
        {
            get => beltStart.RawValue;
            set => beltStart.RawValue = value;
        }

        [XmlIgnore, DataMember(Name = "beltStart")]
        public long BaggageClaimStartEpoch
        {
            get => beltStart.ConvertedValue;
            set => beltStart.ConvertedValue = value;
        }

        [XmlElement("belt_stop"), IgnoreDataMember]
        public DateTime BaggageClaimStop
        {
            get => beltStop.RawValue;
            set => beltStop.RawValue = value;
        }

        [XmlIgnore, DataMember(Name = "beltStop")]
        public long BaggageClaimStopEpoch
        {
            get => beltStop.ConvertedValue;
            set => beltStop.ConvertedValue = value;
        }

        [XmlElement("delayed", DataType = "NMTOKEN")]
        public string DelayedString
        {
            get => delayed.RawValue;
            set => delayed.RawValue = value;
        }

        [XmlIgnore]
        public bool? Delayed
        {
            get => delayed.ConvertedValue;
            set => delayed.ConvertedValue = value;
        }

        private string DebuggerDisplay() => $"{nameof(AirportFeedFlight)}(Flight: {FlightId})";
    }

    [DataContract]
    [XmlType, DebuggerDisplay("{" + nameof(DebuggerDisplay) + "()}")]
    public class AirportFeedFlightListing
    {
        private readonly DuplexConversionTuple<DateTime, long> lastUpdate =
            ModelHelpers.GetUnixEpochConversion();

        [DataMember(Name = "flight")]
        [XmlElement("flight")]
        [SuppressMessage(category: null, "CA1819", Justification = "Must be array for XML serialization.")]
        public AirportFeedFlight[] Flights { get; set; }

        [IgnoreDataMember]
        [XmlAttribute("lastUpdate")]
        public DateTime LastUpdate
        {
            get => lastUpdate.RawValue;
            set => lastUpdate.RawValue = value;
        }

        [XmlIgnore]
        [DataMember(Name = "lastUpdate")]
        public long LastUpdateEpoch
        {
            get => lastUpdate.ConvertedValue;
            set => lastUpdate.ConvertedValue = value;
        }

        internal string DebuggerDisplay() => $"{nameof(AirportFeedFlightListing)}({nameof(Flights.Length)}: {Flights?.Length}, Last update: {LastUpdate})";
    }
}

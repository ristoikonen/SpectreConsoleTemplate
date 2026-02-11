using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TSR.Worker.Models
{
    public class AustralianPostcode
    {

            [JsonPropertyName ("id")]
            public int Id { get; set; }

        // System.Text.Json.Serialization.JsonInclude
            [JsonPropertyName("postcode")]
            public string Postcode { get; set; }

            [JsonPropertyName("locality")]
            public string Locality { get; set; }

            [JsonPropertyName("state")]
            public string State { get; set; }

            [JsonPropertyName("long")]
            public double Longitude { get; set; }

            [JsonPropertyName("lat")]
            public double Latitude { get; set; }

            [JsonPropertyName("Long_precise")]
            public double LongPrecise { get; set; }

            [JsonPropertyName("Lat_precise")]
            public double LatPrecise { get; set; }


            [JsonPropertyName("dc")]
            public string Dc { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("status")]
            public string Status { get; set; }


            [JsonPropertyName("sa3")]
            public string Sa3 { get; set; }

            [JsonPropertyName("sa3name")]
            public string Sa3Name { get; set; }

            [JsonPropertyName("sa4")]
            public string Sa4 { get; set; }

            [JsonPropertyName("sa4name")]
            public string Sa4Name { get; set; }

            [JsonPropertyName("region")]
            public string Region { get; set; }

            [JsonPropertyName("SA1_CODE_2021")]
            public string Sa1Code2021 { get; set; }

            [JsonPropertyName("SA1_NAME_2021")]
            public string Sa1Name2021 { get; set; }

            [JsonPropertyName("SA2_CODE_2021")]
            public string Sa2Code2021 { get; set; }

            [JsonPropertyName("SA2_NAME_2021")]
            public string Sa2Name2021 { get; set; }

            [JsonPropertyName("SA3_CODE_2021")]
            public string Sa3Code2021 { get; set; }

            [JsonPropertyName("SA3_NAME_2021")]
            public string Sa3Name2021 { get; set; }

            [JsonPropertyName("SA4_CODE_2021")]
            public string Sa4Code2021 { get; set; }

            [JsonPropertyName("SA4_NAME_2021")]
            public string Sa4Name2021 { get; set; }

            [JsonPropertyName("RA_2011")]
            public string Ra2011 { get; set; }

            [JsonPropertyName("RA_2016")]
            public string Ra2016 { get; set; }

            [JsonPropertyName("RA_2021")]
            public string Ra2021 { get; set; }

            [JsonPropertyName("RA_2021_NAME")]
            public string Ra2021Name { get; set; }

            [JsonPropertyName("MMM_2015")]
            public string Mmm2015 { get; set; }

            [JsonPropertyName("MMM_2019")]
            public string Mmm2019 { get; set; }


            [JsonPropertyName("ced")]
            public string Ced { get; set; }

            [JsonPropertyName("chargezone")]
            public string ChargeZone { get; set; }

            [JsonPropertyName("phn_code")]
            public string PhnCode { get; set; }

            [JsonPropertyName("phn_name")]
            public string PhnName { get; set; }

            [JsonPropertyName("lgaregion")]
            public string LgaRegion { get; set; }

            [JsonPropertyName("lgacode")]
            public string LgaCode { get; set; }

            [JsonPropertyName("electorate")]
            public string Electorate { get; set; }

            [JsonPropertyName("electoraterating")]
            public string ElectorateRating { get; set; }

            [JsonPropertyName("sed_code")]
            public string SedCode { get; set; }

            [JsonPropertyName("sed_name")]
            public string SedName { get; set; }

            [JsonPropertyName("altitude")]
            public string Altitude { get; set; }
        }
    }


﻿namespace VCCSharp.Models.Configuration
{
    public class FilePaths
    {
        //[DefaultPaths]
        public string Cassette { get; set; } = "";   //C:\CoCo
        public string Rom { get; private set; } = "";//## READ-ONLY ##//
        public string SerialCapture { get; set; } = "";   //C:\CoCo

        public string SetRom(string value) => Rom = value;
    }
}
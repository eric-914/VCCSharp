﻿namespace VCCSharp.Configuration.Support;

public interface IRangeSelect<T> where T : struct
{
    T Value { get; set; }
}

public interface IRangeSelect
{
    int Value { get; set; }
}


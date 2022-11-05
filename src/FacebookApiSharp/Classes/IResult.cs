/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Classes
{
    public interface IResult<out T>
    {
        bool Succeeded { get; }
        T Value { get; }
        ResultInfo Info { get; }
    }
}

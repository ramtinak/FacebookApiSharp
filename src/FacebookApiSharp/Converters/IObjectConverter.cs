/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

namespace FacebookApiSharp.Converters
{
    public interface IObjectConverter<out T, TT>
    {
        TT SourceObject { get; set; }
        T Convert();
    }
}
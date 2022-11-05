/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;

namespace FacebookApiSharp.Classes
{
    public interface IRequestDelay
    {
        TimeSpan Value { get; }
        bool Exist { get; }
        void Enable();
        void Disable();
    }
}
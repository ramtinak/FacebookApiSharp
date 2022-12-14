/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FacebookApiSharp.Enums
{
    public enum FacebookLoginResult
    {
        Success,
        WrongUserOrPassword,
        Unknown,
        //OAuthException,
        UnExpectedError,
        RenewPwdEncKeyPkg,
        SMScodeRequired
    }
}

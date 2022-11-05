/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */
namespace FacebookApiSharp.Classes
{
    public enum ResponseType
    {
        Unknown = 0,
        /// <summary>
        ///     Everything works fine
        /// </summary>
        OK = 1,
        WrongRequest = 2,
        UnExpectedResponse = 3,
        NetworkProblem = 4,
        InternalException = 5,
        CheckpointAccount = 6,
        OAuthException = 7,
        AccountBlocked = 8,
    }
}

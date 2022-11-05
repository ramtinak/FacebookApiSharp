/*
 * Developer: Ramtin Jokar [ Ramtinak@live.com ] [ License: MIT ]
 * 
 * 2022 - Dedicated Library
 */

using FacebookApiSharp.Classes.Models;
using System;

namespace FacebookApiSharp.Classes
{
    [Serializable]
    public class UserSessionData
    {
        public string User { get; set; }
        public string Password { get; set; }
        public string PublicKey { get; internal set; } = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0KTUlJQklqQU5CZ2txaGtpRzl3MEJBUUVGQUFPQ0FROEFNSUlCQ2dLQ0FRRUFyLzE2a2hJWWh1eklwVUF6QWt2YwpXSk1uTll0WmJRZ2JpY0M3RmdOQkpEblZMeHowbG5HUjJWNHZzN2w1S0h1VjkvK3lFZm1qRGU0VGcvRm5sK0I4ClR6TGdPZ0hpWG1LbFpIL1Y5TCt3MGh0cmVjUGlKVTc0Sjh2cHNwUHJNN3hhdlJnN256U0ZYZTYvTUxVNUMrb2gKejdRNjFJeFFrWGtvK0JQNnQ2elBrd3lVbnE2Y0NPckkrcm8xR2ZxNTdUMFRUUVh4WGJreW5FZmZyd3I3VEtsZQp1dEpvdG9sc1BwOTBNNnJvSU9nRzZqdzZ1TWlWYnYxSnp1UmM4V2hJbXBid2FLQmZVd0VYbUF4VzI2OW4yYzUvCmovN1RtUlpVckYxdk5RSm9ESk9OSTI2d09FeWRlRFVtYmtuek00NmVkUzNldnAwM1FNMlJFK2xHMXloQUlmeWcKdVFJREFRQUIKLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0tCg==";
        public string PublicKeyId { get; internal set; } = "181";
        public string MachineId { get; set; }
        public FacebookLoginSession LoggedInUser { get; set; }
        public static UserSessionData Empty => new UserSessionData();
    }
}
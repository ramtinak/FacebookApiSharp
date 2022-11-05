# FacebookApiSharp 
A Tiny Private Facebook API for .NET based on Android version.

IRANIAN DEVELOPERS

-----

Features:
- Login with facebook using email/phone and password
- Search users
- Get friends
- Get direct inbox friends
- Get direct top friends
- Send direct text
- Make new post
- Upload photo

## Note
This library is NOT intend to be getting new updates/features by me(Ramtin). 

I just shared the project to help people for their start.

This library won't publish to Nuget.org by me. 

Build this library by yourself. 
If you have problem with building it, try removing some platforms in the [FacebookApiSharp.csproj in line 4](https://github.com/ramtinak/FacebookApiSharp/blob/c67d0c208c3ee4ebdb643c015ffe7c06a891ace8/src/FacebookApiSharp/FacebookApiSharp.csproj#L4).

## Sample
You won't get any sample for this, this library is just like [InstagramApiSharp](https://github.com/ramtinak/InstagramApiSharp)'s library,
So, if you know how to use [InstagramApiSharp](https://github.com/ramtinak/InstagramApiSharp)'s library, you can use [FacebookApiSharp](https://github.com/ramtinak/FacebookApiSharp)'s library as well.

```C#
var userSession = new UserSessionData
{
    User = "Facebook Email or Phone number",
    Password = "Facebook Password"
};

IFacebookApi FacebookApi = FacebookApiBuilder.CreateBuilder()
    .SetUser(userSession)
    //.UseLogger(new FileDebugLogger(LogLevel.All))
    .UseLogger(new DebugLogger(LogLevel.All))
    .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
    // Session handler, set a file path to save/load your state/session data
    .SetSessionHandler(new FileSessionHandler { FilePath = StateFile })

    //// Setting up proxy if you needed
    //.UseHttpClientHandler(httpClientHandler)
    .Build();

FacebookApi.SimCountry = FacebookApi.NetworkCountry = "us"; // lower case < us => united states
FacebookApi.ClientCountryCode = "US"; // most be upper case <US =>  united states
FacebookApi.AppLocale = FacebookApi.DeviceLocale = "en_US"; // if you want en_US , no need to set these


// load old session
FacebookApi.SessionHandler?.Load();

if (!FacebookApi.IsUserAuthenticated) // if we weren't logged in
{
    await FacebookApi.SendLoginFlowsAsync();

    var loginResult = await FacebookApi.LoginAsync();
    if (loginResult.Succeeded)// logged in
    {
        //// library will saves session automatically, so no need to do this:
        //FacebookApi.SessionHandler?.Save();
        Connected();
        // after we logged in, we need to sends some requests
        await FacebookApi.SendAfterLoginFlowsAsync();
    }
    else
    {
        switch (loginResult.Value)
        {
            case FacebookLoginResult.WrongUserOrPassword:
                MessageBox.Show("Wrong Credentials (user or password is wrong)");

                break;

            case FacebookLoginResult.RenewPwdEncKeyPkg:
                MessageBox.Show("Press login button again");
                break;
        }
    }
}
else
{
    Connected();
}
```

## Terms and conditions
- Use this Library at your own risk.
- Don't ask me anything about this library, just use it and figure it out the problem you might ran into.

## Warning
Note 1: This library doesn't send all the request(s) that is sending by real Facebook app.

Note 2: This library doesn't send any logs to Facebook server.

## License
MIT.

## Legal
This code is in no way affiliated with, authorized, maintained, sponsored or endorsed by Facebook or any of its affiliates or subsidiaries. This is an independent and unofficial API wrapper.


Iranian developers - (c) 2022 | Paeez 1401.

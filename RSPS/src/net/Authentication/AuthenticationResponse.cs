using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSPS.src.net.Authentication
{
    public enum AuthenticationResponse
    {

        Failure = -1, // Waits for 2000ms and tries again while counting failures.
        CredentialsExchange = 0, // Exchanges session keys, player name, password, etc.
        AutoTryAgain = 1, // Waits for 2000ms and tries again.
        Successful = 2, // Client made a successful login.
        InvalidCredentials = 3, // "Invalid username or password."
        AccountDisabled = 4, // "Your account has been disabled. Please check your message-center for details."
        AlreadyLoggedIn = 5, // "Your account is already logged in. Try again in 60 secs..."
        GameWasUpdated = 6, // "RuneScape has been updated! Please reload this page."
        WorldFull = 7, // "This world is full. Please use a different world."
        LoginServerOffline = 8, // "Unable to connect. Login server offline."
        LoginLimitExceeded = 9, // "Login limit exceeded. Too many connections from your address."
        BadSessiondId = 10, // "Unable to connect. Bad session id."
        SessionRejected = 11, // "Login server rejected session. Please try again."
        MembersAccountRequired = 12, // "You need a members account to login to this world. Please subscribe, or use a different world."
        CouldNotCompleteLogin = 13, // "Could not complete login. Please try using a different world."
        ServerBeingUpdated = 14, // "The server is being updated. Please wait 1 minute and try again."
        SeeNotes = 15, // See the notes below.
        LoginAttemptsExceeded = 16, // "Login attempts exceeded. Please wait 1 minute and try again."
        StandingInMembersOnly = 17, // "You are standing in a members-only area. To play on this world move to a free area first."
        InvalidLoginServer = 20, // "Invalid loginserver requested. Please try using a different world."
        JustLeftAnotherWorld = 21, // "You have only just left another world. Your profile will be transferred in: (number) seconds."
        Unknown = 1337 // "Unexpected server response. Please try using a different world."

    }
}

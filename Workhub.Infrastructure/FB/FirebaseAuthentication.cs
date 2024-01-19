using FirebaseAdmin;
using FirebaseAdmin.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using FirebaseAuthentication.net;


namespace Workhub.Infrastructure.FB;

public class FirebaseAuthentication
{



    //public Task Login()

    //{
    //    FirebaseConfiguration configuration = new FirebaseConfiguration
    //    {
    //        AuthSecret = "YOUR_AUTH_SECRET",
    //        BasePath = "https://YOUR_PROJECT_ID.firebaseio.com/"
    //    };

    //    FirebaseAuth auth = new FirebaseAuth(configuration);

    //    try
    //    {
    //        // Sign in with email and password
    //        var user = await auth.SignInWithEmailAndPassword("user@example.com", "password");

    //        // User is authenticated
    //        Console.WriteLine($"User signed in: {user.Email}");
    //    }
    //    catch (FirebaseAuthException ex)
    //    {
    //        // Handle authentication error
    //        Console.WriteLine($"Authentication failed: {ex.Reason}");
    //    }

    //}
    // Initialize Firebase with your Firebase project credentials

    //public async Task Login(string email, string password)
    //{
    //    var client = FirebaseAuthClient();
    //    try
    //    {
    //        var auth = FirebaseAuth.DefaultInstance;

    //        var result = await auth.CreateSessionCookieAsync(email, password);

    //        Console.WriteLine($"User logged in successfully. UID: {result.User.Uid}");
    //    }
    //    catch (FirebaseAuthException e)
    //    {
    //        Console.WriteLine($"Login failed. Error: {e.Message}");
    //    }
    //}


    // ... (other methods remain the same)

}

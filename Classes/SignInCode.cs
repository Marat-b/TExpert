using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TExp.Classes
{
    public enum SignInCode
    {
        //
        // Summary:
        //     Sign in was successful
        Success = 0,
        //
        // Summary:
        //     User is locked out
        LockedOut = 1,
        //
        // Summary:
        //     Sign in requires addition verification (i.e. two factor)
        RequiresVerification = 2,
        //
        // Summary:
        //     Sign in failed
        Failure = 3,
        //
        // Change password is needed
        //
        ChangePassword=4


    }
}
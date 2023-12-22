using System;

public class WrongCodeException : Exception
{
    static string baseMessage = "The code of control should be 0, 1, 2, 3, 8, 9, 11, 13, 15 or 17.";

    public WrongCodeException() : base(baseMessage)
    {

    }
}

public class ControlNotAssigned : Exception
{

}

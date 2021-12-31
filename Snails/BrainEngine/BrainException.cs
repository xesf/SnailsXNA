using System;

namespace TwoBrainsGames.BrainEngine
{
  public class BrainException : System.Exception
  {
    public BrainException()
    {

    }

    public BrainException(string message):
      base(message)
    {

    }


    public BrainException(string message, Exception inner):
      base(message, inner)
    {

    }
  }
}

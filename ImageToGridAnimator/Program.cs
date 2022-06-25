using System;

namespace ImageToGridAnimator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Publics.InputArgs = args;

            Initalization.Start();
            Work.Start();
        }
    }
}

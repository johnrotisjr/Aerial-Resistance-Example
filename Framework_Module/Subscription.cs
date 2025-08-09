using System;

namespace Framework_Module
{
    public class Subscription
    {
        public readonly Delegate Delegate;
        public readonly bool IsOneTimeOnly;

        public Subscription(Delegate del, bool isOneTimeOnly)
        {
            Delegate = del;
            IsOneTimeOnly = isOneTimeOnly;
        }
    }
}
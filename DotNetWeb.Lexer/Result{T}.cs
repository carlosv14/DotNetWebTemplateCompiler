using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetWeb.Lexer
{
    public class Result<T>
    {
        internal Result(T value, Input reminder)
        {
            Value = value;
            Reminder = reminder;
            HasValue = true;
        }

        internal Result(Input reminder)
        {
            Reminder = reminder;
            HasValue = false;
        }

        public T Value { get; private set; }

        public Input Reminder { get; private set; }

        public bool HasValue { get; private set; }
    }
}
